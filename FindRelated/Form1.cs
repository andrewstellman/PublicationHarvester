/*
 *                                FindRelated
 *              Copyright (c) 2003-2011 Stellman & Greene Consulting
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace Com.StellmanGreene.FindRelated
{
    public partial class Form1 : Form
    {
        private static readonly char[] INCLUDE_CATEGORIES_SEPARATORS = new char[] { ';', ',', ' ', '|' };

        /// <summary>
        /// Currently entered include categories
        /// </summary>
        private IEnumerable<int> includeCategoriesValues = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;

            string dsn = DSN.Text;

            FileInfo inputFileInfo = new FileInfo(inputFileTextBox.Text);
            if (!inputFileInfo.Exists)
            {
                MessageBox.Show("Please specify a valid input file");
                startButton.Enabled = true;
                return;
            }

            if (String.IsNullOrEmpty(dsn) || dsn.StartsWith("==") || dsn.EndsWith("DSNs"))
            {
                MessageBox.Show("Please select an ODBC data source from the dropdown");
                startButton.Enabled = true;
                return;
            }
            
            string relatedTableName = relatedTable.Text;
            if (String.IsNullOrEmpty(relatedTableName)) {
                MessageBox.Show("Please specify a related publications table to create/replace");
                startButton.Enabled = true;
                return;
            }

            DialogResult dialogResult = MessageBox.Show("The table '" + relatedTableName + "' will be deleted and recreated if it exists.",
                "Overwrite table?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            if (dialogResult == DialogResult.Cancel)
            {
                startButton.Enabled = true;
                return;
            }

            PublicationFilter publicationFilter = null;
            try
            {
                publicationFilter = new PublicationFilter(
                    sameJournal.Checked,
                    !enableUpperBound.Checked ? null : (int?)pubWindowUpperBound.Value,
                    !enableLowerBound.Checked ? null : (int?)pubWindowLowerBound.Value,
                    !enableMaximumLinkRanking.Checked ? null : (int?)maximumLinkRanking.Value,
                    includeCategoriesValues);

                Trace.WriteLine(DateTime.Now + " - " + publicationFilter);

                if (publicationFilter == null)
                    throw new NullReferenceException("Error creating publication filter");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception caught creating filters:");
                Trace.WriteLine(ex.Message);
                startButton.Enabled = true;
                return;
            }

            // Start the run
            backgroundWorker1.RunWorkerAsync(new Dictionary<string, object>() { 
                { "dsn", dsn }, 
                { "relatedTableName", relatedTableName }, 
                { "inputFileInfo", inputFileInfo },
                { "publicationFilter", publicationFilter },
            } );
            cancelButton.Enabled = true;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            logFilename.Text = Environment.GetEnvironmentVariable("TEMP") + @"\FindRelated_log.txt";
            TraceListener listBoxListener = new ListBoxTraceListener(log, toolStripStatusLabel1);
            Trace.Listeners.Add(new TextWriterTraceListener(logFilename.Text));
            Trace.Listeners.Add(listBoxListener);
            Trace.AutoFlush = true;

            // Add the version to the status bar
            Text += " v" + Application.ProductVersion;

            GetODBCDataSourceNames();

            Trace.WriteLine("----------------------------------------------------------");
            Trace.WriteLine(DateTime.Now + " - " + this.Text + " started");
        }



#region ODBC Data Source Dropdown
        /// <summary>
        /// Repopulate the DSN list when the user clicks on the DSN listbox
        /// </summary>
        private void DSN_Click(object sender, EventArgs e)
        {
            // Re-retrieve all of the DSNs
            GetODBCDataSourceNames();
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
#endregion

        /// <summary>
        /// Open the log in Notepad
        /// </summary>
        private void openInNotepad_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = "notepad.exe";
            proc.StartInfo.Arguments = logFilename.Text;
            proc.Start();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Trace.WriteLine(DateTime.Now + " - started run");
            Dictionary<string, object> args = e.Argument as Dictionary<string, object>;
            RelatedFinder relatedFinder = new RelatedFinder() { BackgroundWorker = backgroundWorker1 };
            relatedFinder.Go(args["dsn"] as string, 
                args["relatedTableName"] as string, 
                args["inputFileInfo"] as FileInfo,
                args["publicationFilter"] as PublicationFilter);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startButton.Enabled = true;
            cancelButton.Enabled = false;
            Trace.WriteLine(DateTime.Now + " - finished run");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Trace.WriteLine(DateTime.Now + " - form closed, cancelling...");
            backgroundWorker1.CancelAsync();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Trace.WriteLine(DateTime.Now + " - cancelling, please wait for the current operation to finish...");
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = 100;
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void inputFileDialog_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = inputFileTextBox.Text;
            openFileDialog.Filter = "Comma-delimited Text Files (*.csv)|*.csv|All files (*.*)|*.*";
           openFileDialog.Title = "Select the input file";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            inputFileTextBox.Text = openFileDialog.FileName;
        }

        private void enableUpperBound_CheckedChanged(object sender, EventArgs e)
        {
            pubWindowUpperBound.Enabled = enableUpperBound.Checked;

        }

        private void enableLowerBound_CheckedChanged(object sender, EventArgs e)
        {
            pubWindowLowerBound.Enabled = enableLowerBound.Checked;
        }

        private void enableMaximumLinkRanking_CheckedChanged(object sender, EventArgs e)
        {
            maximumLinkRanking.Enabled = enableMaximumLinkRanking.Checked;
        }

        private void includeCategories_TextChanged(object sender, EventArgs e)
        {
            // Build a new includeCategoriesValues based on the values in the text box
            includeCategoriesValues = new List<int>();

            includeCategories.SuspendLayout();
            
            int selectionStart = includeCategories.SelectionStart;
            int selectionLength = includeCategories.SelectionLength;

            bool invalidValueFound = false;
            string invalidValue = String.Empty;

            List<string> values = new List<string>(includeCategories.Text.Split(INCLUDE_CATEGORIES_SEPARATORS, StringSplitOptions.RemoveEmptyEntries));
            foreach (string value in values)
            {
                int i;
                if (int.TryParse(value, out i))
                {
                    ((List<int>)includeCategoriesValues).Add(i);
                }
                else
                {
                    invalidValueFound = true;
                    invalidValue = value;
                }
            }

            if (invalidValueFound)
            {
                MessageBox.Show("Invalid value in included categories: " + invalidValue);
                includeCategories.Text = includeCategories.Text.Remove(
                    includeCategories.Text.IndexOf(invalidValue),
                    invalidValue.Length);
                if (selectionStart > 0)
                    includeCategories.SelectionStart = selectionStart - 1;
                if (selectionLength > 0)
                    includeCategories.SelectionLength = selectionLength - 1;
            }

            includeCategories.ResumeLayout();
        }

    }
}
