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
            this.stopButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.inputFileTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.inputFileDialogButton = new System.Windows.Forms.Button();
            this.resumeButton = new System.Windows.Forms.Button();
            this.outputFileDialogButton = new System.Windows.Forms.Button();
            this.outputFileTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.apiKeyFileButton = new System.Windows.Forms.Button();
            this.apiKeyFileTextBox = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(19, 362);
            this.startButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(125, 35);
            this.startButton.TabIndex = 80;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // DSN
            // 
            this.DSN.FormattingEnabled = true;
            this.DSN.Location = new System.Drawing.Point(19, 34);
            this.DSN.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DSN.Name = "DSN";
            this.DSN.Size = new System.Drawing.Size(612, 28);
            this.DSN.TabIndex = 5;
            this.DSN.SelectedIndexChanged += new System.EventHandler(this.DSN_SelectedIndexChanged);
            this.DSN.Click += new System.EventHandler(this.DSN_Click);
            this.DSN.Leave += new System.EventHandler(this.DSN_Leave);
            // 
            // ODBCPanel
            // 
            this.ODBCPanel.AutoEllipsis = true;
            this.ODBCPanel.Location = new System.Drawing.Point(642, 35);
            this.ODBCPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ODBCPanel.Name = "ODBCPanel";
            this.ODBCPanel.Size = new System.Drawing.Size(27, 29);
            this.ODBCPanel.TabIndex = 7;
            this.ODBCPanel.Text = "…";
            this.ODBCPanel.Click += new System.EventHandler(this.ODBCPanel_Click);
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(19, 11);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(243, 29);
            this.Label2.TabIndex = 131;
            this.Label2.Text = "&ODBC Data Source Name";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 29);
            this.label1.TabIndex = 10;
            this.label1.Text = "&Related publications table";
            // 
            // relatedTable
            // 
            this.relatedTable.Location = new System.Drawing.Point(19, 102);
            this.relatedTable.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.relatedTable.Name = "relatedTable";
            this.relatedTable.Size = new System.Drawing.Size(612, 26);
            this.relatedTable.TabIndex = 15;
            this.relatedTable.TextChanged += new System.EventHandler(this.relatedTable_TextChanged);
            // 
            // logFilename
            // 
            this.logFilename.Location = new System.Drawing.Point(19, 445);
            this.logFilename.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.logFilename.Name = "logFilename";
            this.logFilename.ReadOnly = true;
            this.logFilename.Size = new System.Drawing.Size(472, 26);
            this.logFilename.TabIndex = 199;
            this.logFilename.TabStop = false;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(19, 421);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(243, 29);
            this.label5.TabIndex = 198;
            this.label5.Text = "Log file";
            // 
            // openInNotepad
            // 
            this.openInNotepad.Location = new System.Drawing.Point(498, 442);
            this.openInNotepad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.openInNotepad.Name = "openInNotepad";
            this.openInNotepad.Size = new System.Drawing.Size(158, 31);
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
            this.log.ItemHeight = 20;
            this.log.Location = new System.Drawing.Point(19, 506);
            this.log.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(637, 204);
            this.log.TabIndex = 220;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 482);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(243, 29);
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
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(159, 362);
            this.stopButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(125, 35);
            this.stopButton.TabIndex = 90;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 732);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(674, 32);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 142;
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
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(179, 25);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // inputFileTextBox
            // 
            this.inputFileTextBox.Location = new System.Drawing.Point(19, 171);
            this.inputFileTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.inputFileTextBox.Name = "inputFileTextBox";
            this.inputFileTextBox.Size = new System.Drawing.Size(612, 26);
            this.inputFileTextBox.TabIndex = 25;
            this.inputFileTextBox.TextChanged += new System.EventHandler(this.InputFileTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(243, 29);
            this.label3.TabIndex = 20;
            this.label3.Text = "&Input file";
            // 
            // inputFileDialogButton
            // 
            this.inputFileDialogButton.AutoEllipsis = true;
            this.inputFileDialogButton.Location = new System.Drawing.Point(642, 172);
            this.inputFileDialogButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.inputFileDialogButton.Name = "inputFileDialogButton";
            this.inputFileDialogButton.Size = new System.Drawing.Size(27, 29);
            this.inputFileDialogButton.TabIndex = 27;
            this.inputFileDialogButton.Text = "…";
            this.inputFileDialogButton.Click += new System.EventHandler(this.inputFileDialog_Click);
            // 
            // resumeButton
            // 
            this.resumeButton.Enabled = false;
            this.resumeButton.Location = new System.Drawing.Point(298, 362);
            this.resumeButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.resumeButton.Name = "resumeButton";
            this.resumeButton.Size = new System.Drawing.Size(125, 35);
            this.resumeButton.TabIndex = 95;
            this.resumeButton.Text = "Resume";
            this.resumeButton.UseVisualStyleBackColor = true;
            this.resumeButton.Click += new System.EventHandler(this.resumeButton_Click);
            // 
            // outputFileDialogButton
            // 
            this.outputFileDialogButton.AutoEllipsis = true;
            this.outputFileDialogButton.Location = new System.Drawing.Point(642, 239);
            this.outputFileDialogButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.outputFileDialogButton.Name = "outputFileDialogButton";
            this.outputFileDialogButton.Size = new System.Drawing.Size(27, 29);
            this.outputFileDialogButton.TabIndex = 38;
            this.outputFileDialogButton.Text = "…";
            this.outputFileDialogButton.Click += new System.EventHandler(this.outputFileDialog_Click);
            // 
            // outputFileTextBox
            // 
            this.outputFileTextBox.Location = new System.Drawing.Point(19, 238);
            this.outputFileTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.outputFileTextBox.Name = "outputFileTextBox";
            this.outputFileTextBox.Size = new System.Drawing.Size(612, 26);
            this.outputFileTextBox.TabIndex = 34;
            this.outputFileTextBox.TextChanged += new System.EventHandler(this.OutputFileTextBox_TextChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(19, 215);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(243, 29);
            this.label12.TabIndex = 30;
            this.label12.Text = "&Output file";
            // 
            // apiKeyFileButton
            // 
            this.apiKeyFileButton.AutoEllipsis = true;
            this.apiKeyFileButton.Location = new System.Drawing.Point(645, 310);
            this.apiKeyFileButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.apiKeyFileButton.Name = "apiKeyFileButton";
            this.apiKeyFileButton.Size = new System.Drawing.Size(24, 29);
            this.apiKeyFileButton.TabIndex = 60;
            this.apiKeyFileButton.Text = "...";
            this.apiKeyFileButton.Click += new System.EventHandler(this.ApiKeyFileButton_Click);
            // 
            // apiKeyFileTextBox
            // 
            this.apiKeyFileTextBox.Location = new System.Drawing.Point(19, 310);
            this.apiKeyFileTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.apiKeyFileTextBox.Name = "apiKeyFileTextBox";
            this.apiKeyFileTextBox.Size = new System.Drawing.Size(612, 26);
            this.apiKeyFileTextBox.TabIndex = 55;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(19, 287);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(243, 29);
            this.label20.TabIndex = 50;
            this.label20.Text = "API &key file";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 764);
            this.Controls.Add(this.apiKeyFileButton);
            this.Controls.Add(this.apiKeyFileTextBox);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.outputFileDialogButton);
            this.Controls.Add(this.outputFileTextBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.resumeButton);
            this.Controls.Add(this.inputFileDialogButton);
            this.Controls.Add(this.inputFileTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.stopButton);
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
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TextBox inputFileTextBox;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Button inputFileDialogButton;
        private System.Windows.Forms.Button resumeButton;
        internal System.Windows.Forms.Button outputFileDialogButton;
        private System.Windows.Forms.TextBox outputFileTextBox;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.Button apiKeyFileButton;
        private System.Windows.Forms.TextBox apiKeyFileTextBox;
        internal System.Windows.Forms.Label label20;
    }
}

