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
            this.colleaguePublicationsTable = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // About
            // 
            this.About.Location = new System.Drawing.Point(440, 30);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(62, 49);
            this.About.TabIndex = 206;
            this.About.Text = "&About SC/Gen";
            this.About.UseVisualStyleBackColor = true;
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // RosterFileDialog
            // 
            this.RosterFileDialog.AutoEllipsis = true;
            this.RosterFileDialog.Location = new System.Drawing.Point(406, 66);
            this.RosterFileDialog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RosterFileDialog.Name = "RosterFileDialog";
            this.RosterFileDialog.Size = new System.Drawing.Size(16, 19);
            this.RosterFileDialog.TabIndex = 205;
            this.RosterFileDialog.Text = "...";
            this.RosterFileDialog.Click += new System.EventHandler(this.RosterFileDialog_Click);
            // 
            // RosterFile
            // 
            this.RosterFile.Location = new System.Drawing.Point(11, 66);
            this.RosterFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RosterFile.Name = "RosterFile";
            this.RosterFile.Size = new System.Drawing.Size(389, 20);
            this.RosterFile.TabIndex = 204;
            this.RosterFile.TextChanged += new System.EventHandler(this.RosterFile_TextChanged);
            // 
            // DSN
            // 
            this.DSN.FormattingEnabled = true;
            this.DSN.Location = new System.Drawing.Point(13, 23);
            this.DSN.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DSN.Name = "DSN";
            this.DSN.Size = new System.Drawing.Size(387, 21);
            this.DSN.TabIndex = 201;
            this.DSN.SelectedIndexChanged += new System.EventHandler(this.DSN_SelectedIndexChanged);
            this.DSN.Click += new System.EventHandler(this.DSN_Click);
            // 
            // ODBCPanel
            // 
            this.ODBCPanel.AutoEllipsis = true;
            this.ODBCPanel.Location = new System.Drawing.Point(406, 25);
            this.ODBCPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ODBCPanel.Name = "ODBCPanel";
            this.ODBCPanel.Size = new System.Drawing.Size(16, 19);
            this.ODBCPanel.TabIndex = 202;
            this.ODBCPanel.Text = "...";
            this.ODBCPanel.Click += new System.EventHandler(this.ODBCPanel_Click);
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(11, 10);
            this.Label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(162, 19);
            this.Label2.TabIndex = 200;
            this.Label2.Text = "&ODBC Data Source Name";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 19);
            this.label1.TabIndex = 203;
            this.label1.Text = "&Roster File";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(46, 17);
            this.toolStripStatusLabel2.Text = "Version";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 658);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(511, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 199;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(348, 17);
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
            this.groupBox1.Location = new System.Drawing.Point(13, 93);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(490, 125);
            this.groupBox1.TabIndex = 208;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Status";
            // 
            // ColleaguesWithErrors
            // 
            this.ColleaguesWithErrors.Location = new System.Drawing.Point(370, 91);
            this.ColleaguesWithErrors.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ColleaguesWithErrors.Name = "ColleaguesWithErrors";
            this.ColleaguesWithErrors.ReadOnly = true;
            this.ColleaguesWithErrors.Size = new System.Drawing.Size(106, 20);
            this.ColleaguesWithErrors.TabIndex = 76;
            this.ColleaguesWithErrors.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(367, 76);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(114, 13);
            this.label16.TabIndex = 75;
            this.label16.Text = "Colleagues With Errors";
            // 
            // PeopleNotHarvested
            // 
            this.PeopleNotHarvested.Location = new System.Drawing.Point(136, 91);
            this.PeopleNotHarvested.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PeopleNotHarvested.Name = "PeopleNotHarvested";
            this.PeopleNotHarvested.ReadOnly = true;
            this.PeopleNotHarvested.Size = new System.Drawing.Size(106, 20);
            this.PeopleNotHarvested.TabIndex = 74;
            this.PeopleNotHarvested.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(134, 75);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 13);
            this.label10.TabIndex = 73;
            this.label10.Text = "People Not Harvested";
            // 
            // PeopleWithErrors
            // 
            this.PeopleWithErrors.Location = new System.Drawing.Point(19, 91);
            this.PeopleWithErrors.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PeopleWithErrors.Name = "PeopleWithErrors";
            this.PeopleWithErrors.ReadOnly = true;
            this.PeopleWithErrors.Size = new System.Drawing.Size(106, 20);
            this.PeopleWithErrors.TabIndex = 72;
            this.PeopleWithErrors.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 75);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 13);
            this.label11.TabIndex = 71;
            this.label11.Text = "People With Errors";
            // 
            // PublicationsFound
            // 
            this.PublicationsFound.Location = new System.Drawing.Point(370, 36);
            this.PublicationsFound.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PublicationsFound.Name = "PublicationsFound";
            this.PublicationsFound.ReadOnly = true;
            this.PublicationsFound.Size = new System.Drawing.Size(106, 20);
            this.PublicationsFound.TabIndex = 69;
            this.PublicationsFound.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(368, 20);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 13);
            this.label9.TabIndex = 68;
            this.label9.Text = "Publications Found";
            // 
            // PeopleHarvested
            // 
            this.PeopleHarvested.Location = new System.Drawing.Point(253, 36);
            this.PeopleHarvested.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PeopleHarvested.Name = "PeopleHarvested";
            this.PeopleHarvested.ReadOnly = true;
            this.PeopleHarvested.Size = new System.Drawing.Size(106, 20);
            this.PeopleHarvested.TabIndex = 67;
            this.PeopleHarvested.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(250, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 13);
            this.label8.TabIndex = 66;
            this.label8.Text = "People Harvested";
            // 
            // People
            // 
            this.People.Location = new System.Drawing.Point(136, 36);
            this.People.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.People.Name = "People";
            this.People.ReadOnly = true;
            this.People.Size = new System.Drawing.Size(106, 20);
            this.People.TabIndex = 65;
            this.People.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(134, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 64;
            this.label7.Text = "People";
            // 
            // TablesCreated
            // 
            this.TablesCreated.Location = new System.Drawing.Point(19, 36);
            this.TablesCreated.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TablesCreated.Name = "TablesCreated";
            this.TablesCreated.ReadOnly = true;
            this.TablesCreated.Size = new System.Drawing.Size(106, 20);
            this.TablesCreated.TabIndex = 63;
            this.TablesCreated.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 20);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 62;
            this.label6.Text = "Tables Created";
            // 
            // ReadRoster
            // 
            this.ReadRoster.Location = new System.Drawing.Point(12, 232);
            this.ReadRoster.Name = "ReadRoster";
            this.ReadRoster.Size = new System.Drawing.Size(252, 23);
            this.ReadRoster.TabIndex = 209;
            this.ReadRoster.Text = "Step 1: Read the Roster file";
            this.ReadRoster.UseVisualStyleBackColor = true;
            this.ReadRoster.Click += new System.EventHandler(this.ReadRoster_Click);
            // 
            // FindPotentialColleagues
            // 
            this.FindPotentialColleagues.Location = new System.Drawing.Point(12, 265);
            this.FindPotentialColleagues.Name = "FindPotentialColleagues";
            this.FindPotentialColleagues.Size = new System.Drawing.Size(252, 23);
            this.FindPotentialColleagues.TabIndex = 210;
            this.FindPotentialColleagues.Text = "Step 2: Find the Potential Colleagues";
            this.FindPotentialColleagues.UseVisualStyleBackColor = true;
            this.FindPotentialColleagues.Click += new System.EventHandler(this.FindPotentialColleagues_Click);
            // 
            // RetrieveColleaguePublications
            // 
            this.RetrieveColleaguePublications.Location = new System.Drawing.Point(12, 330);
            this.RetrieveColleaguePublications.Name = "RetrieveColleaguePublications";
            this.RetrieveColleaguePublications.Size = new System.Drawing.Size(252, 23);
            this.RetrieveColleaguePublications.TabIndex = 211;
            this.RetrieveColleaguePublications.Text = "Step 4: Retrieve Missing Colleague Publications";
            this.RetrieveColleaguePublications.UseVisualStyleBackColor = true;
            this.RetrieveColleaguePublications.Click += new System.EventHandler(this.RetrieveColleaguePublications_Click);
            // 
            // RosterRows
            // 
            this.RosterRows.Location = new System.Drawing.Point(447, 235);
            this.RosterRows.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RosterRows.Name = "RosterRows";
            this.RosterRows.ReadOnly = true;
            this.RosterRows.Size = new System.Drawing.Size(55, 20);
            this.RosterRows.TabIndex = 213;
            this.RosterRows.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(376, 237);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 212;
            this.label3.Text = "Roster Rows";
            // 
            // DiadsFound
            // 
            this.DiadsFound.Location = new System.Drawing.Point(448, 300);
            this.DiadsFound.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DiadsFound.Name = "DiadsFound";
            this.DiadsFound.ReadOnly = true;
            this.DiadsFound.Size = new System.Drawing.Size(56, 20);
            this.DiadsFound.TabIndex = 215;
            this.DiadsFound.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(308, 300);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 13);
            this.label4.TabIndex = 214;
            this.label4.Text = "Star/Colleague Pairs Found";
            // 
            // ColleaguePublications
            // 
            this.ColleaguePublications.Location = new System.Drawing.Point(448, 365);
            this.ColleaguePublications.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ColleaguePublications.Name = "ColleaguePublications";
            this.ColleaguePublications.ReadOnly = true;
            this.ColleaguePublications.Size = new System.Drawing.Size(55, 20);
            this.ColleaguePublications.TabIndex = 217;
            this.ColleaguePublications.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(269, 367);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(177, 13);
            this.label5.TabIndex = 216;
            this.label5.Text = "Colleague Publications Downloaded";
            // 
            // ColleaguesHarvested
            // 
            this.ColleaguesHarvested.Location = new System.Drawing.Point(448, 332);
            this.ColleaguesHarvested.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ColleaguesHarvested.Name = "ColleaguesHarvested";
            this.ColleaguesHarvested.ReadOnly = true;
            this.ColleaguesHarvested.Size = new System.Drawing.Size(56, 20);
            this.ColleaguesHarvested.TabIndex = 219;
            this.ColleaguesHarvested.TabStop = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(334, 335);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(111, 13);
            this.label15.TabIndex = 218;
            this.label15.Text = "Colleagues Harvested";
            // 
            // GenerateReports
            // 
            this.GenerateReports.Location = new System.Drawing.Point(12, 395);
            this.GenerateReports.Name = "GenerateReports";
            this.GenerateReports.Size = new System.Drawing.Size(252, 23);
            this.GenerateReports.TabIndex = 220;
            this.GenerateReports.Text = "Step 6: Generate Reports";
            this.GenerateReports.UseVisualStyleBackColor = true;
            this.GenerateReports.Click += new System.EventHandler(this.GenerateReports_Click);
            // 
            // LogFilename
            // 
            this.LogFilename.Location = new System.Drawing.Point(14, 522);
            this.LogFilename.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LogFilename.Name = "LogFilename";
            this.LogFilename.ReadOnly = true;
            this.LogFilename.Size = new System.Drawing.Size(379, 20);
            this.LogFilename.TabIndex = 223;
            this.LogFilename.TabStop = false;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(14, 509);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(162, 19);
            this.label12.TabIndex = 222;
            this.label12.Text = "&Log file";
            // 
            // OpenInNotepad
            // 
            this.OpenInNotepad.Enabled = false;
            this.OpenInNotepad.Location = new System.Drawing.Point(398, 522);
            this.OpenInNotepad.Name = "OpenInNotepad";
            this.OpenInNotepad.Size = new System.Drawing.Size(105, 20);
            this.OpenInNotepad.TabIndex = 224;
            this.OpenInNotepad.Text = "Open in &Notepad";
            this.OpenInNotepad.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.OpenInNotepad.UseVisualStyleBackColor = true;
            this.OpenInNotepad.Click += new System.EventHandler(this.OpenInNotepad_Click);
            // 
            // Log
            // 
            this.Log.FormattingEnabled = true;
            this.Log.HorizontalScrollbar = true;
            this.Log.Location = new System.Drawing.Point(14, 557);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(489, 95);
            this.Log.TabIndex = 226;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(14, 543);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(162, 17);
            this.label13.TabIndex = 225;
            this.label13.Text = "Log";
            // 
            // RemoveFalseColleagues
            // 
            this.RemoveFalseColleagues.Location = new System.Drawing.Point(12, 362);
            this.RemoveFalseColleagues.Name = "RemoveFalseColleagues";
            this.RemoveFalseColleagues.Size = new System.Drawing.Size(252, 23);
            this.RemoveFalseColleagues.TabIndex = 227;
            this.RemoveFalseColleagues.Text = "Step 5: Remove False Colleagues";
            this.RemoveFalseColleagues.UseVisualStyleBackColor = true;
            this.RemoveFalseColleagues.Click += new System.EventHandler(this.RemoveFalseColleagues_Click);
            // 
            // CopyPublicationsFromAnotherDB
            // 
            this.CopyPublicationsFromAnotherDB.Location = new System.Drawing.Point(12, 297);
            this.CopyPublicationsFromAnotherDB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CopyPublicationsFromAnotherDB.Name = "CopyPublicationsFromAnotherDB";
            this.CopyPublicationsFromAnotherDB.Size = new System.Drawing.Size(252, 23);
            this.CopyPublicationsFromAnotherDB.TabIndex = 228;
            this.CopyPublicationsFromAnotherDB.Text = "Step 3: Copy Publications from Another Database";
            this.CopyPublicationsFromAnotherDB.UseVisualStyleBackColor = true;
            this.CopyPublicationsFromAnotherDB.Click += new System.EventHandler(this.CopyPublicationsFromAnotherDB_Click);
            // 
            // StarsWithColleagues
            // 
            this.StarsWithColleagues.Location = new System.Drawing.Point(448, 267);
            this.StarsWithColleagues.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.StarsWithColleagues.Name = "StarsWithColleagues";
            this.StarsWithColleagues.ReadOnly = true;
            this.StarsWithColleagues.Size = new System.Drawing.Size(54, 20);
            this.StarsWithColleagues.TabIndex = 230;
            this.StarsWithColleagues.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(337, 267);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(108, 13);
            this.label14.TabIndex = 229;
            this.label14.Text = "Stars with Colleagues";
            // 
            // LanguageList
            // 
            this.LanguageList.Location = new System.Drawing.Point(15, 450);
            this.LanguageList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LanguageList.Name = "LanguageList";
            this.LanguageList.Size = new System.Drawing.Size(488, 20);
            this.LanguageList.TabIndex = 232;
            this.LanguageList.Text = "eng";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(11, 436);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(514, 19);
            this.label17.TabIndex = 231;
            this.label17.Text = "Languages (list of Medline language abbreviations separated by commas, blank for " +
                "no restriction)";
            // 
            // AllowedPubTypeCategories
            // 
            this.AllowedPubTypeCategories.Location = new System.Drawing.Point(15, 488);
            this.AllowedPubTypeCategories.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.AllowedPubTypeCategories.Name = "AllowedPubTypeCategories";
            this.AllowedPubTypeCategories.Size = new System.Drawing.Size(265, 20);
            this.AllowedPubTypeCategories.TabIndex = 234;
            this.AllowedPubTypeCategories.Text = "1,2,3";
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(11, 474);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(514, 19);
            this.label18.TabIndex = 233;
            this.label18.Text = "Allowed publication type categories";
            // 
            // ColleaguesFound
            // 
            this.ColleaguesFound.Location = new System.Drawing.Point(448, 397);
            this.ColleaguesFound.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ColleaguesFound.Name = "ColleaguesFound";
            this.ColleaguesFound.ReadOnly = true;
            this.ColleaguesFound.Size = new System.Drawing.Size(55, 20);
            this.ColleaguesFound.TabIndex = 236;
            this.ColleaguesFound.TabStop = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(316, 400);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(129, 13);
            this.label19.TabIndex = 235;
            this.label19.Text = "Unique Colleagues Found";
            // 
            // colleaguePublicationsTable
            // 
            this.colleaguePublicationsTable.Location = new System.Drawing.Point(302, 488);
            this.colleaguePublicationsTable.Name = "colleaguePublicationsTable";
            this.colleaguePublicationsTable.Size = new System.Drawing.Size(202, 20);
            this.colleaguePublicationsTable.TabIndex = 238;
            this.colleaguePublicationsTable.Text = "ColleaguePublications";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(300, 472);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(133, 13);
            this.label20.TabIndex = 237;
            this.label20.Text = "Collague publications table";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 680);
            this.Controls.Add(this.colleaguePublicationsTable);
            this.Controls.Add(this.label20);
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
        private System.Windows.Forms.TextBox colleaguePublicationsTable;
        private System.Windows.Forms.Label label20;
    }
}

