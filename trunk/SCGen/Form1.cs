using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using Com.StellmanGreene.PubMed;

namespace SCGen
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Roster object
        /// </summary>
        private Roster roster;


        public Form1()
        {
            InitializeComponent();
            UpdateStatus();
        }

        /// <summary>
        /// Pop up the About box
        /// </summary>
        private void About_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.ShowDialog(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Add the version to the status bar
            toolStripStatusLabel2.Text = "v" + Application.ProductVersion;

            // Set the log filename
            LogFilename.Text = (AppDomain.CurrentDomain.BaseDirectory
                + ("PublicationHarvester log " + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year
                + " " + DateTime.Now.Hour + DateTime.Now.Minute + ".log"));

            // Set the initial directory for open file dialog boxes
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Retrieve the DSNs from the registry
            GetODBCDataSourceNames();

            // Set the DSN dropdown listbox to its last value
            if (Application.CommonAppDataRegistry.GetValue("DSN", "").ToString().Length != 0)
            {
                DSN.Text = Application.CommonAppDataRegistry.GetValue("DSN", "").ToString();
            }

            // Set the people file textbox to its last value
            if (Application.CommonAppDataRegistry.GetValue("RosterFile", "").ToString().Length != 0)
            {
                RosterFile.Text = Application.CommonAppDataRegistry.GetValue("RosterFile", "").ToString();
            }
        }


        /// <summary>
        /// Retrieve the ODBC DSNs from the registry and populate the DSN dropdown listbox
        /// </summary>
        public void GetODBCDataSourceNames()
        {
            string DropDownListText = DSN.Text;
            string str;
            RegistryKey rootKey;
            RegistryKey subKey;
            string[] dsnList;
            DSN.Items.Clear();
            rootKey = Registry.LocalMachine;
            str = "SOFTWARE\\\\ODBC\\\\ODBC.INI\\\\ODBC Data Sources";
            subKey = rootKey.OpenSubKey(str);
            if (subKey != null)
            {
                dsnList = subKey.GetValueNames();
                DSN.Items.Add("System DSNs");
                DSN.Items.Add("================");

                foreach (string dsnName in dsnList)
                {
                    DSN.Items.Add(dsnName);
                }
                subKey.Close();
            }
            rootKey.Close();
            rootKey = Registry.CurrentUser;
            str = "SOFTWARE\\\\ODBC\\\\ODBC.INI\\\\ODBC Data Sources";
            subKey = rootKey.OpenSubKey(str);
            dsnList = subKey.GetValueNames();
            if (subKey != null)
            {
                DSN.Items.Add("================");
                DSN.Items.Add("User DSNs");
                DSN.Items.Add("================");
                foreach (string dsnName in dsnList)
                {
                    DSN.Items.Add(dsnName);
                }
                subKey.Close();
            }
            rootKey.Close();
            DSN.Text = DropDownListText;
        }


        /// <summary>
        /// Implement the "..." button to pop up the ODBC administrator (odbcad32.exe)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ODBCPanel_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = "odbcad32.exe";
            proc.Start();
        }

        /// <summary>
        /// Display the Open File dialog for the Roster file
        /// </summary>
        private void RosterFileDialog_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = RosterFile.Text;
            openFileDialog1.Filter = "Comma-Delimited Files (*.csv)|*.csv|XML Files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.Title = "Select the Roster file";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            RosterFile.Text = openFileDialog1.FileName;
        }

        /// <summary>
        /// When a DSN is changed, write it to the resgistry
        /// </summary>
        private void DSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            Application.CommonAppDataRegistry.SetValue("DSN", DSN.Text);
            UpdateStatus();
        }


        /// <summary>
        /// When the roster file is changed, write it to the resgistry
        /// </summary>
        private void RosterFile_TextChanged(object sender, EventArgs e)
        {
            Application.CommonAppDataRegistry.SetValue("RosterFile", RosterFile.Text);
            UpdateStatus();
        }



        public void UpdateStatus()
        {
            try
            {
                Database UpdateDB = new Database(DSN.Text);
                bool TablesCreated;
                int NumPeople;
                int NumHarvestedPeople;
                int NumPublications;
                int NumErrors;
                UpdateDB.GetStatus(out TablesCreated, out NumPeople, out NumHarvestedPeople, out NumPublications, out NumErrors);
                this.TablesCreated.Text = TablesCreated.ToString();
                this.TablesCreated.Refresh();
                this.People.Text = NumPeople.ToString();
                this.People.Refresh();
                this.PeopleHarvested.Text = NumHarvestedPeople.ToString();
                this.PeopleHarvested.Refresh();
                this.PublicationsFound.Text = NumPublications.ToString();
                this.PublicationsFound.Refresh();

                // People With Errors textbox -- set it to red if there are errors
                this.PeopleWithErrors.Text = NumErrors.ToString();
                if (NumErrors > 0)
                    PeopleWithErrors.BackColor = Color.Red;
                else
                    PeopleWithErrors.BackColor = People.BackColor;

                this.PeopleNotHarvested.Text = ((int)(NumPeople - NumHarvestedPeople)).ToString();

                // People Not Harvested textbox -- set it to red if there are unharvested
                // people and the software is not currently processing
                if (NumPeople - NumHarvestedPeople > 0)
                    PeopleNotHarvested.BackColor = Color.Red;
                else
                    PeopleNotHarvested.BackColor = People.BackColor;


                // Get the colleague status, enable buttons appropriately
                int DiadsFound;
                int ColleaguesFound;
                int ColleaguesHarvested;
                int ColleaguePublications;
                int StarsWithColleagues;
                ColleagueFinder.GetDBStatus(UpdateDB, out ColleaguesFound, out DiadsFound, out ColleaguesHarvested, out ColleaguePublications, out StarsWithColleagues);
                this.DiadsFound.Text = DiadsFound.ToString();
                this.ColleaguesFound.Text = ColleaguesFound.ToString();
                this.ColleaguesHarvested.Text = ColleaguesHarvested.ToString();
                this.ColleaguePublications.Text = ColleaguePublications.ToString();
                this.StarsWithColleagues.Text = StarsWithColleagues.ToString();

                // Colleagues With Errors textbox -- set it to red if there are errors 
                // (first check ColleaguesFound so we don't issue this SELECT against a database with no Colleagues table)
                if (ColleaguesFound > 0)
                {
                    try
                    {
                        NumErrors = UpdateDB.GetIntValue("SELECT Count(*) FROM Colleagues WHERE Error IS NOT NULL");
                        this.ColleaguesWithErrors.Text = NumErrors.ToString();
                    }
                    catch (Exception ex)
                    {
                        this.ColleaguesWithErrors.Text = ex.Message;
                        NumErrors = -1;
                    }
                    if (NumErrors != 0)
                        ColleaguesWithErrors.BackColor = Color.Red;
                    else
                        ColleaguesWithErrors.BackColor = People.BackColor;
                }

                // Enable/disable buttons
                if (ColleaguesFound == 0)
                {
                    RetrieveColleaguePublications.Enabled = false;
                    CopyPublicationsFromAnotherDB.Enabled = false;
                }
                else
                {
                    RetrieveColleaguePublications.Enabled = true;
                    CopyPublicationsFromAnotherDB.Enabled = true;
                }

                if (ColleaguePublications > 0)
                    RemoveFalseColleagues.Enabled = true;
                else
                    RemoveFalseColleagues.Enabled = false;


                // Roster status -- disable buttons if the roster is not loaded
                if (roster != null)
                {
                    this.RosterRows.Text = roster.RosterData.Rows.Count.ToString();
                    FindPotentialColleagues.Enabled = true;
                }
                else
                {
                    this.RosterRows.Text = "not loaded";
                    FindPotentialColleagues.Enabled = false;
                    RetrieveColleaguePublications.Enabled = false;
                    CopyPublicationsFromAnotherDB.Enabled = false;
                }

                Application.DoEvents();

            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// Read the roster
        /// </summary>
        private void ReadRoster_Click(object sender, EventArgs e)
        {
            if (roster != null)
            {
                if (MessageBox.Show("The roster is already loaded. Are you sure you want to reload it?",
                    "Reload the Roster?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.No)
                    return;
            }
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            roster = new Roster(RosterFile.Text);
            UpdateStatus();
            this.Cursor = Cursors.Default;
            this.Enabled = true;

            AddLogEntry("Read " + roster.RosterData.Rows.Count.ToString() + " from " + RosterFile.Text);
        }


        /// <summary>
        /// Generate the colleagues using the Colleagues class
        /// </summary>
        private void FindPotentialColleagues_Click(object sender, EventArgs e)
        {
            // If the tables aren't populated, don't look for colleagues.
            Database UpdateDB = new Database(DSN.Text);
            bool TablesCreated;
            int NumPeople;
            int NumHarvestedPeople;
            int NumPublications;
            int NumErrors;
            UpdateDB.GetStatus(out TablesCreated, out NumPeople, out NumHarvestedPeople, out NumPublications, out NumErrors);
            if ((!TablesCreated) || (NumPeople == 0) || (NumHarvestedPeople == 0))
            {
                MessageBox.Show("Please select a database that the Publication Harvester has been run on.", "Specify a Valid Database", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }


            // Reset the database by default -- if the user says "Yes" when asked,
            // then continue from the previous search.
            bool ResetDatabase = true;
            if (DiadsFound.Text != "0")
            {
                if (MessageBox.Show("Colleagues have already been found. Are you sure you want to re-find them (or continue the previous search)?",
                    "Re-find Colleagues?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.No)
                    return;

                if (MessageBox.Show("Should the colleague search continue where it was previously left off? (Click 'No' to reset the database and start finding new colleagues.)",
                    "Continue from previous search?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
                    ResetDatabase = false;
            }
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            Database DB = new Database(DSN.Text);

            // Use the ResetDatabase variable to determine whether or not to resume 
            // a past find. If resuming, get the Setnb's of stars from the StarColleagues
            // table so they can be skipped.
            if (ResetDatabase) 
                ColleagueFinder.CreateTables(DB);
            DataTable StarSetnbsResult = DB.ExecuteQuery("SELECT StarSetnb FROM StarColleagues");
            ArrayList StarSetnbs = new ArrayList();
            foreach (DataRow Row in StarSetnbsResult.Rows)
            {
                if (!(StarSetnbs.Contains(Row[0].ToString())))
                    StarSetnbs.Add(Row[0].ToString());
            }

            NCBI ncbi = new NCBI("Medline");
            ColleagueFinder finder = new ColleagueFinder(DB, roster, ncbi);
            People Stars = new People(DB);
            int NumStars = Stars.PersonList.Count;
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = NumStars;
            int Count = 0;
            foreach (Person Star in Stars.PersonList)
            {
                Count++;
                toolStripProgressBar1.Value = Count;
                statusStrip1.Refresh();
                Application.DoEvents();
                if (StarSetnbs.Contains(Star.Setnb))
                {
                    AddLogEntry("Already found colleagues for " + Star.Last + " (" + Star.Setnb + "), " + Count.ToString() + " of " + NumStars.ToString());
                }
                else
                {
                    AddLogEntry("Finding " + Star.Last + " (" + Star.Setnb + "), " + Count.ToString() + " of " + NumStars.ToString());
                    UpdateStatus();
                    Person[] Colleagues = finder.FindPotentialColleagues(Star);
                }
            }
            UpdateStatus();
            this.Cursor = Cursors.Default;
            this.Enabled = true;
        }
        
        /// <summary>
        /// Open the log in Notepad
        /// </summary>
        private void OpenInNotepad_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = "notepad.exe";
            proc.StartInfo.Arguments = LogFilename.Text;
            proc.Start();
        }


        /// <summary>
        ///  Set the status bar values
        /// </summary>
        public void SetProgressBar(int Min, int Max, int Value)
        {
            toolStripProgressBar1.Minimum = Min;
            toolStripProgressBar1.Maximum = Max;
            toolStripProgressBar1.Value = Value;
            Application.DoEvents();
        }


        /// <summary>
        /// Add an entry to the log -- write it to the file, add it to the listbox
        /// </summary>
        /// <param name="Entry">Entry to add</param>
        public void AddLogEntry(string Entry)
        {
            // Write the entry to the log file
            StreamWriter writer = new StreamWriter(LogFilename.Text, true);
            writer.WriteLine(DateTime.Now.ToString() + ": " + Entry);
            writer.Close();

            // Append it to the log listbox
            OpenInNotepad.Enabled = true;
            Log.Items.Add(DateTime.Now.ToString() + ": " + Entry);
            Log.SelectedIndex = Log.Items.Count - 1;
            //this.Refresh();
            //Log.Refresh();

            statusStrip1.Text = Entry;

            Application.DoEvents();
        }





        /// <summary>
        /// Add an entry to the log and display an error box
        /// </summary>
        /// <param name="Entry">Entry to add</param>
        private void AddLogEntryWithErrorBox(string Entry, string ErrorCaption)
        {
            AddLogEntry(Entry);
            MessageBox.Show(Entry, ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        /// <summary>
        /// Retrieve the publications for found colleagues
        /// </summary>
        private void RetrieveColleaguePublications_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            // Set the language restriction
            string[] Languages = null;
            if (LanguageList.Text != "")
            {
                Languages = LanguageList.Text.Split(',');
                foreach (string Language in Languages)
                    AddLogEntry("Adding language restriction: " + Language);
            }
            else
            {
                AddLogEntry("No language restriction added");
            }

            Database DB = new Database(DSN.Text);

            // Clear any publications left over after an interruption
            DB.ExecuteNonQuery(
                @"DELETE p.* FROM ColleaguePublications p, Colleagues c
                   WHERE c.Setnb = p.Setnb
                     AND c.Harvested = 0"
            );

            // Clear any errors
            int NumberOfErrors = DB.GetIntValue("SELECT Count(*) FROM Colleagues WHERE Error IS NOT NULL");
            AddLogEntry("Clearing " + NumberOfErrors.ToString() + " colleagues with errors");
            DB.ExecuteNonQuery(
                @"DELETE p.* FROM ColleaguePublications p, Colleagues c
                   WHERE c.Setnb = p.Setnb
                     AND c.Error IS NOT NULL"
            );
            DB.ExecuteNonQuery(
                @"UPDATE Colleagues 
                     SET Harvested = 0, Error = NULL, ErrorMessage = NULL
                   WHERE Error IS NOT NULL"
            );
            NumberOfErrors = DB.GetIntValue("SELECT Count(*) FROM Colleagues WHERE Error = 1");
            if (NumberOfErrors != 0)
            {
                AddLogEntry("WARNING: " + NumberOfErrors + " errors were not cleared!");
            }
            UpdateStatus();

            // Retrieve the publications for each unharvested colleague
            NCBI ncbi = new NCBI("Medline");
            ColleagueFinder finder = new ColleagueFinder(DB, roster, ncbi);
            People Colleagues = new People(DB, "Colleagues");
            int Total = Colleagues.PersonList.Count;
            int Count = 0;
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = Total;
            foreach (Person person in Colleagues.PersonList)
            {
                Count++;
                toolStripProgressBar1.Value = Count;
                statusStrip1.Refresh();
                if (person.Harvested == false)
                {
                    AddLogEntry("Harvesting publications for colleague " + person.Last + " (" + person.Setnb + "), " + Count.ToString() + " of " + Total.ToString());
                    UpdateStatus();

                    // Check for existing publications in ColleaguePublications
                    ArrayList Parameters = new ArrayList();
                    Parameters.Add(Database.Parameter(person.Setnb));
                    int ExistingPublications = DB.GetIntValue(
                        "SELECT Count(*) FROM ColleaguePublications WHERE Setnb = ?", Parameters);
                    if (ExistingPublications > 0)
                    {
                        DB.ExecuteNonQuery("UPDATE Colleagues SET Harvested = 1 WHERE Setnb = ?", Parameters);
                        AddLogEntry(ExistingPublications.ToString() + " publications already exist for colleague " + person.Last + " (" + person.Setnb + "), " + Count.ToString() + " of " + Total.ToString());
                    }
                    else
                    {
                        Person[] personArray = new Person[1];
                        personArray[0] = person;
                        try
                        {
                            List<int> allowedPubTypes = new List<int>();
                            foreach (string type in AllowedPubTypeCategories.Text.Split(','))
                                allowedPubTypes.Add(int.Parse(type));
                            finder.GetColleaguePublications(personArray, Languages, allowedPubTypes);
                            GC.Collect(); // no need to wait for finalizers, because they don't do anything
                        }
                        catch (Exception ex)
                        {
                            AddLogEntry(ex.Message);

                            // Set the error for the colleague
                            Parameters = new ArrayList();
                            Parameters.Add(Database.Parameter(ex.Message));
                            Parameters.Add(Database.Parameter(person.Setnb));
                            DB.ExecuteNonQuery(
                                @"UPDATE Colleagues 
                                     SET Error = 1, ErrorMessage = ?
                                   WHERE Setnb = ?", Parameters);

                        }
                    }
                    UpdateStatus();
                }
                else
                {
                    AddLogEntry("Already harvested publications for colleague " + person.Last + " (" + person.Setnb + "), " + Count.ToString() + " of " + Total.ToString());
                }
            }

            this.Cursor = Cursors.Default;
            this.Enabled = true;
        }


        /// <summary>
        /// Remove any false colleagues from the database
        /// </summary>
        private void RemoveFalseColleagues_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            Database DB = new Database(DSN.Text);
            int Before = DB.GetIntValue("SELECT Count(*) FROM StarColleagues");
            ColleagueFinder.RemoveFalseColleagues(DB, this);
            int After = DB.GetIntValue("SELECT Count(*) FROM StarColleagues"); ;
            int Removed = Before - After;
            AddLogEntry("Removed " + Removed.ToString() + " false colleague" +
                ((Removed == 1) ? "" : "s"));

            UpdateStatus();

            this.Cursor = Cursors.Default;
            this.Enabled = true;

        }


        /// <summary>
        /// Pop up the Harvesting Reports dialog box
        /// </summary>
        private void GenerateReports_Click(object sender, EventArgs e)
        {
            if (DSN.Text == "")
            {
                MessageBox.Show("Please specify a valid ODBC data source that points to a MySQL 5.1 server", "Unable to Initialize Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ReportsDialog reportsDialog = new ReportsDialog();
            reportsDialog.DB = new Database(DSN.Text);
            reportsDialog.MainForm = this;
            this.Enabled = false;
            reportsDialog.ShowDialog(this);
            this.Enabled = true;

        }

        private void DSN_Click(object sender, EventArgs e)
        {
            GetODBCDataSourceNames();
        }


        /// <summary>
        /// Copy the publications for the square roster
        /// </summary>
        private void CopyPublicationsFromAnotherDB_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            CopyPublicationsDialog Dialog = new CopyPublicationsDialog(this, new Database(DSN.Text), AllowedPubTypeCategories.Text);
            Dialog.ShowDialog(this);

            this.Cursor = Cursors.Default;
            this.Enabled = true;
        }


    }
}