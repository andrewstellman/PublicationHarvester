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
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(15, 131);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 30;
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
            this.DSN.Click += new System.EventHandler(this.DSN_Click);
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
            this.relatedTable.Size = new System.Drawing.Size(263, 20);
            this.relatedTable.TabIndex = 15;
            this.relatedTable.Text = "relatedpublications";
            // 
            // logFilename
            // 
            this.logFilename.Location = new System.Drawing.Point(11, 286);
            this.logFilename.Margin = new System.Windows.Forms.Padding(2);
            this.logFilename.Name = "logFilename";
            this.logFilename.ReadOnly = true;
            this.logFilename.Size = new System.Drawing.Size(316, 20);
            this.logFilename.TabIndex = 199;
            this.logFilename.TabStop = false;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(11, 271);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 19);
            this.label5.TabIndex = 198;
            this.label5.Text = "Log file";
            // 
            // openInNotepad
            // 
            this.openInNotepad.Location = new System.Drawing.Point(332, 285);
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
            this.log.Location = new System.Drawing.Point(11, 326);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(426, 134);
            this.log.TabIndex = 220;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 311);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 19);
            this.label4.TabIndex = 210;
            this.label4.Text = "&Log";
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
            this.cancelButton.Location = new System.Drawing.Point(101, 131);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 40;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 463);
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
            this.inputFileTextBox.Text = "findrelated_input.csv";
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 485);
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
    }
}

