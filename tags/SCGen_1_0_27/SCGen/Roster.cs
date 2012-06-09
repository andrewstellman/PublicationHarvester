using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Collections;
using Com.StellmanGreene.PubMed;
using Com.StellmanGreene.CSVReader;


namespace SCGen
{
    /// <summary>
    /// Class to wrap around the AAMC roster file
    /// </summary>
    class Roster
    {

        /// <summary>
        /// RosterData contains the AAMC roster entries read from the file
        /// </summary>
        public DataTable RosterData;

        /// <summary>
        /// MatchNames contains all of the values in match_name1 and match_name2 as the
        /// keys, with the array offset of the roster entry in RosterData
        /// </summary>
        public Hashtable MatchNames;


        /// <summary>
        /// Read the AAMC roster
        /// </summary>
        /// <param name="RosterFile">CSV or XML file that contains the roster</param>
        public Roster(string RosterFile)
        {
            string Folder = Path.GetDirectoryName(RosterFile);
            string Filename = Path.GetFileName(RosterFile);

//            int Rows = 0;
            if (Filename.ToLower().EndsWith(".csv"))
            {
                
                // Get the publication types from the input file

//                We're replacing ODBC CSV code with our own CSVReader
//
//                string ConnectionString =
//                    "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq="
//                    + Folder + ";";
//                OdbcConnection Connection = new OdbcConnection(ConnectionString);
//                OdbcDataAdapter DataAdapter = new OdbcDataAdapter
//                    (@"SELECT setnb, fname, mname, lname, suffix, match_name1, 
//                          match_name2, search_name1, search_name2, search_name3, 
//                          search_name4, query FROM [" + Filename + "]", Connection);
//                RosterData = new DataTable();
//                RosterData.TableName = Filename;
//                Rows = DataAdapter.Fill(RosterData);

                RosterData = CSVReader.ReadCSVFile(RosterFile, true);
                RosterData.TableName = Filename;
                if (File.Exists(Folder + "\\" + Filename + ".xml"))
                    File.Delete(Folder + "\\" + Filename + ".xml");
                RosterData.WriteXml(Folder + "\\" + Filename + ".xml", XmlWriteMode.WriteSchema);
            }
            else if (Filename.ToLower().EndsWith(".xml"))
            {
                RosterData = new DataTable();
                RosterData.ReadXml(RosterFile);
            }

            // Populate MatchNames -- initialize it with case insensitive comparer
            MatchNames = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            for (int i = 0; i < RosterData.Rows.Count; i++)
            {
                DataRow Row = RosterData.Rows[i];
                if (Row["match_name1"].ToString() != "")
                    AddMatch(Row["match_name1"].ToString(), i);
                if (Row["match_name2"].ToString() != "")
                    AddMatch(Row["match_name2"].ToString(), i);
            }
        }


        /// <summary>
        /// Add a matching name to the MatchNames hashtable
        /// </summary>
        /// <param name="MatchName">Name to add</param>
        /// <param name="RosterRow">Row in RosterData that the name corresponds to</param>
        private void AddMatch(string MatchName, int RosterRow)
        {
            // Each value in MatchNames is an ArrayList of integers that contains
            // the roster rows for that match
            if (!MatchNames.ContainsKey(MatchName))
                MatchNames[MatchName] = new ArrayList();
            ArrayList RosterRows = (ArrayList) MatchNames[MatchName];
            if (!RosterRows.Contains(RosterRow))
                RosterRows.Add(RosterRow);
            MatchNames[MatchName] = RosterRows;
        }

        /// <summary>
        /// Find a person in the AAMC roster
        /// </summary>
        /// <param name="NameToMatch">Medline-formatted name to find</param>
        /// <returns>A Person object containing the person if found (null if no match was found)</returns>
        public Person[] FindPerson(string NameToMatch)
        {
            // See if the matchname exists in the roster
            if (NameToMatch == null || !MatchNames.ContainsKey(NameToMatch))
            {
                return null;
            }
            else
            {
                // Fetch the ArrayList of row numbers
                ArrayList RosterRows = (ArrayList)MatchNames[NameToMatch];

                // Dimension the return array
                Person[] PeopleToReturn = new Person[RosterRows.Count];
                int Count = 0;
                foreach (int RowNum in RosterRows)
                {
                    Count++;
                    DataRow Row = RosterData.Rows[RowNum];

                    // Build the array of searchnames
                    string[] Names = new string[1];
                    Names[0] = Row["search_name1"].ToString();
                    for (int i = 2; i <= 4; i++)
                    {
                        string col = "search_name" + i.ToString();
                        if ((!Row[col].Equals(DBNull.Value)) && (Row[col].ToString() != ""))
                        {
                            Array.Resize(ref Names, Names.Length + 1);
                            Names[Names.GetUpperBound(0)] = Row[col].ToString();
                        }

                    }

                    // Add the person to the return array
                    PeopleToReturn[Count - 1] = new Person(Row["Setnb"].ToString(), Row["fname"].ToString(),
                        Row["mname"].ToString(), Row["lname"].ToString(), false,
                        Names, Row["query"].ToString());
                }

                return PeopleToReturn;
            }
        }
    }
}
