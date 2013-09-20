namespace Com.StellmanGreene.FindRelated
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
            this.startButton = new System.Windows.Forms.Button();
            this.DSN = new System.Windows.Forms.ComboBox();
            this.ODBCPanel = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.relatedTable = new System.Windows.Forms.TextBox();
            this.logFilename = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.openInNotepad = new System.Windows.Forms.Button();
            this.log = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.cancelButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.inputFileTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.inputFileDialog = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.includeLanguages = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.includeCategories = new System.Windows.Forms.TextBox();
            this.maximumLinkRanking = new System.Windows.Forms.NumericUpDown();
            this.enableMaximumLinkRanking = new System.Windows.Forms.CheckBox();
            this.pubWindowLowerBound = new System.Windows.Forms.NumericUpDown();
            this.enableLowerBound = new System.Windows.Forms.CheckBox();
            this.pubWindowUpperBound = new System.Windows.Forms.NumericUpDown();
            this.enableUpperBound = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.sameJournal = new System.Windows.Forms.CheckBox();
            this.generateReports = new System.Windows.Forms.Button();
            this.peoplePublicationsView = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.resumeButton = new System.Windows.Forms.Button();
            this.liteModeCheckBox = new System.Windows.Forms.CheckBox();
            this.liteModeOutputFileDialog = new System.Windows.Forms.Button();
            this.liteModeOutputTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maximumLinkRanking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pubWindowLowerBound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pubWindowUpperBound)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 360);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 80;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // DSN
            // 
            this.DSN.FormattingEnabled = true;
            this.DSN.Location = new System.Drawing.Point(13, 22);
            this.DSN.Margin = new System.Windows.Forms.Padding(2);
            this.DSN.Name = "DSN";
            this.DSN.Size = new System.Drawing.Size(411, 21);
            this.DSN.TabIndex = 5;
            this.DSN.SelectedIndexChanged += new System.EventHandler(this.DSN_SelectedIndexChanged);
            this.DSN.Click += new System.EventHandler(this.DSN_Click);
            this.DSN.Leave += new System.EventHandler(this.DSN_Leave);
            // 
            // ODBCPanel
            // 
            this.ODBCPanel.AutoEllipsis = true;
            this.ODBCPanel.Location = new System.Drawing.Point(428, 24);
            this.ODBCPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ODBCPanel.Name = "ODBCPanel";
            this.ODBCPanel.Size = new System.Drawing.Size(16, 19);
            this.ODBCPanel.TabIndex = 7;
            this.ODBCPanel.Text = "...";
            this.ODBCPanel.Click += new System.EventHandler(this.ODBCPanel_Click);
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(11, 9);
            this.Label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(162, 19);
            this.Label2.TabIndex = 131;
            this.Label2.Text = "&ODBC Data Source Name";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 19);
            this.label1.TabIndex = 10;
            this.label1.Text = "&Related publications table";
            // 
            // relatedTable
            // 
            this.relatedTable.Location = new System.Drawing.Point(14, 69);
            this.relatedTable.Name = "relatedTable";
            this.relatedTable.Size = new System.Drawing.Size(177, 20);
            this.relatedTable.TabIndex = 15;
            this.relatedTable.TextChanged += new System.EventHandler(this.relatedTable_TextChanged);
            // 
            // logFilename
            // 
            this.logFilename.Location = new System.Drawing.Point(11, 401);
            this.logFilename.Margin = new System.Windows.Forms.Padding(2);
            this.logFilename.Name = "logFilename";
            this.logFilename.ReadOnly = true;
            this.logFilename.Size = new System.Drawing.Size(316, 20);
            this.logFilename.TabIndex = 199;
            this.logFilename.TabStop = false;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(11, 386);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 19);
            this.label5.TabIndex = 198;
            this.label5.Text = "Log file";
            // 
            // openInNotepad
            // 
            this.openInNotepad.Location = new System.Drawing.Point(332, 400);
            this.openInNotepad.Name = "openInNotepad";
            this.openInNotepad.Size = new System.Drawing.Size(105, 20);
            this.openInNotepad.TabIndex = 200;
            this.openInNotepad.Text = "Open in &Notepad";
            this.openInNotepad.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.openInNotepad.UseVisualStyleBackColor = true;
            this.openInNotepad.Click += new System.EventHandler(this.openInNotepad_Click);
            // 
            // log
            // 
            this.log.FormattingEnabled = true;
            this.log.HorizontalScrollbar = true;
            this.log.Location = new System.Drawing.Point(11, 441);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(426, 134);
            this.log.TabIndex = 220;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 426);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 19);
            this.label4.TabIndex = 210;
            this.label4.Text = "Lo&g";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // cancelButton
            // 
            this.cancelButton.Enabled = false;
            this.cancelButton.Location = new System.Drawing.Point(174, 360);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 90;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 578);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(449, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 142;
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
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // inputFileTextBox
            // 
            this.inputFileTextBox.Location = new System.Drawing.Point(14, 105);
            this.inputFileTextBox.Name = "inputFileTextBox";
            this.inputFileTextBox.Size = new System.Drawing.Size(409, 20);
            this.inputFileTextBox.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 92);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 19);
            this.label3.TabIndex = 20;
            this.label3.Text = "&Input file";
            // 
            // inputFileDialog
            // 
            this.inputFileDialog.AutoEllipsis = true;
            this.inputFileDialog.Location = new System.Drawing.Point(428, 106);
            this.inputFileDialog.Margin = new System.Windows.Forms.Padding(2);
            this.inputFileDialog.Name = "inputFileDialog";
            this.inputFileDialog.Size = new System.Drawing.Size(16, 19);
            this.inputFileDialog.TabIndex = 27;
            this.inputFileDialog.Text = "...";
            this.inputFileDialog.Click += new System.EventHandler(this.inputFileDialog_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.includeLanguages);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.includeCategories);
            this.groupBox1.Controls.Add(this.maximumLinkRanking);
            this.groupBox1.Controls.Add(this.enableMaximumLinkRanking);
            this.groupBox1.Controls.Add(this.pubWindowLowerBound);
            this.groupBox1.Controls.Add(this.enableLowerBound);
            this.groupBox1.Controls.Add(this.pubWindowUpperBound);
            this.groupBox1.Controls.Add(this.enableUpperBound);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.sameJournal);
            this.groupBox1.Location = new System.Drawing.Point(12, 190);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 159);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
            // 
            // includeLanguages
            // 
            this.includeLanguages.Location = new System.Drawing.Point(184, 133);
            this.includeLanguages.Name = "includeLanguages";
            this.includeLanguages.Size = new System.Drawing.Size(136, 20);
            this.includeLanguages.TabIndex = 77;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(181, 117);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(176, 13);
            this.label10.TabIndex = 76;
            this.label10.Text = "Restrict lan&guages (eg. eng;fre;heb)";
            // 
            // includeCategories
            // 
            this.includeCategories.Location = new System.Drawing.Point(184, 90);
            this.includeCategories.Name = "includeCategories";
            this.includeCategories.Size = new System.Drawing.Size(136, 20);
            this.includeCategories.TabIndex = 72;
            this.includeCategories.TextChanged += new System.EventHandler(this.includeCategories_TextChanged);
            // 
            // maximumLinkRanking
            // 
            this.maximumLinkRanking.Enabled = false;
            this.maximumLinkRanking.Location = new System.Drawing.Point(30, 110);
            this.maximumLinkRanking.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.maximumLinkRanking.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maximumLinkRanking.Name = "maximumLinkRanking";
            this.maximumLinkRanking.Size = new System.Drawing.Size(55, 20);
            this.maximumLinkRanking.TabIndex = 64;
            this.maximumLinkRanking.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // enableMaximumLinkRanking
            // 
            this.enableMaximumLinkRanking.AutoSize = true;
            this.enableMaximumLinkRanking.Location = new System.Drawing.Point(9, 113);
            this.enableMaximumLinkRanking.Name = "enableMaximumLinkRanking";
            this.enableMaximumLinkRanking.Size = new System.Drawing.Size(15, 14);
            this.enableMaximumLinkRanking.TabIndex = 62;
            this.enableMaximumLinkRanking.UseVisualStyleBackColor = true;
            this.enableMaximumLinkRanking.CheckedChanged += new System.EventHandler(this.enableMaximumLinkRanking_CheckedChanged);
            // 
            // pubWindowLowerBound
            // 
            this.pubWindowLowerBound.Enabled = false;
            this.pubWindowLowerBound.Location = new System.Drawing.Point(30, 71);
            this.pubWindowLowerBound.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.pubWindowLowerBound.Name = "pubWindowLowerBound";
            this.pubWindowLowerBound.Size = new System.Drawing.Size(55, 20);
            this.pubWindowLowerBound.TabIndex = 58;
            // 
            // enableLowerBound
            // 
            this.enableLowerBound.AutoSize = true;
            this.enableLowerBound.Location = new System.Drawing.Point(9, 74);
            this.enableLowerBound.Name = "enableLowerBound";
            this.enableLowerBound.Size = new System.Drawing.Size(15, 14);
            this.enableLowerBound.TabIndex = 44;
            this.enableLowerBound.UseVisualStyleBackColor = true;
            this.enableLowerBound.CheckedChanged += new System.EventHandler(this.enableLowerBound_CheckedChanged);
            // 
            // pubWindowUpperBound
            // 
            this.pubWindowUpperBound.Enabled = false;
            this.pubWindowUpperBound.Location = new System.Drawing.Point(30, 32);
            this.pubWindowUpperBound.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.pubWindowUpperBound.Name = "pubWindowUpperBound";
            this.pubWindowUpperBound.Size = new System.Drawing.Size(55, 20);
            this.pubWindowUpperBound.TabIndex = 38;
            // 
            // enableUpperBound
            // 
            this.enableUpperBound.AutoSize = true;
            this.enableUpperBound.Location = new System.Drawing.Point(9, 35);
            this.enableUpperBound.Name = "enableUpperBound";
            this.enableUpperBound.Size = new System.Drawing.Size(15, 14);
            this.enableUpperBound.TabIndex = 36;
            this.enableUpperBound.UseVisualStyleBackColor = true;
            this.enableUpperBound.CheckedChanged += new System.EventHandler(this.enableUpperBound_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(181, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(149, 13);
            this.label9.TabIndex = 69;
            this.label9.Text = "Restrict &categories (e.g. 1;2;5)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 13);
            this.label8.TabIndex = 60;
            this.label8.Text = "&Maximum  link ranking";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(255, 13);
            this.label7.TabIndex = 40;
            this.label7.Text = "&Lower bound for the publication window (pubdate-t2)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(258, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "&Upper bound for the publication window (pubdate+t1)";
            // 
            // sameJournal
            // 
            this.sameJournal.AutoSize = true;
            this.sameJournal.Location = new System.Drawing.Point(9, 136);
            this.sameJournal.Name = "sameJournal";
            this.sameJournal.Size = new System.Drawing.Size(87, 17);
            this.sameJournal.TabIndex = 75;
            this.sameJournal.Text = "&Same journal";
            this.sameJournal.UseVisualStyleBackColor = true;
            // 
            // generateReports
            // 
            this.generateReports.Location = new System.Drawing.Point(335, 360);
            this.generateReports.Name = "generateReports";
            this.generateReports.Size = new System.Drawing.Size(106, 23);
            this.generateReports.TabIndex = 221;
            this.generateReports.Text = "Generate Re&ports";
            this.generateReports.UseVisualStyleBackColor = true;
            this.generateReports.Click += new System.EventHandler(this.generateReports_Click);
            // 
            // peoplePublicationsView
            // 
            this.peoplePublicationsView.Location = new System.Drawing.Point(196, 69);
            this.peoplePublicationsView.Margin = new System.Windows.Forms.Padding(2);
            this.peoplePublicationsView.Name = "peoplePublicationsView";
            this.peoplePublicationsView.ReadOnly = true;
            this.peoplePublicationsView.Size = new System.Drawing.Size(253, 20);
            this.peoplePublicationsView.TabIndex = 222;
            this.peoplePublicationsView.TabStop = false;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(193, 56);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(234, 19);
            this.label11.TabIndex = 223;
            this.label11.Text = "PeoplePublications view to create/replace";
            // 
            // resumeButton
            // 
            this.resumeButton.Location = new System.Drawing.Point(93, 360);
            this.resumeButton.Name = "resumeButton";
            this.resumeButton.Size = new System.Drawing.Size(75, 23);
            this.resumeButton.TabIndex = 224;
            this.resumeButton.Text = "Resume";
            this.resumeButton.UseVisualStyleBackColor = true;
            this.resumeButton.Click += new System.EventHandler(this.resumeButton_Click);
            // 
            // liteModeCheckBox
            // 
            this.liteModeCheckBox.AutoSize = true;
            this.liteModeCheckBox.Location = new System.Drawing.Point(14, 131);
            this.liteModeCheckBox.Name = "liteModeCheckBox";
            this.liteModeCheckBox.Size = new System.Drawing.Size(365, 17);
            this.liteModeCheckBox.TabIndex = 225;
            this.liteModeCheckBox.Text = "&\"Lite\" mode (no additional processing done, only queue table is created)";
            this.liteModeCheckBox.UseVisualStyleBackColor = true;
            this.liteModeCheckBox.CheckedChanged += new System.EventHandler(this.liteModeCheckBox_CheckedChanged);
            // 
            // outputFileDialog
            // 
            this.liteModeOutputFileDialog.AutoEllipsis = true;
            this.liteModeOutputFileDialog.Enabled = false;
            this.liteModeOutputFileDialog.Location = new System.Drawing.Point(427, 165);
            this.liteModeOutputFileDialog.Margin = new System.Windows.Forms.Padding(2);
            this.liteModeOutputFileDialog.Name = "outputFileDialog";
            this.liteModeOutputFileDialog.Size = new System.Drawing.Size(16, 19);
            this.liteModeOutputFileDialog.TabIndex = 228;
            this.liteModeOutputFileDialog.Text = "...";
            this.liteModeOutputFileDialog.Click += new System.EventHandler(this.outputFileDialog_Click);
            // 
            // liteModeOutputTextBox
            // 
            this.liteModeOutputTextBox.Enabled = false;
            this.liteModeOutputTextBox.Location = new System.Drawing.Point(13, 164);
            this.liteModeOutputTextBox.Name = "liteModeOutputTextBox";
            this.liteModeOutputTextBox.Size = new System.Drawing.Size(409, 20);
            this.liteModeOutputTextBox.TabIndex = 227;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(11, 151);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(162, 19);
            this.label12.TabIndex = 226;
            this.label12.Text = "Lite mode &output file";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 600);
            this.Controls.Add(this.liteModeOutputFileDialog);
            this.Controls.Add(this.liteModeOutputTextBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.liteModeCheckBox);
            this.Controls.Add(this.resumeButton);
            this.Controls.Add(this.peoplePublicationsView);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.generateReports);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.inputFileDialog);
            this.Controls.Add(this.inputFileTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.logFilename);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.openInNotepad);
            this.Controls.Add(this.log);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.relatedTable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DSN);
            this.Controls.Add(this.ODBCPanel);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "FindRelated";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maximumLinkRanking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pubWindowLowerBound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pubWindowUpperBound)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox DSN;
        internal System.Windows.Forms.Button ODBCPanel;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox relatedTable;
        private System.Windows.Forms.TextBox logFilename;
        internal System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button openInNotepad;
        private System.Windows.Forms.ListBox log;
        internal System.Windows.Forms.Label label4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TextBox inputFileTextBox;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Button inputFileDialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox sameJournal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown pubWindowLowerBound;
        private System.Windows.Forms.CheckBox enableLowerBound;
        private System.Windows.Forms.NumericUpDown pubWindowUpperBound;
        private System.Windows.Forms.CheckBox enableUpperBound;
        private System.Windows.Forms.TextBox includeCategories;
        private System.Windows.Forms.NumericUpDown maximumLinkRanking;
        private System.Windows.Forms.CheckBox enableMaximumLinkRanking;
        private System.Windows.Forms.TextBox includeLanguages;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button generateReports;
        private System.Windows.Forms.TextBox peoplePublicationsView;
        internal System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button resumeButton;
        private System.Windows.Forms.CheckBox liteModeCheckBox;
        internal System.Windows.Forms.Button liteModeOutputFileDialog;
        private System.Windows.Forms.TextBox liteModeOutputTextBox;
        internal System.Windows.Forms.Label label12;
    }
}

