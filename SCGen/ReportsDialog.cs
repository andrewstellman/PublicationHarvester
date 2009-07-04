using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Com.StellmanGreene.PubMed;
using System.Collections;

namespace SCGen
{

    public partial class ReportsDialog : Form
    {
        public Database DB;
        public Form1 MainForm;

        public ReportsDialog()
        {
            InitializeComponent();

            // Initialize the People Report Sections list
            peopleReportSections.Items.Add("1+2+3");

        }

        /// <summary>
        /// Browse for a new folder
        /// </summary>
        private void SpecifyFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Folder.Text;
            DialogResult Result = folderBrowserDialog1.ShowDialog();
            if (Result == DialogResult.OK)
            {
                Folder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        /// <summary>
        /// Load the dialog box: set the folder to the current folder, read the 
        /// journal weights filename from the registry
        /// </summary>
        private void ReportsDialog_Load(object sender, EventArgs e)
        {
            Folder.Text = AppDomain.CurrentDomain.BaseDirectory;

            // Set the people file textbox to its last value
            if (Application.CommonAppDataRegistry.GetValue("JournalWeightsFile", "").ToString().Length != 0)
            {
                JournalWeights.Text = Application.CommonAppDataRegistry.GetValue("JournalWeightsFile", "").ToString();
            }
        }

        /// <summary>
        /// When the folder changes, check for existing files
        /// </summary>
        private void Folder_TextChanged(object sender, EventArgs e)
        {
            CheckForExistingFiles();
        }


        /// <summary>
        /// If files already exist in the specified folder, enable the proper
        /// radio buttons.
        /// </summary>
        private void CheckForExistingFiles()
        {
            if (File.Exists(Folder.Text + "\\" + Colleagues.Text))
                PeopleRadioButtons.Enabled = true;
            else
                PeopleRadioButtons.Enabled = false;

            if (File.Exists(Folder.Text + "\\" + Publications.Text))
                PublicationsRadioButtons.Enabled = true;
            else
                PublicationsRadioButtons.Enabled = false;

            if (File.Exists(Folder.Text + "\\" + StarColleagues.Text))
                StarColleaguesRadioButtons.Enabled = true;
            else
                StarColleaguesRadioButtons.Enabled = false;
        }

        /// <summary>
        /// Generate the reports
        /// </summary>
        private void GenerateReports_Click(object sender, EventArgs e)
        {
            // Make an anonymous callback function that keeps track of the callback data
            // and one that reports messages
            Reports.ReportStatus StatusCallback = delegate(int number, int total, Person person, bool StatusBarOnly)
            {
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = total;
                toolStripProgressBar1.Value = number;
                if (!StatusBarOnly)
                    AddLogEntry("Writing " + person.Last + " " + person.Setnb + " (" + number.ToString() + " of " + total.ToString() + ")");
                Application.DoEvents();
            };
            Reports.ReportMessage MessageCallback = delegate(string Message)
            {
                AddLogEntry(Message);
                Application.DoEvents();
            };

            
            if ((JournalWeights.Text == "") || (!File.Exists(JournalWeights.Text)))
            {
                MessageBox.Show("Please specify a valid Journal Weights file.", "Unable to Generate Reports", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            AddLogEntry("Generating reports");

            this.Enabled = false;

            Reports reports = new Reports(DB, JournalWeights.Text);
            reports.PeopleTable = "Colleagues";
            reports.PeoplePublicationsTable = "ColleaguePublications";

            int NumReports = 0;

            if (DoColleagues.Checked)
            {
                NumReports++;
                toolStripStatusLabel1.Text = "Colleagues";

                AddLogEntry("Creating Colleagues report: " + Colleagues.Text);

                // Set the people report sections based on the listbox
                string[] ReportSections = new string[peopleReportSections.Items.Count];
                for (int i = 0; i < peopleReportSections.Items.Count; i++)
                {
                    ReportSections[i] = peopleReportSections.Items[i].ToString();
                }
                reports.PeopleReportSections = ReportSections;

                PeopleOrPublicationsReport(WhichReport.People, ContinuePeople.Checked, reports);
            }


            if (DoPublications.Checked)
            {
                NumReports++;
                toolStripStatusLabel1.Text = "Publications";
                AddLogEntry("Creating Publications report: " + Publications.Text);
                PeopleOrPublicationsReport(WhichReport.Publications, ContinuePeople.Checked, reports);
            }

            if (DoStarColleagues.Checked)
            {
                try
                {
                    NumReports++;
                    AddLogEntry("Creating StarColleagues report: " + StarColleagues.Text);
                    CreateStarColleaguesReport();
                }
                catch (Exception ex)
                {
                    AddLogEntry("An error occurred while writing the Star Colleagues report: " + ex.Message);
                }
            }

            toolStripStatusLabel1.Text = "Done";
            toolStripProgressBar1.Minimum = 1;
            toolStripProgressBar1.Maximum = 1;
            toolStripProgressBar1.Value = 1;
            AddLogEntry(NumReports.ToString() + " reports written");

            // Refresh the radio buttons

            this.Enabled = true;

            CheckForExistingFiles();
        }


        /// <summary>
        /// Write the Star Colleagues report
        /// </summary>
        private void CreateStarColleaguesReport()
        {
            // Note: Errors are handled by calling function

            // If we're continuing the previous report, rename it to a backup filename,
            // then copy every person in it to the file to append -- except for the
            // last person, which we'll restart.
            string Filename = Folder.Text + "\\" + StarColleagues.Text;
            ArrayList SetnbsToSkip = new ArrayList();
            if (ContinueStarColleagues.Checked && File.Exists(Filename))
            {
                SetnbsToSkip = Reports.BackupReportAndGetSetnbs(Filename);
            }

            StreamWriter writer = new StreamWriter(Filename, ContinueStarColleagues.Checked);
            StarColleaguesReport.Generate(writer, DB, JournalWeights.Text, SetnbsToSkip, this, OverwriteStarColleagues.Checked);
            writer.Close();
        }

        /// <summary>
        /// When the journal weights file is changed, write it to the resgistry
        /// </summary>
        private void JournalWeights_TextChanged(object sender, EventArgs e)
        {
            Application.CommonAppDataRegistry.SetValue("JournalWeightsFile", JournalWeights.Text);
        }

        /// <summary>
        /// Display the Open File dialog for the Journal Weights file
        /// </summary>
        private void JournalWeightsFileDialog_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = JournalWeights.Text;
            openFileDialog1.Filter = "Microsoft Excel Files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog1.Title = "Select the Journal Weights file";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            JournalWeights.Text = openFileDialog1.FileName;
        }



        private enum WhichReport
        {
            People,
            Publications
        }

        /// <summary>
        /// Write the People or Publications report
        /// </summary>
        private void PeopleOrPublicationsReport(WhichReport whichReport, bool ContinuePreviousReport, Reports reports)
        {
            ArrayList SetnbsToSkip = null;

            string Filename = Folder.Text + "\\";
            if (whichReport == WhichReport.People)
                Filename += Colleagues.Text;
            else
                Filename += Publications.Text;

            // If we're continuing the previous report, rename it to a backup filename,
            // then copy every person in it to the file to append -- except for the
            // last person, which we'll restart.
            if (ContinuePreviousReport && File.Exists(Filename))
            {
                try
                {
                    SetnbsToSkip = Reports.BackupReportAndGetSetnbs(Filename);
                }
                catch (Exception ex)
                {
                    AddLogEntry("An error occurred while creating the report '" + Filename + "': " + ex.Message);
                    return;
                }
            }

            // Make an anonymous callback function that keeps track of the callback data
            Reports.ReportStatus StatusCallback = delegate(int number, int total, Person person, bool StatusBarOnly)
            {
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = total;
                toolStripProgressBar1.Value = number;
                if (!StatusBarOnly)
                    AddLogEntry( "Writing " + person.Last + " " + person.Setnb + " (" + number.ToString() + " of " + total.ToString() + ")");
                Application.DoEvents();
            };

            // Make an anonymous callback function to receive message
            Reports.ReportMessage MessageCallback = delegate(string Message)
            {
                AddLogEntry(Message);
                Application.DoEvents();
            };

            // Write the Stars report
            StreamWriter writer;
            try
            {
                // Open the writer to append the file only if continuing from a previous report
                writer = new StreamWriter(Filename, ContinuePreviousReport);
            } 
            catch (Exception ex)
            {
                AddLogEntry("An error occurred while creating the report '" + Filename + "': " + ex.Message);
                return;
            }
            try
            {
                if (whichReport == WhichReport.People) 
                    reports.PeopleReport(SetnbsToSkip, writer, StatusCallback, MessageCallback);
                else
                    reports.PubsReport(SetnbsToSkip, writer, StatusCallback, MessageCallback);
            }
            catch (Exception ex)
            {
                AddLogEntry("An error occurred while creating the report '" + Filename + "': " + ex.Message);
            }
            finally
            {
                writer.Close();
            }
        }


        /// <summary>
        /// Write a message to the log and to the status bar
        /// </summary>
        /// <param name="Message">Message to write</param>
        public void AddLogEntry(string Message)
        {
            toolStripStatusLabel2.Text = Message;
            MainForm.AddLogEntry(Message);
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
        /// When a section is clicked, make sure the "Remove" button is enabled
        /// </summary>
        private void peopleReportSections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (peopleReportSections.SelectedIndices.Count > 0)
                RemoveButton.Enabled = true;
            else 
                RemoveButton.Enabled = false;
        }


        /// <summary>
        /// Remove an item from the People Report Sections list
        /// </summary>
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            peopleReportSections.Items.RemoveAt(peopleReportSections.SelectedIndex);
        }


        /// <summary>
        /// Prompt for a section to add
        /// </summary>
        private void AddButton_Click(object sender, EventArgs e)
        {
            string Section = SectionToAdd.Text;
            if (Section == "")
            {
                MessageBox.Show("Specify the item to add in the textbox labeled 'Item to Add'.\n\nSections must be either a category number (1, 2, 3, etc.), a sum of categories (where \"1+2+3\" means\nadd publication types 1, 2 and 3), or \"all\" (which menas add all categories).", "Cannot add item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            else if (Section.ToLower() == "all")
            {
                if (peopleReportSections.Items.Contains("all"))
                
                    MessageBox.Show("The list already contains 'all'.", "Cannot add item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    peopleReportSections.Items.Add("all");
            }

            else if (Section.Contains("+"))
            {
                string[] Split = Section.Split('+');
                foreach (string OneSection in Split)
                {
                    if ((!Com.StellmanGreene.PubMed.Publications.IsNumeric(OneSection))
                        || (OneSection.Contains(".")))
                    {
                        MessageBox.Show("The section '" + Section + "' is invalid.\n\nSections must be either a category number (1, 2, 3, etc.), a sum of categories (where \"1+2+3\" means\nadd publication types 1, 2 and 3), or \"all\" (which menas add all categories).", "Cannot add item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                peopleReportSections.Items.Add(Section);

            }
            else
            {
                if ((!Com.StellmanGreene.PubMed.Publications.IsNumeric(Section))
                    || (Section.Contains(".")))
                {
                    MessageBox.Show("The section '" + Section + "' is invalid.\n\nSections must be either a category number (1, 2, 3, etc.), a sum of categories (where \"1+2+3\" means\nadd publication types 1, 2 and 3), or \"all\" (which menas add all categories).", "Cannot add item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                peopleReportSections.Items.Add(Section);
            }

            SectionToAdd.Text = "";
        }


    }
}