/*
 *                         Stars Colleague Generator
 *              Copyright © 2003-2019 Stellman & Greene Consulting
 *      Developed for Joshua Zivin and Pierre Azoulay, Columbia University
 *            http://www.stellman-greene.com/PublicationHarvester
 *
 * This program is free software; you can redistribute it and/or modify it under
 * the terms of the GNU General Public License as published by the Free Software 
 * Foundation; either version 2 of the License, or (at your option) any later 
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT 
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
 * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with 
 * this program (GPL.txt); if not, write to the Free Software Foundation, Inc., 51 
 * Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using Microsoft.Win32;
using Com.StellmanGreene.PubMed;



namespace PublicationHarvester
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Store the last DSN to prevent unnecessary checks for interrupted data
        /// </summary>
        private string lastDSNSelected = "";

        public Form1()
        {
            InitializeComponent();
        }

        #region Button Handlers

        /// <summary>
        /// Implement the "Harvest PublicationList" button
        /// </summary>
        private void HarvestPublications_Click(object sender, EventArgs e)
        {
            if (DSN.Text == "")
            {
                MessageBox.Show("Please specify a valid ODBC data source that points to a MySQL 5.5 server", "Unable to Initialize Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
                

            // If the harvester was previously interrupted or there were errors,
            // give an extra warning to the user.
            bool TablesCreated;
            int NumPeople;
            int NumHarvestedPeople;
            int NumPublications;
            int NumErrors;
            try
            {
                if ( (UpdateDB == null) || (UpdateDB.DSN != DSN.Text) )
                    UpdateDB = new Database(DSN.Text);
                UpdateDB.GetStatus(out TablesCreated, out NumPeople, out NumHarvestedPeople, out NumPublications, out NumErrors);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to initialize database '" + DSN.Text + "': " + ex.Message, "Error Initializing Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                SetGUIEnabled(true);
                return;
            }
            DialogResult Result;
            if (NumErrors > 0)
            {
                Result = MessageBox.Show("WARNING: Some people from a previous harvest were not processed because they generated errors. If you start a new harvest, the results of the previous one will be discarded. Are you sure you want to continue?",
                    "Discard results of the previous harvest?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (Result == DialogResult.Cancel)
                    return;
            }
            else if ((NumHarvestedPeople > 0) && (NumHarvestedPeople < NumPeople))
            {
                Result = MessageBox.Show("WARNING: A previous harvest was interrupted. If you start a new harvest, the results of the previous one will be discarded. Are you sure you want to continue?",
                    "Discard results of the previous harvest?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (Result == DialogResult.Cancel)
                    return;
            }


            if (!ReadyForHarvest("Reset database and get new publications?"))
                return;

            this.Cursor = Cursors.WaitCursor;
            SetGUIEnabled(false);
            Processing = true;
            InterruptClicked = false;

            Harvest(PeopleFile.Text, PublicationTypeFile.Text, false);

            Processing = false;
            SetGUIEnabled(true);
            this.Cursor = Cursors.Default;

            AddLogEntry("Done.");
        }



        public bool InterruptClicked;
        /// <summary>
        /// Interrupt the current processing
        /// </summary>
        private void Interrupt_Click(object sender, EventArgs e)
        {
            InterruptClicked = true;
        }
        #endregion

        /// <summary>
        /// Initialize the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Add the version to the status bar
            toolStripStatusLabel2.Text = "v" + Application.ProductVersion;

            // Set the log file
            LogFilename.Text = (Environment.GetEnvironmentVariable("TMP")
                + ("PublicationHarvester log " + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year
                + " " + DateTime.Now.Hour + DateTime.Now.Minute + ".log"));
            

            // Set the initial directory for open file dialog boxes
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Retrieve the DSNs from the registry
            GetODBCDataSourceNames();

            //Don't load the DSN to avoid a long check
            //
            //// Set the DSN dropdown listbox to its last value
            //if (Application.CommonAppDataRegistry.GetValue("DSN", "").ToString().Length != 0)
            //{
            //    DSN.Text = Application.CommonAppDataRegistry.GetValue("DSN", "").ToString();
            //}

            // Set the people file textbox to its last value
            string peopleFile = PubMed.Settings.GetValueString("PeopleFile", "");
            if (!String.IsNullOrEmpty(peopleFile))
            {
                PeopleFile.Text = peopleFile;
            }

            // Set the publication type file textbox to its last value
            string publicationTypeFile = PubMed.Settings.GetValueString("PublicationTypeFile", "");
            if (!String.IsNullOrEmpty(publicationTypeFile))
            {
                PublicationTypeFile.Text = publicationTypeFile;
            }

            // Enable the GUI objects
            SetGUIEnabled(true);
        }

        /// <summary>
        /// When a DSN is changed, write it to the resgistry
        /// </summary>
        private void DSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            PubMed.Settings.SetValue("DSN", DSN.Text);

            // The GUI is enabled since this could change, so we should call
            // SetGUIEnabled(true) in order to set the "Resume Harvesting"
            // button's Enabled property properly.
            SetGUIEnabled(true);
        }


        /// <summary>
        /// When the people file is changed, write it to the resgistry
        /// </summary>
        private void PeopleFile_TextChanged(object sender, EventArgs e)
        {
            PubMed.Settings.SetValue("PeopleFile", PeopleFile.Text);
        }

        /// <summary>
        /// When the publication type file is changed, write it to the resgistry
        /// </summary>
        private void PublicationTypeFile_TextChanged(object sender, EventArgs e)
        {
            PubMed.Settings.SetValue("PublicationTypeFile", PublicationTypeFile.Text);
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
        /// Make sure that the proper selections have been made so that 
        /// the harvesting can begin
        /// </summary>
        /// <param name="Message">Message to display in confirmation box</param>
        /// <returns>True if we're ready, false otherwise</returns>
        private bool ReadyForHarvest(string Message)
        {
            // Make sure a DSN is selected
            if (DSN.Text.Length == 0)
            {
                MessageBox.Show("Select an ODBC data source.", "Unable to harvest publications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Make sure a People file is selected
            if (PeopleFile.Text.Length == 0)
            {
                MessageBox.Show("Select a People file.", "Unable to harvest publications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Make sure a Publication Type file is selected
            if (PublicationTypeFile.Text.Length == 0)
            {
                MessageBox.Show("Select a Publication Type file.", "Unable to harvest publications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            DialogResult result = MessageBox.Show(Message, "Are you sure?", MessageBoxButtons.YesNo , MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return false;

            return true;
        }


        /// <summary>
        /// Enable or disable GUI elements
        /// </summary>
        private void SetGUIEnabled(bool Enabled)
        {
            UpdateDatabaseStatus();

            // Reset the toolstrip
            toolStripProgressBar1.Value = 0;
            toolStripStatusLabel1.Text = "";
            
            // Enable or disable the GUI widgets
            DSN.Enabled = Enabled;
            ODBCPanel.Enabled = Enabled;
            PeopleFile.Enabled = Enabled;
            PeopleFileDialog.Enabled = Enabled;
            PublicationTypeFile.Enabled = Enabled;
            PublicationTypeFileDialog.Enabled = Enabled;
            HarvestPublications.Enabled = Enabled;
            HarvestingReports.Enabled = Enabled;

            // Enable add/remove buttons only if the databases are created
            AddUpdatePeople.Enabled = Enabled && (TablesCreated.Text.ToLower() == "true");
            RemovePeople.Enabled = Enabled && (TablesCreated.Text.ToLower() == "true");
                
            // Enable the "Interrupt" button only if the system is NOT processing
            Interrupt.Enabled = !Enabled;

            // Enable "Open in Notepad" only if the file exists
            if (File.Exists(LogFilename.Text))
                OpenInNotepad.Enabled = true;
            else
                OpenInNotepad.Enabled = false;

            // Only enable the Clear/Resume button (ResumeHarvesting) if the selected DSN
            // is valid and either interrupted data or unharvested people exist
            try
            {
                Database DB = new Database(DSN.Text);
                Harvester harvester = new Harvester(DB);
                if (lastDSNSelected != DSN.Text)
                {
                    lastDSNSelected = DSN.Text;
                    AddLogEntry("Checking for interrupted data");
                    if ((CheckForInterruptedData.Checked && harvester.InterruptedDataExists())
                        || harvester.UnharvestedPeopleExist())
                        ResumeHarvesting.Enabled = Enabled;
                    else
                        ResumeHarvesting.Enabled = false;

                    if (CheckForInterruptedData.Checked == false)
                        AddLogEntry("Warning: skipped check for interrupted data");
                }
                else
                {
                    // Enable the resume harvesting button if the form is enabled and unharvested people exist
                    ResumeHarvesting.Enabled = Enabled && harvester.UnharvestedPeopleExist();
                }
            }
            catch
            {
                ResumeHarvesting.Enabled = false;
            }
        }


        private Database UpdateDB;
        /// <summary>
        /// Update the "Database Status" textboxes
        /// </summary>
        public void UpdateDatabaseStatus()
        {
            if (!UpdateStatusDuringHarvest.Checked)
            {
                Application.DoEvents();
                return;
            }

            try
            {
                if ( (UpdateDB == null) || (UpdateDB.DSN != DSN.Text) )
                    UpdateDB = new Database(DSN.Text);
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
                if (!Processing && (NumPeople - NumHarvestedPeople > 0))
                    PeopleNotHarvested.BackColor = Color.Red;
                else
                    PeopleNotHarvested.BackColor = People.BackColor;
                Application.DoEvents();
            }
            catch
            {
                this.TablesCreated.Text = "Error";
                this.TablesCreated.Refresh();
                this.People.Text = "Error";
                this.People.Refresh();
                this.PeopleHarvested.Text = "Error";
                this.PeopleHarvested.Refresh();
                this.PublicationsFound.Text = "Error";
                this.PublicationsFound.Refresh();
                Application.DoEvents();
            }
        }



        /// <summary>
        /// Harvest each of the publications in the people file
        /// </summary>
        /// <param name="PeopleFile">Filename of the people file</param>
        /// <param name="PublicationTypeFile">Filename of publication type file</param>
        /// <param name="ContinueFromInterruption">True if continuing from a previously interrupted harvest</param>
        public void Harvest(string PeopleFile, string PublicationTypeFile, bool ContinueFromInterruption)
        {
            // First verify that the files exist
            if (!File.Exists(PeopleFile))
            {
                MessageBox.Show("The People file '" + PeopleFile + "' does not exist", "People file not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!File.Exists(PublicationTypeFile))
            {
                MessageBox.Show("The Publication Type file '" + PublicationTypeFile + "' does not exist", "Publication Type file not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            UpdateDatabaseStatus(); 
            if (ContinueFromInterruption)
                AddLogEntry("Continuing interrupted harvest");
            else
                AddLogEntry("Beginning harvesting");

            // Reset lastDSNSelected to make sure that the next check for interrupted data is NOT skipped
            lastDSNSelected = "";

            // Initialize the harvester
            Harvester harvester;
            Database DB;

            // Initialize objects
            try
            {
                DB = new Database(DSN.Text);
                harvester = new Harvester(DB);

                // Set the language restriction
                string[] Languages;
                if (LanguageList.Text != "")
                {
                    Languages = LanguageList.Text.Split(',');
                    harvester.Languages = Languages;
                    foreach (string Language in Languages) 
                        AddLogEntry("Adding language restriction: " + Language);
                }
                else
                {
                    AddLogEntry("No language restriction added");
                }
            }
            catch (Exception ex)
            {
                AddLogEntryWithErrorBox(ex.Message, "Unable to begin harvesting");
                return;
            }

            // Initializethe database
            try
            {
                if (!ContinueFromInterruption)
                {
                    AddLogEntry("Initializing the database");
                    harvester.CreateTables();
                    UpdateDatabaseStatus();
                }
            }
            catch (Exception ex)
            {
                AddLogEntryWithErrorBox(ex.Message, "Unable to initialize database");
                return;
            }


            PublicationTypes pubTypes;
            if (ContinueFromInterruption)
            {
                // If we're continuing, read the publication types from the databse
                try
                {
                    AddLogEntry("Reading publication types from the database");
                    pubTypes = new PublicationTypes(DB);
                }
                catch (Exception ex)
                {
                    AddLogEntryWithErrorBox(ex.Message, "Unable to read publication types");
                    return;
                }
                // Remove any data left over from the interruption
                if (ContinueFromInterruption)
                {
                    AddLogEntry("Removing any data left over from the previous interruption");
                    harvester.ClearDataAfterInterruption();
                }
                UpdateDatabaseStatus();
            }
            else
            {
                // Read the publication types from the file and write them to the database
                try
                {
                    AddLogEntry("Writing publication types to database");
                    pubTypes = new PublicationTypes(Path.GetDirectoryName(PublicationTypeFile), Path.GetFileName(PublicationTypeFile));
                    pubTypes.WriteToDB(DB);
                    UpdateDatabaseStatus();
                }
                catch (Exception ex)
                {
                    AddLogEntryWithErrorBox(ex.Message, "Unable to read publication types");
                    return;
                }

                // Read the people
                try
                {
                    AddLogEntry("Reading people from " + Path.GetFileName(PeopleFile) + " and writing them to the database");
                    harvester.ImportPeople(PeopleFile);
                    UpdateDatabaseStatus();
                }
                catch (Exception ex)
                {
                    AddLogEntryWithErrorBox(ex.Message, "Unable to read the people from " + Path.GetFileName(PeopleFile));
                    return;
                }
            }


            // Make an anonymous callback function that keeps track of the callback data
            Harvester.GetPublicationsStatus StatusCallback = delegate(int number, int total, int averageTime)
            {
                // No need to update the progress bar for this -- it leads to a messy-looking UI because it's also updated for the person total
                // toolStripProgressBar1.Minimum = 0;
                // toolStripProgressBar1.Maximum = total;
                // toolStripProgressBar1.Value = number;
                toolStripStatusLabel1.Text = "Reading publication " + number.ToString() + " of " + total.ToString() + " (" + averageTime.ToString() + " ms average)";
                UpdateDatabaseStatus();
                Application.DoEvents();
            };

            // Make an anonymous callback function that logs any messages passed back
            Harvester.GetPublicationsMessage MessageCallback = delegate(string Message, bool StatusBarOnly)
            {
                if (StatusBarOnly)
                {
                    toolStripStatusLabel1.Text = Message;
                    //this.Refresh();
                    //statusStrip1.Refresh();
                    Application.DoEvents();
                } else 
                    AddLogEntry(Message);
            };

            // Make an anonymous callback function to return the value of Interrupt for CheckForInterrupt
            Harvester.CheckForInterrupt InterruptCallback = delegate()
            {
                return InterruptClicked;
            };

            // Get each person's publications and write them to the database
            NCBI ncbi = new NCBI("medline");
            if (NCBI.ApiKeyExists)
            {
                AddLogEntry("Using API key: " + NCBI.ApiKeyPath);
            } else
            {
                AddLogEntry("Performance is limited to under 3 requests per second.");
                AddLogEntry("Consider pasting an API key into " + NCBI.ApiKeyPath);
                AddLogEntry("For more information, see https://ncbiinsights.ncbi.nlm.nih.gov/2017/11/02/new-api-keys-for-the-e-utilities/");
            }
            People people = new People(DB);
            int totalPeopleInPersonList = people.PersonList.Count;
            int numberOfPeopleProcessed = 0;

            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = totalPeopleInPersonList;

            foreach (Person person in people.PersonList)
            {
                numberOfPeopleProcessed++;
                try
                {
                    // If continuing from interruption, only harvest unharvested people
                    if ((!ContinueFromInterruption) || (!person.Harvested))
                    {
                        AddLogEntry("Getting publications for " + person.Last + " (" + person.Setnb + "), number " + numberOfPeopleProcessed.ToString() + " of " + totalPeopleInPersonList.ToString());
                        toolStripProgressBar1.Value = numberOfPeopleProcessed;
                        double AverageMilliseconds;
                        int NumPublications = harvester.GetPublications(ncbi, pubTypes, person, StatusCallback, MessageCallback, InterruptCallback, out AverageMilliseconds);
                        if (InterruptClicked)
                        {
                            AddLogEntry("Publication harvesting was interrupted");
                            UpdateDatabaseStatus();
                            return;
                        }
                        AddLogEntry("Wrote " + NumPublications.ToString() + " publications, average write time " + Convert.ToString(Math.Round(AverageMilliseconds, 1)) + " ms");
                        UpdateDatabaseStatus();
                    }
                    else
                    {
                        AddLogEntry("Already retrieved publications for " + person.Last + " (" + person.Setnb + ")");
                    }
                }
                catch (Exception ex)
                {
                    AddLogEntry("An error occurred while reading publications for " + person.Last + " (" + person.Setnb + "): " + ex.Message);
                }
            }

            AddLogEntry("Finished reading publications");
            UpdateDatabaseStatus();
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
        /// Display the Open File dialog for the People file
        /// </summary>
        private void PeopleFileDialog_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = PeopleFile.Text;
            openFileDialog1.Filter = "Microsoft Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx|Comma-delimited Text Files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.Title = "Select the People file";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            PeopleFile.Text = openFileDialog1.FileName;
        }




        /// <summary>
        /// Display the Open File dialog for the Publication type file
        /// </summary>
        private void PublicationTypeFileDialog_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = PublicationTypeFile.Text;
            openFileDialog1.Filter = "Comma-delimited text files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.Title = "Select the Publication Type file";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            PublicationTypeFile.Text = openFileDialog1.FileName;
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
        /// Resume a previously interrupted harvest
        /// </summary>
        private void ResumeHarvesting_Click(object sender, EventArgs e)
        {
            if (!ReadyForHarvest("Continue previously interrupted harvest?"))
                return;

            this.Cursor = Cursors.WaitCursor;
            SetGUIEnabled(false);
            Processing = true;
            InterruptClicked = false;
            
            Harvest(PeopleFile.Text, PublicationTypeFile.Text, true);
            
            Processing = false;
            SetGUIEnabled(true);
            this.Cursor = Cursors.Default;
        }


        /// <summary>
        /// Processing is set to true when the software begins processing. This is
        /// used to tell Form1_FormClosing to prompt when the user closes the window.
        /// Note: The form initializes this to false
        /// </summary>
        public bool Processing;

        /// <summary>
        /// If the user clicks the X button or otherwise closes the form while the
        /// harvester is running, prompt to make sure they want to interrupt the
        /// process. Don't worry about what's in the database -- the fault-tolerance
        /// will take care of everything when the user resumes the harvest.
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Processing)
            {
                DialogResult Result = MessageBox.Show("Are you sure you want to interrupt the processing and quit?", "Verify Closing Program", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Repopulate the DSN list when the user clicks on the DSN listbox
        /// </summary>
        private void DSN_Click(object sender, EventArgs e)
        {
            // Re-retrieve all of the DSNs
            GetODBCDataSourceNames();
        }


        /// <summary>
        /// Pop up the Harvesting Reports dialog box
        /// </summary>
        private void HarvestingReports_Click(object sender, EventArgs e)
        {
            if (DSN.Text == "")
            {
                MessageBox.Show("Please specify a valid ODBC data source that points to a MySQL 5.5 server", "Unable to Initialize Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            bool TablesCreated;
            int NumPeople;
            int NumHarvestedPeople;
            int NumPublications;
            int NumErrors;
            try
            {
                if ((UpdateDB == null) || (UpdateDB.DSN != DSN.Text))
                    UpdateDB = new Database(DSN.Text);
                UpdateDB.GetStatus(out TablesCreated, out NumPeople, out NumHarvestedPeople, out NumPublications, out NumErrors);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to initialize database '" + DSN.Text + "': " + ex.Message, "Error Initializing Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                SetGUIEnabled(true);
                return;
            }

            ReportsDialog reportsDialog = new ReportsDialog();
            reportsDialog.DB = new Database(DSN.Text);
            reportsDialog.MainForm = this;
            this.Enabled = false;
            reportsDialog.ShowDialog(this);
            this.Enabled = true;
        }

        /// <summary>
        /// Pop up the About box
        /// </summary>
        private void About_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.ShowDialog(this);
        }

        /// <summary>
        /// Add or update people in the people file
        /// </summary>
        private void AddUpdatePeopleFile_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.FileName = PeopleFile.Text;
                openFileDialog1.Filter = "Microsoft Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx|Comma-delimited Text Files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog1.Title = "Select the People file to use for adding/updating";
                openFileDialog1.CheckFileExists = true;
                openFileDialog1.CheckPathExists = true;
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.Cancel)
                    return;

                Database DB = new Database(DSN.Text);
                int Count = PeopleMaintenance.AddUpdate(DB, openFileDialog1.FileName);
                UpdateDatabaseStatus();
                SetGUIEnabled(true);
                MessageBox.Show("Added/updated " + Count.ToString() + " people", "Add/Update Successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to add/update people", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Remove people from the people file
        /// </summary>
        private void RemovePeople_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.FileName = PeopleFile.Text;
                openFileDialog1.Filter = "Microsoft Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx|Comma-delimited Text Files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog1.Title = "Select the People file to use for adding/updating";
                openFileDialog1.CheckFileExists = true;
                openFileDialog1.CheckPathExists = true;
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.Cancel)
                    return;

                Database DB = new Database(DSN.Text);
                int Count = PeopleMaintenance.Remove(DB, openFileDialog1.FileName);
                UpdateDatabaseStatus();
                SetGUIEnabled(true);
                MessageBox.Show("Removed " + Count.ToString() + " people", "Add/Update Successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to remove people", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Update the status when the box is checked 
        /// </summary>
        private void UpdateStatusDuringHarvest_CheckedChanged(object sender, EventArgs e)
        {
            if (!UpdateStatusDuringHarvest.Checked)
            {
                UpdateDatabaseStatus();

                // Only set reset the GUI enabled the harvest is not currently happening.
                if (!Interrupt.Enabled)
                    SetGUIEnabled(true);
            }
        }
    }
}