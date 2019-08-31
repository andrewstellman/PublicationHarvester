namespace SCGen
{
    partial class Form1
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
            this.About = new System.Windows.Forms.Button();
            this.RosterFileDialog = new System.Windows.Forms.Button();
            this.RosterFile = new System.Windows.Forms.TextBox();
            this.DSN = new System.Windows.Forms.ComboBox();
            this.ODBCPanel = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ColleaguesWithErrors = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.PeopleNotHarvested = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.PeopleWithErrors = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.PublicationsFound = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.PeopleHarvested = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.People = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TablesCreated = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ReadRoster = new System.Windows.Forms.Button();
            this.FindPotentialColleagues = new System.Windows.Forms.Button();
            this.RetrieveColleaguePublications = new System.Windows.Forms.Button();
            this.RosterRows = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DiadsFound = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ColleaguePublications = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ColleaguesHarvested = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.GenerateReports = new System.Windows.Forms.Button();
            this.LogFilename = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.OpenInNotepad = new System.Windows.Forms.Button();
            this.Log = new System.Windows.Forms.ListBox();
            this.label13 = new System.Windows.Forms.Label();
            this.RemoveFalseColleagues = new System.Windows.Forms.Button();
            this.CopyPublicationsFromAnotherDB = new System.Windows.Forms.Button();
            this.StarsWithColleagues = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.LanguageList = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.AllowedPubTypeCategories = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.ColleaguesFound = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.useAlternateCheckbox = new System.Windows.Forms.CheckBox();
            this.alternateTableName = new System.Windows.Forms.TextBox();
            this.ApiKeyFileButton = new System.Windows.Forms.Button();
            this.ApiKeyFile = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // About
            // 
            this.About.Location = new System.Drawing.Point(660, 46);
            this.About.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(93, 75);
            this.About.TabIndex = 95;
            this.About.Text = "&About SC/Gen";
            this.About.UseVisualStyleBackColor = true;
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // RosterFileDialog
            // 
            this.RosterFileDialog.AutoEllipsis = true;
            this.RosterFileDialog.Location = new System.Drawing.Point(610, 94);
            this.RosterFileDialog.Name = "RosterFileDialog";
            this.RosterFileDialog.Size = new System.Drawing.Size(24, 29);
            this.RosterFileDialog.TabIndex = 60;
            this.RosterFileDialog.Text = "...";
            this.RosterFileDialog.Click += new System.EventHandler(this.RosterFileDialog_Click);
            // 
            // RosterFile
            // 
            this.RosterFile.Location = new System.Drawing.Point(20, 94);
            this.RosterFile.Name = "RosterFile";
            this.RosterFile.Size = new System.Drawing.Size(578, 26);
            this.RosterFile.TabIndex = 50;
            this.RosterFile.TextChanged += new System.EventHandler(this.RosterFile_TextChanged);
            // 
            // DSN
            // 
            this.DSN.FormattingEnabled = true;
            this.DSN.Location = new System.Drawing.Point(20, 35);
            this.DSN.Name = "DSN";
            this.DSN.Size = new System.Drawing.Size(578, 28);
            this.DSN.TabIndex = 20;
            this.DSN.SelectedIndexChanged += new System.EventHandler(this.DSN_SelectedIndexChanged);
            this.DSN.Click += new System.EventHandler(this.DSN_Click);
            // 
            // ODBCPanel
            // 
            this.ODBCPanel.AutoEllipsis = true;
            this.ODBCPanel.Location = new System.Drawing.Point(610, 38);
            this.ODBCPanel.Name = "ODBCPanel";
            this.ODBCPanel.Size = new System.Drawing.Size(24, 29);
            this.ODBCPanel.TabIndex = 30;
            this.ODBCPanel.Text = "...";
            this.ODBCPanel.Click += new System.EventHandler(this.ODBCPanel_Click);
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(16, 15);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(243, 29);
            this.Label2.TabIndex = 10;
            this.Label2.Text = "&ODBC Data Source Name";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 29);
            this.label1.TabIndex = 40;
            this.label1.Text = "&Roster File";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(70, 25);
            this.toolStripStatusLabel2.Text = "Version";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1018);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(766, 32);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 199;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(150, 24);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(519, 25);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ColleaguesWithErrors);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.PeopleNotHarvested);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.PeopleWithErrors);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.PublicationsFound);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.PeopleHarvested);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.People);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.TablesCreated);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(20, 194);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(735, 192);
            this.groupBox1.TabIndex = 100;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Status";
            // 
            // ColleaguesWithErrors
            // 
            this.ColleaguesWithErrors.Location = new System.Drawing.Point(555, 140);
            this.ColleaguesWithErrors.Name = "ColleaguesWithErrors";
            this.ColleaguesWithErrors.ReadOnly = true;
            this.ColleaguesWithErrors.Size = new System.Drawing.Size(157, 26);
            this.ColleaguesWithErrors.TabIndex = 76;
            this.ColleaguesWithErrors.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(550, 117);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(171, 20);
            this.label16.TabIndex = 75;
            this.label16.Text = "Colleagues With Errors";
            // 
            // PeopleNotHarvested
            // 
            this.PeopleNotHarvested.Location = new System.Drawing.Point(204, 140);
            this.PeopleNotHarvested.Name = "PeopleNotHarvested";
            this.PeopleNotHarvested.ReadOnly = true;
            this.PeopleNotHarvested.Size = new System.Drawing.Size(157, 26);
            this.PeopleNotHarvested.TabIndex = 74;
            this.PeopleNotHarvested.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(201, 115);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(164, 20);
            this.label10.TabIndex = 73;
            this.label10.Text = "People Not Harvested";
            // 
            // PeopleWithErrors
            // 
            this.PeopleWithErrors.Location = new System.Drawing.Point(28, 140);
            this.PeopleWithErrors.Name = "PeopleWithErrors";
            this.PeopleWithErrors.ReadOnly = true;
            this.PeopleWithErrors.Size = new System.Drawing.Size(157, 26);
            this.PeopleWithErrors.TabIndex = 72;
            this.PeopleWithErrors.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(24, 115);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(141, 20);
            this.label11.TabIndex = 71;
            this.label11.Text = "People With Errors";
            // 
            // PublicationsFound
            // 
            this.PublicationsFound.Location = new System.Drawing.Point(555, 55);
            this.PublicationsFound.Name = "PublicationsFound";
            this.PublicationsFound.ReadOnly = true;
            this.PublicationsFound.Size = new System.Drawing.Size(157, 26);
            this.PublicationsFound.TabIndex = 69;
            this.PublicationsFound.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(552, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(144, 20);
            this.label9.TabIndex = 68;
            this.label9.Text = "Publications Found";
            // 
            // PeopleHarvested
            // 
            this.PeopleHarvested.Location = new System.Drawing.Point(380, 55);
            this.PeopleHarvested.Name = "PeopleHarvested";
            this.PeopleHarvested.ReadOnly = true;
            this.PeopleHarvested.Size = new System.Drawing.Size(157, 26);
            this.PeopleHarvested.TabIndex = 67;
            this.PeopleHarvested.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(375, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 20);
            this.label8.TabIndex = 66;
            this.label8.Text = "People Harvested";
            // 
            // People
            // 
            this.People.Location = new System.Drawing.Point(204, 55);
            this.People.Name = "People";
            this.People.ReadOnly = true;
            this.People.Size = new System.Drawing.Size(157, 26);
            this.People.TabIndex = 65;
            this.People.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(201, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 20);
            this.label7.TabIndex = 64;
            this.label7.Text = "People";
            // 
            // TablesCreated
            // 
            this.TablesCreated.Location = new System.Drawing.Point(28, 55);
            this.TablesCreated.Name = "TablesCreated";
            this.TablesCreated.ReadOnly = true;
            this.TablesCreated.Size = new System.Drawing.Size(157, 26);
            this.TablesCreated.TabIndex = 63;
            this.TablesCreated.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 20);
            this.label6.TabIndex = 62;
            this.label6.Text = "Tables Created";
            // 
            // ReadRoster
            // 
            this.ReadRoster.Location = new System.Drawing.Point(18, 408);
            this.ReadRoster.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ReadRoster.Name = "ReadRoster";
            this.ReadRoster.Size = new System.Drawing.Size(378, 35);
            this.ReadRoster.TabIndex = 110;
            this.ReadRoster.Text = "Step 1: Read the Roster file";
            this.ReadRoster.UseVisualStyleBackColor = true;
            this.ReadRoster.Click += new System.EventHandler(this.ReadRoster_Click);
            // 
            // FindPotentialColleagues
            // 
            this.FindPotentialColleagues.Location = new System.Drawing.Point(18, 459);
            this.FindPotentialColleagues.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FindPotentialColleagues.Name = "FindPotentialColleagues";
            this.FindPotentialColleagues.Size = new System.Drawing.Size(378, 35);
            this.FindPotentialColleagues.TabIndex = 120;
            this.FindPotentialColleagues.Text = "Step 2: Find the Potential Colleagues";
            this.FindPotentialColleagues.UseVisualStyleBackColor = true;
            this.FindPotentialColleagues.Click += new System.EventHandler(this.FindPotentialColleagues_Click);
            // 
            // RetrieveColleaguePublications
            // 
            this.RetrieveColleaguePublications.Location = new System.Drawing.Point(15, 590);
            this.RetrieveColleaguePublications.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RetrieveColleaguePublications.Name = "RetrieveColleaguePublications";
            this.RetrieveColleaguePublications.Size = new System.Drawing.Size(378, 35);
            this.RetrieveColleaguePublications.TabIndex = 160;
            this.RetrieveColleaguePublications.Text = "Step 4: Retrieve Missing Colleague Publications";
            this.RetrieveColleaguePublications.UseVisualStyleBackColor = true;
            this.RetrieveColleaguePublications.Click += new System.EventHandler(this.RetrieveColleaguePublications_Click);
            // 
            // RosterRows
            // 
            this.RosterRows.Location = new System.Drawing.Point(670, 413);
            this.RosterRows.Name = "RosterRows";
            this.RosterRows.ReadOnly = true;
            this.RosterRows.Size = new System.Drawing.Size(80, 26);
            this.RosterRows.TabIndex = 213;
            this.RosterRows.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(564, 416);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 20);
            this.label3.TabIndex = 212;
            this.label3.Text = "Roster Rows";
            // 
            // DiadsFound
            // 
            this.DiadsFound.Location = new System.Drawing.Point(669, 544);
            this.DiadsFound.Name = "DiadsFound";
            this.DiadsFound.ReadOnly = true;
            this.DiadsFound.Size = new System.Drawing.Size(82, 26);
            this.DiadsFound.TabIndex = 215;
            this.DiadsFound.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(459, 544);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(203, 20);
            this.label4.TabIndex = 214;
            this.label4.Text = "Star/Colleague Pairs Found";
            // 
            // ColleaguePublications
            // 
            this.ColleaguePublications.Location = new System.Drawing.Point(669, 644);
            this.ColleaguePublications.Name = "ColleaguePublications";
            this.ColleaguePublications.ReadOnly = true;
            this.ColleaguePublications.Size = new System.Drawing.Size(80, 26);
            this.ColleaguePublications.TabIndex = 217;
            this.ColleaguePublications.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(400, 647);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(262, 20);
            this.label5.TabIndex = 216;
            this.label5.Text = "Colleague Publications Downloaded";
            // 
            // ColleaguesHarvested
            // 
            this.ColleaguesHarvested.Location = new System.Drawing.Point(669, 593);
            this.ColleaguesHarvested.Name = "ColleaguesHarvested";
            this.ColleaguesHarvested.ReadOnly = true;
            this.ColleaguesHarvested.Size = new System.Drawing.Size(82, 26);
            this.ColleaguesHarvested.TabIndex = 219;
            this.ColleaguesHarvested.TabStop = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(498, 598);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(165, 20);
            this.label15.TabIndex = 218;
            this.label15.Text = "Colleagues Harvested";
            // 
            // GenerateReports
            // 
            this.GenerateReports.Location = new System.Drawing.Point(15, 690);
            this.GenerateReports.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GenerateReports.Name = "GenerateReports";
            this.GenerateReports.Size = new System.Drawing.Size(378, 35);
            this.GenerateReports.TabIndex = 180;
            this.GenerateReports.Text = "Step 6: Generate Reports";
            this.GenerateReports.UseVisualStyleBackColor = true;
            this.GenerateReports.Click += new System.EventHandler(this.GenerateReports_Click);
            // 
            // LogFilename
            // 
            this.LogFilename.Location = new System.Drawing.Point(18, 872);
            this.LogFilename.Name = "LogFilename";
            this.LogFilename.ReadOnly = true;
            this.LogFilename.Size = new System.Drawing.Size(566, 26);
            this.LogFilename.TabIndex = 250;
            this.LogFilename.TabStop = false;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(18, 852);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(243, 29);
            this.label12.TabIndex = 240;
            this.label12.Text = "&Log file";
            // 
            // OpenInNotepad
            // 
            this.OpenInNotepad.Enabled = false;
            this.OpenInNotepad.Location = new System.Drawing.Point(594, 872);
            this.OpenInNotepad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OpenInNotepad.Name = "OpenInNotepad";
            this.OpenInNotepad.Size = new System.Drawing.Size(158, 31);
            this.OpenInNotepad.TabIndex = 260;
            this.OpenInNotepad.Text = "Open in &Notepad";
            this.OpenInNotepad.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.OpenInNotepad.UseVisualStyleBackColor = true;
            this.OpenInNotepad.Click += new System.EventHandler(this.OpenInNotepad_Click);
            // 
            // Log
            // 
            this.Log.FormattingEnabled = true;
            this.Log.HorizontalScrollbar = true;
            this.Log.ItemHeight = 20;
            this.Log.Location = new System.Drawing.Point(18, 925);
            this.Log.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(732, 84);
            this.Log.TabIndex = 310;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(18, 904);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(243, 26);
            this.label13.TabIndex = 300;
            this.label13.Text = "Log";
            // 
            // RemoveFalseColleagues
            // 
            this.RemoveFalseColleagues.Location = new System.Drawing.Point(15, 639);
            this.RemoveFalseColleagues.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RemoveFalseColleagues.Name = "RemoveFalseColleagues";
            this.RemoveFalseColleagues.Size = new System.Drawing.Size(378, 35);
            this.RemoveFalseColleagues.TabIndex = 170;
            this.RemoveFalseColleagues.Text = "Step 5: Remove False Colleagues";
            this.RemoveFalseColleagues.UseVisualStyleBackColor = true;
            this.RemoveFalseColleagues.Click += new System.EventHandler(this.RemoveFalseColleagues_Click);
            // 
            // CopyPublicationsFromAnotherDB
            // 
            this.CopyPublicationsFromAnotherDB.Location = new System.Drawing.Point(15, 539);
            this.CopyPublicationsFromAnotherDB.Name = "CopyPublicationsFromAnotherDB";
            this.CopyPublicationsFromAnotherDB.Size = new System.Drawing.Size(378, 35);
            this.CopyPublicationsFromAnotherDB.TabIndex = 150;
            this.CopyPublicationsFromAnotherDB.Text = "Step 3: Copy Publications from Another Database";
            this.CopyPublicationsFromAnotherDB.UseVisualStyleBackColor = true;
            this.CopyPublicationsFromAnotherDB.Click += new System.EventHandler(this.CopyPublicationsFromAnotherDB_Click);
            // 
            // StarsWithColleagues
            // 
            this.StarsWithColleagues.Location = new System.Drawing.Point(672, 462);
            this.StarsWithColleagues.Name = "StarsWithColleagues";
            this.StarsWithColleagues.ReadOnly = true;
            this.StarsWithColleagues.Size = new System.Drawing.Size(79, 26);
            this.StarsWithColleagues.TabIndex = 230;
            this.StarsWithColleagues.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(506, 462);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(162, 20);
            this.label14.TabIndex = 229;
            this.label14.Text = "Stars with Colleagues";
            // 
            // LanguageList
            // 
            this.LanguageList.Location = new System.Drawing.Point(20, 761);
            this.LanguageList.Name = "LanguageList";
            this.LanguageList.Size = new System.Drawing.Size(730, 26);
            this.LanguageList.TabIndex = 210;
            this.LanguageList.Text = "eng";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(14, 739);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(771, 29);
            this.label17.TabIndex = 200;
            this.label17.Text = "Languages (list of Medline language abbreviations separated by commas, blank for " +
    "no restriction)";
            // 
            // AllowedPubTypeCategories
            // 
            this.AllowedPubTypeCategories.Location = new System.Drawing.Point(20, 819);
            this.AllowedPubTypeCategories.Name = "AllowedPubTypeCategories";
            this.AllowedPubTypeCategories.Size = new System.Drawing.Size(730, 26);
            this.AllowedPubTypeCategories.TabIndex = 230;
            this.AllowedPubTypeCategories.Text = "1,2,3";
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(14, 798);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(771, 29);
            this.label18.TabIndex = 220;
            this.label18.Text = "Allowed publication type categories";
            // 
            // ColleaguesFound
            // 
            this.ColleaguesFound.Location = new System.Drawing.Point(669, 693);
            this.ColleaguesFound.Name = "ColleaguesFound";
            this.ColleaguesFound.ReadOnly = true;
            this.ColleaguesFound.Size = new System.Drawing.Size(80, 26);
            this.ColleaguesFound.TabIndex = 236;
            this.ColleaguesFound.TabStop = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(471, 698);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(193, 20);
            this.label19.TabIndex = 235;
            this.label19.Text = "Unique Colleagues Found";
            // 
            // useAlternateCheckbox
            // 
            this.useAlternateCheckbox.AutoSize = true;
            this.useAlternateCheckbox.Location = new System.Drawing.Point(22, 503);
            this.useAlternateCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.useAlternateCheckbox.Name = "useAlternateCheckbox";
            this.useAlternateCheckbox.Size = new System.Drawing.Size(312, 24);
            this.useAlternateCheckbox.TabIndex = 130;
            this.useAlternateCheckbox.Text = "Use alternate PeoplePublications table:";
            this.useAlternateCheckbox.UseVisualStyleBackColor = true;
            this.useAlternateCheckbox.CheckedChanged += new System.EventHandler(this.useAlternateCheckbox_CheckedChanged);
            // 
            // alternateTableName
            // 
            this.alternateTableName.Enabled = false;
            this.alternateTableName.Location = new System.Drawing.Point(339, 500);
            this.alternateTableName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.alternateTableName.Name = "alternateTableName";
            this.alternateTableName.Size = new System.Drawing.Size(414, 26);
            this.alternateTableName.TabIndex = 140;
            this.alternateTableName.TextChanged += new System.EventHandler(this.alternateTableName_TextChanged);
            // 
            // ApiKeyFileButton
            // 
            this.ApiKeyFileButton.AutoEllipsis = true;
            this.ApiKeyFileButton.Location = new System.Drawing.Point(610, 146);
            this.ApiKeyFileButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ApiKeyFileButton.Name = "ApiKeyFileButton";
            this.ApiKeyFileButton.Size = new System.Drawing.Size(24, 29);
            this.ApiKeyFileButton.TabIndex = 90;
            this.ApiKeyFileButton.Text = "...";
            this.ApiKeyFileButton.Click += new System.EventHandler(this.ApiKeyFileButton_Click);
            // 
            // ApiKeyFile
            // 
            this.ApiKeyFile.Location = new System.Drawing.Point(20, 149);
            this.ApiKeyFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ApiKeyFile.Name = "ApiKeyFile";
            this.ApiKeyFile.Size = new System.Drawing.Size(578, 26);
            this.ApiKeyFile.TabIndex = 80;
            this.ApiKeyFile.TextChanged += new System.EventHandler(this.ApiKeyFile_TextChanged);
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(16, 126);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(243, 29);
            this.label20.TabIndex = 70;
            this.label20.Text = "API &key file";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(766, 1050);
            this.Controls.Add(this.ApiKeyFileButton);
            this.Controls.Add(this.ApiKeyFile);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.alternateTableName);
            this.Controls.Add(this.useAlternateCheckbox);
            this.Controls.Add(this.ColleaguesFound);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.AllowedPubTypeCategories);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.LanguageList);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.StarsWithColleagues);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.CopyPublicationsFromAnotherDB);
            this.Controls.Add(this.RemoveFalseColleagues);
            this.Controls.Add(this.LogFilename);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.OpenInNotepad);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.GenerateReports);
            this.Controls.Add(this.ColleaguesHarvested);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.ColleaguePublications);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DiadsFound);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.RosterRows);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RetrieveColleaguePublications);
            this.Controls.Add(this.FindPotentialColleagues);
            this.Controls.Add(this.ReadRoster);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.About);
            this.Controls.Add(this.RosterFileDialog);
            this.Controls.Add(this.RosterFile);
            this.Controls.Add(this.DSN);
            this.Controls.Add(this.ODBCPanel);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "SC/Gen - Stars Colleague Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button About;
        internal System.Windows.Forms.Button RosterFileDialog;
        private System.Windows.Forms.TextBox RosterFile;
        private System.Windows.Forms.ComboBox DSN;
        internal System.Windows.Forms.Button ODBCPanel;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox PeopleNotHarvested;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox PeopleWithErrors;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox PublicationsFound;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox PeopleHarvested;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox People;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TablesCreated;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ReadRoster;
        private System.Windows.Forms.Button FindPotentialColleagues;
        private System.Windows.Forms.Button RetrieveColleaguePublications;
        private System.Windows.Forms.TextBox RosterRows;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox DiadsFound;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ColleaguePublications;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ColleaguesHarvested;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button GenerateReports;
        private System.Windows.Forms.TextBox LogFilename;
        internal System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button OpenInNotepad;
        private System.Windows.Forms.ListBox Log;
        internal System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button RemoveFalseColleagues;
        private System.Windows.Forms.Button CopyPublicationsFromAnotherDB;
        private System.Windows.Forms.TextBox StarsWithColleagues;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox ColleaguesWithErrors;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox LanguageList;
        internal System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox AllowedPubTypeCategories;
        internal System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox ColleaguesFound;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox useAlternateCheckbox;
        private System.Windows.Forms.TextBox alternateTableName;
        internal System.Windows.Forms.Button ApiKeyFileButton;
        private System.Windows.Forms.TextBox ApiKeyFile;
        internal System.Windows.Forms.Label label20;
    }
}

