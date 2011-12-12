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
using Com.StellmanGreene.SocialNetworking;

namespace Com.StellmanGreene.SocialNetworking
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            statusStrip1.Items[0].Text = "v" + Application.ProductVersion;
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
        /// When the DSN changes, load the databases
        /// </summary>
        private void DSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Open the DSN and read the databases
            Database DB = new Database(DSN.Text);
            DataTable DatabaseList = DB.ExecuteQuery("show databases;");
            FirstDegreeDB.Items.Clear();
            SecondDegreeDB.Items.Clear();
            foreach (DataRow Row in DatabaseList.Rows)
            {
                string DBName = Row[0].ToString();
                FirstDegreeDB.Items.Add(DBName);
                SecondDegreeDB.Items.Add(DBName);
            }
            FirstDegreeDB.Enabled = true;
            SecondDegreeDB.Enabled = true;
        }


        /// <summary>
        /// Enable or disable the button based on selections
        /// </summary>
        private void EnableButton()
        {
            if (FirstDegreeDB.Text != "" && SecondDegreeDB.Text != "" && ReportPath.Text != "")
                WriteReport.Enabled = true;
            else
                WriteReport.Enabled = false;
        }

        private void FirstDegreeDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableButton();
        }

        private void SecondDegreeDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableButton();
        }

        private void ReportPath_TextChanged(object sender, EventArgs e)
        {
            EnableButton();
        }

        private void SecondDegreeDB_TextChanged(object sender, EventArgs e)
        {
            EnableButton();
        }

        private void FirstDegreeDB_TextChanged(object sender, EventArgs e)
        {
            EnableButton();
        }

        private void ReportPathDialog_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = ReportPath.Text;
            saveFileDialog1.Filter = "Comma-delimited Text Files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog1.Title = "Select the report file to write";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.OverwritePrompt = true;
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            ReportPath.Text = saveFileDialog1.FileName;
        }

        /// <summary>
        /// Write the report to the specified file
        /// </summary>
        private void WriteReport_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;

            // Read the second-degree stars to exclude
            List<string> ColleaguesToInclude = null;
            if (IncludeFilePath.Text != "")
                if (!File.Exists(IncludeFilePath.Text))
                    MessageBox.Show("The list of second-degree stars to include does not exist.", "File not found");
                else
                    ColleaguesToInclude = new List<string>(File.ReadAllLines(IncludeFilePath.Text));

            try
            {
                // Set up an anonymous callback function to receive status
                Report.Status StatusCallback = delegate(int Number, int Total, string Setnb, string Name)
                {
                    statusStrip1.Items[1].Text = Name + " (" + Setnb + ")";
                    statusStrip1.Items[2].Text = Number.ToString() + " of " + Total.ToString();
                    Application.DoEvents();
                };

                // Set up an anonymous callback to receive detailed progress
                Report.DetailedProgress DetailedProgressCallback = delegate(int FirstDegree, int TotalFirstDegree,
                int SecondDegree, int TotalSecondDegree) {
                    ToolStripProgressBar progress = statusStrip2.Items[0] as ToolStripProgressBar;
                    progress.Minimum = 1;
                    progress.Maximum = TotalFirstDegree * 10;
                    // Use the second-degree stars to give smooth progress bar increments
                    progress.Value = Math.Min(TotalFirstDegree * 10, 
                        FirstDegree * 10 + (int) (10 * ((float)SecondDegree / (float)TotalSecondDegree)));
                    statusStrip2.Items[1].Text = FirstDegree + " of " + TotalFirstDegree + " 1st degree colleagues";
                    statusStrip2.Items[2].Text = SecondDegree + " of " + TotalSecondDegree + " 2nd degree colleagues";
                    Application.DoEvents();
                };


                // Check if the selected file exists. If it does, the user can either choose to continue processing the
                // report or overwrite it and restart processing.
                bool ContinueReport = false;
                if (File.Exists(ReportPath.Text))
                {
                    DialogResult Result = MessageBox.Show("The report file exists. Continue from where the previous report left off?\n(Click 'No' to overwrite the report and restart it from the beginning.)",
                        "Continue existing report file?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (Result)
                    {
                        case DialogResult.Cancel:
                            this.Enabled = true;
                            this.Cursor = Cursors.Default;
                            return;

                        case DialogResult.No:
                            File.Delete(ReportPath.Text);
                            ContinueReport = false;
                            break;

                        case DialogResult.Yes:
                            File.Delete(ReportPath.Text + ".bak");
                            File.Move(ReportPath.Text, ReportPath.Text + ".bak");
                            ContinueReport = true;
                            break;
                    }
                    
                }

                // Open the file to write the report to
                using (StreamWriter Writer = File.CreateText(ReportPath.Text))
                {
                    bool HeaderWritten = false;

                    // If continuing the previous report, copy the rows and create the list of stars to exclude
                    List<string> ColleaguesToExclude = new List<string>();
                    if (ContinueReport)
                    {
                        string[] OldReport = File.ReadAllLines(ReportPath.Text + ".bak");

                        // Copy the header
                        Writer.WriteLine(OldReport[0]);
                        HeaderWritten = true;

                        // Find the next block of rows. Write them to the file -- as long as they're not the
                        // last block.
                        int CopiedLines = 1;
                        int CurrentLine = 1;
                        string Setnb = OldReport[CopiedLines].Substring(0, OldReport[CopiedLines].IndexOf(','));

                        // Loop through each block of setnbs until we run out of lines
                        while (CurrentLine < OldReport.Length - 1)
                        {

                            while (CurrentLine < OldReport.Length - 1 && OldReport[CurrentLine].StartsWith(Setnb + ","))
                                CurrentLine++;

                            if (CurrentLine < OldReport.Length - 1)
                            {
                                Setnb = OldReport[CopiedLines].Substring(0, OldReport[CopiedLines].IndexOf(','));
                                ColleaguesToExclude.Add(Setnb);
                                while (CopiedLines < CurrentLine)
                                {
                                    Writer.WriteLine(OldReport[CopiedLines]);
                                    CopiedLines++;
                                }
                            }

                        }

                    }

                    // Set up an anonymous callback function to write the rows to the database
                    Report.WriteRows WriteRowsCallback = delegate(DataTable RowsToWrite)
                    {
                        if (!HeaderWritten)
                        {
                            Report.WriteHeader(RowsToWrite, Writer);
                            HeaderWritten = true;
                        }
                        Report.WriteCSV(RowsToWrite, Writer);
                    };

                    Report report = new Report(new Database(DSN.Text), FirstDegreeDB.Text, SecondDegreeDB.Text, ColleaguesToInclude, null);
                    report.Generate(StatusCallback, WriteRowsCallback, DetailedProgressCallback);
                    MessageBox.Show("Wrote the report file '" + ReportPath.Text + "'", "Social networking report written");
                    statusStrip1.Items[1].Text = "Finished";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occurred while writing the report");
            }
            this.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void IncludeFilePathDialog_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = IncludeFilePath.Text;
            openFileDialog1.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.Title = "Select the text file with second degree stars to include";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = true;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.Cancel)
                IncludeFilePath.Text = "";
            else
                IncludeFilePath.Text = openFileDialog1.FileName;
        }
    }
}