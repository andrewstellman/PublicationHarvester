/*
 *                         Stars Colleague Generator
 *              Copyright (c) 2003-2006 Stellman & Greene Consulting
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
using Com.StellmanGreene.PubMed;
using System.Collections;

namespace PublicationHarvester
{

    public partial class ReportsDialog : Form
    {
        public Database DB;
        public Form1 MainForm;

        public ReportsDialog()
        {
            InitializeComponent();

            // Initialize the People Report Sections list
            string[] DefaultReportSections = Reports.DefaultPeopleReportSections();
            for (int i = 0; i < DefaultReportSections.Length; i++)
            {
                peopleReportSections.Items.Add(DefaultReportSections[i]);
            }

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
            if (File.Exists(Folder.Text + "\\" + People.Text))
                PeopleRadioButtons.Enabled = true;
            else
                PeopleRadioButtons.Enabled = false;

            if (File.Exists(Folder.Text + "\\" + Publications.Text))
                PublicationsRadioButtons.Enabled = true;
            else
                PublicationsRadioButtons.Enabled = false;
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
            StreamWriter writer = null;

            int NumReports = 0;

            if (DoPeople.Checked)
            {
                NumReports++;
                toolStripStatusLabel1.Text = "People";

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
                PeopleOrPublicationsReport(WhichReport.Publications, ContinuePeople.Checked, reports);
            }

            if (DoMeSHHeadings.Checked)
            {
                writer = null;
                try
                {
                    NumReports++;
                    toolStripStatusLabel1.Text = "MeSHHeadings";
                    AddLogEntry("Writing MeSH Headings report");
                    toolStripProgressBar1.Value = 0;
                    writer = new StreamWriter(Folder.Text + "\\" + MeSHHeadings.Text, false);
                    reports.MeSHHeadingReport(writer, StatusCallback, MessageCallback);
                }
                catch (Exception ex)
                {
                    AddLogEntry("An error occurred while writing the MeSH Headings report: " + ex.Message);
                }
                finally
                {
                    if (writer != null)
                        writer.Close();
                }
            }

            if (DoGrantIDs.Checked) 
            {
                writer = null;
                try
                {
                    NumReports++;
                    toolStripStatusLabel1.Text = "Grants";
                    AddLogEntry("Writing Grants report");
                    toolStripProgressBar1.Value = 0;
                    writer = new StreamWriter(Folder.Text + "\\" + GrantIDs.Text, false);
                    reports.GrantsReport(writer);
                }
                catch (Exception ex)
                {
                    AddLogEntry("An error occurred while writing the Grants report: " + ex.Message);
                }
                finally
                {
                    if (writer != null)
                        writer.Close();
                }
            }

            toolStripStatusLabel1.Text = "Done";
            AddLogEntry(NumReports.ToString() + " reports written");

            // Refresh the radio buttons

            this.Enabled = true;

            CheckForExistingFiles();
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
                Filename += People.Text;
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
        private void AddLogEntry(string Message)
        {
            toolStripStatusLabel2.Text = Message;
            MainForm.AddLogEntry(Message);
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