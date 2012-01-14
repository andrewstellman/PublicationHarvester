namespace PublicationHarvester
{
    partial class ReportsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DoPeople = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DoPublications = new System.Windows.Forms.CheckBox();
            this.DoMeSHHeadings = new System.Windows.Forms.CheckBox();
            this.DoGrantIDs = new System.Windows.Forms.CheckBox();
            this.MeSHHeadings = new System.Windows.Forms.TextBox();
            this.GrantIDs = new System.Windows.Forms.TextBox();
            this.Publications = new System.Windows.Forms.TextBox();
            this.People = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.Folder = new System.Windows.Forms.TextBox();
            this.SpecifyFolder = new System.Windows.Forms.Button();
            this.PublicationsRadioButtons = new System.Windows.Forms.Panel();
            this.ContinuePublications = new System.Windows.Forms.RadioButton();
            this.OverwritePublications = new System.Windows.Forms.RadioButton();
            this.PeopleRadioButtons = new System.Windows.Forms.Panel();
            this.ContinuePeople = new System.Windows.Forms.RadioButton();
            this.OverwritePeople = new System.Windows.Forms.RadioButton();
            this.GenerateReports = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.JournalWeightsFileDialog = new System.Windows.Forms.Button();
            this.JournalWeights = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.peopleReportSections = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SectionToAdd = new System.Windows.Forms.TextBox();
            this.PublicationsRadioButtons.SuspendLayout();
            this.PeopleRadioButtons.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DoPeople
            // 
            this.DoPeople.AutoSize = true;
            this.DoPeople.Checked = true;
            this.DoPeople.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DoPeople.Location = new System.Drawing.Point(15, 29);
            this.DoPeople.Name = "DoPeople";
            this.DoPeople.Size = new System.Drawing.Size(74, 21);
            this.DoPeople.TabIndex = 10;
            this.DoPeople.Text = "&People";
            this.DoPeople.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select Reports to Write";
            // 
            // DoPublications
            // 
            this.DoPublications.AutoSize = true;
            this.DoPublications.Checked = true;
            this.DoPublications.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DoPublications.Location = new System.Drawing.Point(15, 96);
            this.DoPublications.Name = "DoPublications";
            this.DoPublications.Size = new System.Drawing.Size(106, 21);
            this.DoPublications.TabIndex = 30;
            this.DoPublications.Text = "P&ublications";
            this.DoPublications.UseVisualStyleBackColor = true;
            // 
            // DoMeSHHeadings
            // 
            this.DoMeSHHeadings.AutoSize = true;
            this.DoMeSHHeadings.Checked = true;
            this.DoMeSHHeadings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DoMeSHHeadings.Location = new System.Drawing.Point(15, 161);
            this.DoMeSHHeadings.Name = "DoMeSHHeadings";
            this.DoMeSHHeadings.Size = new System.Drawing.Size(132, 21);
            this.DoMeSHHeadings.TabIndex = 50;
            this.DoMeSHHeadings.Text = "&MeSH Headings";
            this.DoMeSHHeadings.UseVisualStyleBackColor = true;
            // 
            // DoGrantIDs
            // 
            this.DoGrantIDs.AutoSize = true;
            this.DoGrantIDs.Checked = true;
            this.DoGrantIDs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DoGrantIDs.Location = new System.Drawing.Point(15, 188);
            this.DoGrantIDs.Name = "DoGrantIDs";
            this.DoGrantIDs.Size = new System.Drawing.Size(90, 21);
            this.DoGrantIDs.TabIndex = 60;
            this.DoGrantIDs.Text = "Grant &IDs";
            this.DoGrantIDs.UseVisualStyleBackColor = true;
            // 
            // MeSHHeadings
            // 
            this.MeSHHeadings.Location = new System.Drawing.Point(153, 159);
            this.MeSHHeadings.Name = "MeSHHeadings";
            this.MeSHHeadings.Size = new System.Drawing.Size(393, 22);
            this.MeSHHeadings.TabIndex = 55;
            this.MeSHHeadings.Text = "meshheadings.csv";
            // 
            // GrantIDs
            // 
            this.GrantIDs.Location = new System.Drawing.Point(153, 186);
            this.GrantIDs.Name = "GrantIDs";
            this.GrantIDs.Size = new System.Drawing.Size(393, 22);
            this.GrantIDs.TabIndex = 65;
            this.GrantIDs.Text = "grantids.csv";
            // 
            // Publications
            // 
            this.Publications.Location = new System.Drawing.Point(153, 94);
            this.Publications.Name = "Publications";
            this.Publications.Size = new System.Drawing.Size(393, 22);
            this.Publications.TabIndex = 35;
            this.Publications.Text = "publications.csv";
            // 
            // People
            // 
            this.People.Location = new System.Drawing.Point(153, 27);
            this.People.Name = "People";
            this.People.Size = new System.Drawing.Size(393, 22);
            this.People.TabIndex = 15;
            this.People.Text = "people.csv";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 17);
            this.label2.TabIndex = 70;
            this.label2.Text = "Specify a &Folder to Write To";
            // 
            // Folder
            // 
            this.Folder.Location = new System.Drawing.Point(15, 254);
            this.Folder.Multiline = true;
            this.Folder.Name = "Folder";
            this.Folder.ReadOnly = true;
            this.Folder.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Folder.Size = new System.Drawing.Size(682, 41);
            this.Folder.TabIndex = 80;
            this.Folder.DoubleClick += new System.EventHandler(this.SpecifyFolder_Click);
            this.Folder.TextChanged += new System.EventHandler(this.Folder_TextChanged);
            // 
            // SpecifyFolder
            // 
            this.SpecifyFolder.AutoEllipsis = true;
            this.SpecifyFolder.Location = new System.Drawing.Point(703, 273);
            this.SpecifyFolder.Name = "SpecifyFolder";
            this.SpecifyFolder.Size = new System.Drawing.Size(21, 22);
            this.SpecifyFolder.TabIndex = 75;
            this.SpecifyFolder.Text = "...";
            this.SpecifyFolder.UseVisualStyleBackColor = true;
            this.SpecifyFolder.Click += new System.EventHandler(this.SpecifyFolder_Click);
            // 
            // PublicationsRadioButtons
            // 
            this.PublicationsRadioButtons.Controls.Add(this.ContinuePublications);
            this.PublicationsRadioButtons.Controls.Add(this.OverwritePublications);
            this.PublicationsRadioButtons.Enabled = false;
            this.PublicationsRadioButtons.Location = new System.Drawing.Point(15, 118);
            this.PublicationsRadioButtons.Name = "PublicationsRadioButtons";
            this.PublicationsRadioButtons.Size = new System.Drawing.Size(531, 33);
            this.PublicationsRadioButtons.TabIndex = 16;
            // 
            // ContinuePublications
            // 
            this.ContinuePublications.AutoSize = true;
            this.ContinuePublications.Location = new System.Drawing.Point(294, 3);
            this.ContinuePublications.Name = "ContinuePublications";
            this.ContinuePublications.Size = new System.Drawing.Size(204, 21);
            this.ContinuePublications.TabIndex = 45;
            this.ContinuePublications.Text = "Co&ntinue where report ends";
            this.ContinuePublications.UseVisualStyleBackColor = true;
            // 
            // OverwritePublications
            // 
            this.OverwritePublications.AutoSize = true;
            this.OverwritePublications.Checked = true;
            this.OverwritePublications.Location = new System.Drawing.Point(32, 3);
            this.OverwritePublications.Name = "OverwritePublications";
            this.OverwritePublications.Size = new System.Drawing.Size(256, 21);
            this.OverwritePublications.TabIndex = 40;
            this.OverwritePublications.TabStop = true;
            this.OverwritePublications.Text = "O&vewrite existing publications report";
            this.OverwritePublications.UseVisualStyleBackColor = true;
            // 
            // PeopleRadioButtons
            // 
            this.PeopleRadioButtons.Controls.Add(this.ContinuePeople);
            this.PeopleRadioButtons.Controls.Add(this.OverwritePeople);
            this.PeopleRadioButtons.Enabled = false;
            this.PeopleRadioButtons.Location = new System.Drawing.Point(15, 51);
            this.PeopleRadioButtons.Name = "PeopleRadioButtons";
            this.PeopleRadioButtons.Size = new System.Drawing.Size(531, 33);
            this.PeopleRadioButtons.TabIndex = 17;
            // 
            // ContinuePeople
            // 
            this.ContinuePeople.AutoSize = true;
            this.ContinuePeople.Location = new System.Drawing.Point(294, 3);
            this.ContinuePeople.Name = "ContinuePeople";
            this.ContinuePeople.Size = new System.Drawing.Size(204, 21);
            this.ContinuePeople.TabIndex = 25;
            this.ContinuePeople.Text = "&Continue where report ends";
            this.ContinuePeople.UseVisualStyleBackColor = true;
            // 
            // OverwritePeople
            // 
            this.OverwritePeople.AutoSize = true;
            this.OverwritePeople.Checked = true;
            this.OverwritePeople.Location = new System.Drawing.Point(32, 3);
            this.OverwritePeople.Name = "OverwritePeople";
            this.OverwritePeople.Size = new System.Drawing.Size(229, 21);
            this.OverwritePeople.TabIndex = 20;
            this.OverwritePeople.TabStop = true;
            this.OverwritePeople.Text = "&Overwrite existing people report";
            this.OverwritePeople.UseVisualStyleBackColor = true;
            // 
            // GenerateReports
            // 
            this.GenerateReports.Location = new System.Drawing.Point(244, 355);
            this.GenerateReports.Name = "GenerateReports";
            this.GenerateReports.Size = new System.Drawing.Size(248, 28);
            this.GenerateReports.TabIndex = 1;
            this.GenerateReports.Text = "&Generate Reports";
            this.GenerateReports.UseVisualStyleBackColor = true;
            this.GenerateReports.Click += new System.EventHandler(this.GenerateReports_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 392);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(736, 23);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(150, 18);
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(469, 18);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // JournalWeightsFileDialog
            // 
            this.JournalWeightsFileDialog.AutoEllipsis = true;
            this.JournalWeightsFileDialog.Location = new System.Drawing.Point(703, 328);
            this.JournalWeightsFileDialog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.JournalWeightsFileDialog.Name = "JournalWeightsFileDialog";
            this.JournalWeightsFileDialog.Size = new System.Drawing.Size(21, 23);
            this.JournalWeightsFileDialog.TabIndex = 95;
            this.JournalWeightsFileDialog.Text = "...";
            this.JournalWeightsFileDialog.Click += new System.EventHandler(this.JournalWeightsFileDialog_Click);
            // 
            // JournalWeights
            // 
            this.JournalWeights.Location = new System.Drawing.Point(15, 328);
            this.JournalWeights.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.JournalWeights.Name = "JournalWeights";
            this.JournalWeights.Size = new System.Drawing.Size(682, 22);
            this.JournalWeights.TabIndex = 90;
            this.JournalWeights.TextChanged += new System.EventHandler(this.JournalWeights_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 310);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(216, 23);
            this.label3.TabIndex = 85;
            this.label3.Text = "&Journal Weights File";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // peopleReportSections
            // 
            this.peopleReportSections.FormattingEnabled = true;
            this.peopleReportSections.ItemHeight = 16;
            this.peopleReportSections.Location = new System.Drawing.Point(570, 29);
            this.peopleReportSections.Name = "peopleReportSections";
            this.peopleReportSections.Size = new System.Drawing.Size(154, 148);
            this.peopleReportSections.TabIndex = 96;
            this.peopleReportSections.SelectedIndexChanged += new System.EventHandler(this.peopleReportSections_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(567, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 17);
            this.label4.TabIndex = 97;
            this.label4.Text = "People Report Sections";
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(570, 209);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 98;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Enabled = false;
            this.RemoveButton.Location = new System.Drawing.Point(651, 209);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveButton.TabIndex = 99;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(567, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 17);
            this.label5.TabIndex = 100;
            this.label5.Text = "Section to add:";
            // 
            // SectionToAdd
            // 
            this.SectionToAdd.Location = new System.Drawing.Point(669, 180);
            this.SectionToAdd.Name = "SectionToAdd";
            this.SectionToAdd.Size = new System.Drawing.Size(55, 22);
            this.SectionToAdd.TabIndex = 101;
            // 
            // ReportsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 415);
            this.Controls.Add(this.SectionToAdd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.peopleReportSections);
            this.Controls.Add(this.JournalWeightsFileDialog);
            this.Controls.Add(this.JournalWeights);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.GenerateReports);
            this.Controls.Add(this.PeopleRadioButtons);
            this.Controls.Add(this.PublicationsRadioButtons);
            this.Controls.Add(this.SpecifyFolder);
            this.Controls.Add(this.Folder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.People);
            this.Controls.Add(this.Publications);
            this.Controls.Add(this.GrantIDs);
            this.Controls.Add(this.MeSHHeadings);
            this.Controls.Add(this.DoGrantIDs);
            this.Controls.Add(this.DoMeSHHeadings);
            this.Controls.Add(this.DoPublications);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DoPeople);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate Harvesting Reports";
            this.Load += new System.EventHandler(this.ReportsDialog_Load);
            this.PublicationsRadioButtons.ResumeLayout(false);
            this.PublicationsRadioButtons.PerformLayout();
            this.PeopleRadioButtons.ResumeLayout(false);
            this.PeopleRadioButtons.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox DoPeople;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox DoPublications;
        private System.Windows.Forms.CheckBox DoMeSHHeadings;
        private System.Windows.Forms.CheckBox DoGrantIDs;
        private System.Windows.Forms.TextBox MeSHHeadings;
        private System.Windows.Forms.TextBox GrantIDs;
        private System.Windows.Forms.TextBox Publications;
        private System.Windows.Forms.TextBox People;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox Folder;
        private System.Windows.Forms.Button SpecifyFolder;
        private System.Windows.Forms.Panel PublicationsRadioButtons;
        private System.Windows.Forms.RadioButton ContinuePublications;
        private System.Windows.Forms.RadioButton OverwritePublications;
        private System.Windows.Forms.Panel PeopleRadioButtons;
        private System.Windows.Forms.RadioButton ContinuePeople;
        private System.Windows.Forms.RadioButton OverwritePeople;
        private System.Windows.Forms.Button GenerateReports;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        internal System.Windows.Forms.Button JournalWeightsFileDialog;
        private System.Windows.Forms.TextBox JournalWeights;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListBox peopleReportSections;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SectionToAdd;
    }
}