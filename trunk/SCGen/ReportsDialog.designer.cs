namespace SCGen
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
            this.DoColleagues = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DoPublications = new System.Windows.Forms.CheckBox();
            this.DoStarColleagues = new System.Windows.Forms.CheckBox();
            this.StarColleagues = new System.Windows.Forms.TextBox();
            this.Publications = new System.Windows.Forms.TextBox();
            this.Colleagues = new System.Windows.Forms.TextBox();
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
            this.StarColleaguesRadioButtons = new System.Windows.Forms.Panel();
            this.ContinueStarColleagues = new System.Windows.Forms.RadioButton();
            this.OverwriteStarColleagues = new System.Windows.Forms.RadioButton();
            this.StarColleaguePositions = new System.Windows.Forms.TextBox();
            this.DoStarColleaguePositions = new System.Windows.Forms.CheckBox();
            this.alternateTableName = new System.Windows.Forms.TextBox();
            this.useAlternateCheckbox = new System.Windows.Forms.CheckBox();
            this.PublicationsRadioButtons.SuspendLayout();
            this.PeopleRadioButtons.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.StarColleaguesRadioButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // DoColleagues
            // 
            this.DoColleagues.AutoSize = true;
            this.DoColleagues.Checked = true;
            this.DoColleagues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DoColleagues.Location = new System.Drawing.Point(11, 24);
            this.DoColleagues.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DoColleagues.Name = "DoColleagues";
            this.DoColleagues.Size = new System.Drawing.Size(78, 17);
            this.DoColleagues.TabIndex = 10;
            this.DoColleagues.Text = "&Colleagues";
            this.DoColleagues.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select Reports to Write";
            // 
            // DoPublications
            // 
            this.DoPublications.AutoSize = true;
            this.DoPublications.Checked = true;
            this.DoPublications.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DoPublications.Location = new System.Drawing.Point(11, 78);
            this.DoPublications.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DoPublications.Name = "DoPublications";
            this.DoPublications.Size = new System.Drawing.Size(83, 17);
            this.DoPublications.TabIndex = 30;
            this.DoPublications.Text = "P&ublications";
            this.DoPublications.UseVisualStyleBackColor = true;
            // 
            // DoStarColleagues
            // 
            this.DoStarColleagues.AutoSize = true;
            this.DoStarColleagues.Checked = true;
            this.DoStarColleagues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DoStarColleagues.Location = new System.Drawing.Point(11, 131);
            this.DoStarColleagues.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DoStarColleagues.Name = "DoStarColleagues";
            this.DoStarColleagues.Size = new System.Drawing.Size(100, 17);
            this.DoStarColleagues.TabIndex = 50;
            this.DoStarColleagues.Text = "&Star Colleagues";
            this.DoStarColleagues.UseVisualStyleBackColor = true;
            // 
            // StarColleagues
            // 
            this.StarColleagues.Location = new System.Drawing.Point(115, 129);
            this.StarColleagues.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.StarColleagues.Name = "StarColleagues";
            this.StarColleagues.Size = new System.Drawing.Size(296, 20);
            this.StarColleagues.TabIndex = 55;
            this.StarColleagues.Text = "starcolleagues.csv";
            // 
            // Publications
            // 
            this.Publications.Location = new System.Drawing.Point(115, 76);
            this.Publications.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Publications.Name = "Publications";
            this.Publications.Size = new System.Drawing.Size(296, 20);
            this.Publications.TabIndex = 35;
            this.Publications.Text = "publications.csv";
            // 
            // Colleagues
            // 
            this.Colleagues.Location = new System.Drawing.Point(115, 22);
            this.Colleagues.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Colleagues.Name = "Colleagues";
            this.Colleagues.Size = new System.Drawing.Size(296, 20);
            this.Colleagues.TabIndex = 15;
            this.Colleagues.Text = "colleagues.csv";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 270);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 70;
            this.label2.Text = "Specify a &Folder to Write To";
            // 
            // Folder
            // 
            this.Folder.Location = new System.Drawing.Point(10, 286);
            this.Folder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Folder.Multiline = true;
            this.Folder.Name = "Folder";
            this.Folder.ReadOnly = true;
            this.Folder.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Folder.Size = new System.Drawing.Size(512, 34);
            this.Folder.TabIndex = 80;
            this.Folder.TextChanged += new System.EventHandler(this.Folder_TextChanged);
            this.Folder.DoubleClick += new System.EventHandler(this.SpecifyFolder_Click);
            // 
            // SpecifyFolder
            // 
            this.SpecifyFolder.AutoEllipsis = true;
            this.SpecifyFolder.Location = new System.Drawing.Point(526, 302);
            this.SpecifyFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpecifyFolder.Name = "SpecifyFolder";
            this.SpecifyFolder.Size = new System.Drawing.Size(16, 18);
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
            this.PublicationsRadioButtons.Location = new System.Drawing.Point(11, 98);
            this.PublicationsRadioButtons.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PublicationsRadioButtons.Name = "PublicationsRadioButtons";
            this.PublicationsRadioButtons.Size = new System.Drawing.Size(398, 27);
            this.PublicationsRadioButtons.TabIndex = 16;
            // 
            // ContinuePublications
            // 
            this.ContinuePublications.AutoSize = true;
            this.ContinuePublications.Location = new System.Drawing.Point(243, 2);
            this.ContinuePublications.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ContinuePublications.Name = "ContinuePublications";
            this.ContinuePublications.Size = new System.Drawing.Size(155, 17);
            this.ContinuePublications.TabIndex = 45;
            this.ContinuePublications.Text = "Co&ntinue where report ends";
            this.ContinuePublications.UseVisualStyleBackColor = true;
            // 
            // OverwritePublications
            // 
            this.OverwritePublications.AutoSize = true;
            this.OverwritePublications.Checked = true;
            this.OverwritePublications.Location = new System.Drawing.Point(24, 2);
            this.OverwritePublications.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OverwritePublications.Name = "OverwritePublications";
            this.OverwritePublications.Size = new System.Drawing.Size(194, 17);
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
            this.PeopleRadioButtons.Location = new System.Drawing.Point(11, 45);
            this.PeopleRadioButtons.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PeopleRadioButtons.Name = "PeopleRadioButtons";
            this.PeopleRadioButtons.Size = new System.Drawing.Size(398, 27);
            this.PeopleRadioButtons.TabIndex = 17;
            // 
            // ContinuePeople
            // 
            this.ContinuePeople.AutoSize = true;
            this.ContinuePeople.Location = new System.Drawing.Point(243, 2);
            this.ContinuePeople.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ContinuePeople.Name = "ContinuePeople";
            this.ContinuePeople.Size = new System.Drawing.Size(155, 17);
            this.ContinuePeople.TabIndex = 25;
            this.ContinuePeople.Text = "&Continue where report ends";
            this.ContinuePeople.UseVisualStyleBackColor = true;
            // 
            // OverwritePeople
            // 
            this.OverwritePeople.AutoSize = true;
            this.OverwritePeople.Checked = true;
            this.OverwritePeople.Location = new System.Drawing.Point(24, 2);
            this.OverwritePeople.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OverwritePeople.Name = "OverwritePeople";
            this.OverwritePeople.Size = new System.Drawing.Size(173, 17);
            this.OverwritePeople.TabIndex = 20;
            this.OverwritePeople.TabStop = true;
            this.OverwritePeople.Text = "&Overwrite existing people report";
            this.OverwritePeople.UseVisualStyleBackColor = true;
            // 
            // GenerateReports
            // 
            this.GenerateReports.Location = new System.Drawing.Point(182, 368);
            this.GenerateReports.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GenerateReports.Name = "GenerateReports";
            this.GenerateReports.Size = new System.Drawing.Size(186, 23);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 395);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(552, 23);
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
            this.toolStripProgressBar1.Size = new System.Drawing.Size(75, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(314, 18);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // JournalWeightsFileDialog
            // 
            this.JournalWeightsFileDialog.AutoEllipsis = true;
            this.JournalWeightsFileDialog.Location = new System.Drawing.Point(526, 346);
            this.JournalWeightsFileDialog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.JournalWeightsFileDialog.Name = "JournalWeightsFileDialog";
            this.JournalWeightsFileDialog.Size = new System.Drawing.Size(16, 19);
            this.JournalWeightsFileDialog.TabIndex = 95;
            this.JournalWeightsFileDialog.Text = "...";
            this.JournalWeightsFileDialog.Click += new System.EventHandler(this.JournalWeightsFileDialog_Click);
            // 
            // JournalWeights
            // 
            this.JournalWeights.Location = new System.Drawing.Point(10, 346);
            this.JournalWeights.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.JournalWeights.Name = "JournalWeights";
            this.JournalWeights.Size = new System.Drawing.Size(512, 20);
            this.JournalWeights.TabIndex = 90;
            this.JournalWeights.TextChanged += new System.EventHandler(this.JournalWeights_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 332);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 19);
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
            this.peopleReportSections.Location = new System.Drawing.Point(416, 24);
            this.peopleReportSections.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.peopleReportSections.Name = "peopleReportSections";
            this.peopleReportSections.Size = new System.Drawing.Size(116, 121);
            this.peopleReportSections.TabIndex = 96;
            this.peopleReportSections.SelectedIndexChanged += new System.EventHandler(this.peopleReportSections_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(413, 7);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 97;
            this.label4.Text = "Colleagues Report Sections";
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(416, 167);
            this.AddButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(56, 19);
            this.AddButton.TabIndex = 98;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Enabled = false;
            this.RemoveButton.Location = new System.Drawing.Point(476, 167);
            this.RemoveButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(56, 19);
            this.RemoveButton.TabIndex = 99;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(413, 146);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 100;
            this.label5.Text = "Section to add:";
            // 
            // SectionToAdd
            // 
            this.SectionToAdd.Location = new System.Drawing.Point(490, 144);
            this.SectionToAdd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SectionToAdd.Name = "SectionToAdd";
            this.SectionToAdd.Size = new System.Drawing.Size(42, 20);
            this.SectionToAdd.TabIndex = 101;
            // 
            // StarColleaguesRadioButtons
            // 
            this.StarColleaguesRadioButtons.Controls.Add(this.ContinueStarColleagues);
            this.StarColleaguesRadioButtons.Controls.Add(this.OverwriteStarColleagues);
            this.StarColleaguesRadioButtons.Enabled = false;
            this.StarColleaguesRadioButtons.Location = new System.Drawing.Point(11, 152);
            this.StarColleaguesRadioButtons.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.StarColleaguesRadioButtons.Name = "StarColleaguesRadioButtons";
            this.StarColleaguesRadioButtons.Size = new System.Drawing.Size(398, 27);
            this.StarColleaguesRadioButtons.TabIndex = 102;
            // 
            // ContinueStarColleagues
            // 
            this.ContinueStarColleagues.AutoSize = true;
            this.ContinueStarColleagues.Location = new System.Drawing.Point(243, 2);
            this.ContinueStarColleagues.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ContinueStarColleagues.Name = "ContinueStarColleagues";
            this.ContinueStarColleagues.Size = new System.Drawing.Size(155, 17);
            this.ContinueStarColleagues.TabIndex = 45;
            this.ContinueStarColleagues.Text = "Co&ntinue where report ends";
            this.ContinueStarColleagues.UseVisualStyleBackColor = true;
            // 
            // OverwriteStarColleagues
            // 
            this.OverwriteStarColleagues.AutoSize = true;
            this.OverwriteStarColleagues.Checked = true;
            this.OverwriteStarColleagues.Location = new System.Drawing.Point(24, 2);
            this.OverwriteStarColleagues.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OverwriteStarColleagues.Name = "OverwriteStarColleagues";
            this.OverwriteStarColleagues.Size = new System.Drawing.Size(209, 17);
            this.OverwriteStarColleagues.TabIndex = 40;
            this.OverwriteStarColleagues.TabStop = true;
            this.OverwriteStarColleagues.Text = "O&vewrite existing star colleagues report";
            this.OverwriteStarColleagues.UseVisualStyleBackColor = true;
            // 
            // StarColleaguePositions
            // 
            this.StarColleaguePositions.Location = new System.Drawing.Point(154, 197);
            this.StarColleaguePositions.Margin = new System.Windows.Forms.Padding(2);
            this.StarColleaguePositions.Name = "StarColleaguePositions";
            this.StarColleaguePositions.Size = new System.Drawing.Size(368, 20);
            this.StarColleaguePositions.TabIndex = 104;
            this.StarColleaguePositions.Text = "starcolleaguepositions.csv";
            // 
            // DoStarColleaguePositions
            // 
            this.DoStarColleaguePositions.AutoSize = true;
            this.DoStarColleaguePositions.Checked = true;
            this.DoStarColleaguePositions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DoStarColleaguePositions.Location = new System.Drawing.Point(10, 197);
            this.DoStarColleaguePositions.Margin = new System.Windows.Forms.Padding(2);
            this.DoStarColleaguePositions.Name = "DoStarColleaguePositions";
            this.DoStarColleaguePositions.Size = new System.Drawing.Size(140, 17);
            this.DoStarColleaguePositions.TabIndex = 103;
            this.DoStarColleaguePositions.Text = "St&ar Colleague Positions";
            this.DoStarColleaguePositions.UseVisualStyleBackColor = true;
            // 
            // alternateTableName
            // 
            this.alternateTableName.Enabled = false;
            this.alternateTableName.Location = new System.Drawing.Point(245, 222);
            this.alternateTableName.Name = "alternateTableName";
            this.alternateTableName.Size = new System.Drawing.Size(277, 20);
            this.alternateTableName.TabIndex = 240;
            // 
            // useAlternateCheckbox
            // 
            this.useAlternateCheckbox.AutoSize = true;
            this.useAlternateCheckbox.Location = new System.Drawing.Point(34, 224);
            this.useAlternateCheckbox.Name = "useAlternateCheckbox";
            this.useAlternateCheckbox.Size = new System.Drawing.Size(211, 17);
            this.useAlternateCheckbox.TabIndex = 239;
            this.useAlternateCheckbox.Text = "Use alternate PeoplePublications table:";
            this.useAlternateCheckbox.UseVisualStyleBackColor = true;
            // 
            // ReportsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 418);
            this.Controls.Add(this.alternateTableName);
            this.Controls.Add(this.useAlternateCheckbox);
            this.Controls.Add(this.StarColleaguePositions);
            this.Controls.Add(this.DoStarColleaguePositions);
            this.Controls.Add(this.StarColleaguesRadioButtons);
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
            this.Controls.Add(this.Colleagues);
            this.Controls.Add(this.Publications);
            this.Controls.Add(this.StarColleagues);
            this.Controls.Add(this.DoStarColleagues);
            this.Controls.Add(this.DoPublications);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DoColleagues);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.StarColleaguesRadioButtons.ResumeLayout(false);
            this.StarColleaguesRadioButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox DoColleagues;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox DoPublications;
        private System.Windows.Forms.CheckBox DoStarColleagues;
        private System.Windows.Forms.TextBox StarColleagues;
        private System.Windows.Forms.TextBox Publications;
        private System.Windows.Forms.TextBox Colleagues;
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
        private System.Windows.Forms.Panel StarColleaguesRadioButtons;
        private System.Windows.Forms.RadioButton ContinueStarColleagues;
        private System.Windows.Forms.RadioButton OverwriteStarColleagues;
        private System.Windows.Forms.TextBox StarColleaguePositions;
        private System.Windows.Forms.CheckBox DoStarColleaguePositions;
        private System.Windows.Forms.TextBox alternateTableName;
        private System.Windows.Forms.CheckBox useAlternateCheckbox;
    }
}