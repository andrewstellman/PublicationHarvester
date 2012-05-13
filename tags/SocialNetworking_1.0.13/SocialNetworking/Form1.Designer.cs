namespace Com.StellmanGreene.SocialNetworking
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
            this.DSN = new System.Windows.Forms.ComboBox();
            this.ODBCPanel = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.ReportPathDialog = new System.Windows.Forms.Button();
            this.ReportPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FirstDegreeDB = new System.Windows.Forms.ComboBox();
            this.SecondDegreeDB = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.WriteReport = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ExcludeFilePathDialog = new System.Windows.Forms.Button();
            this.IncludeFilePath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DSN
            // 
            this.DSN.FormattingEnabled = true;
            this.DSN.Location = new System.Drawing.Point(15, 14);
            this.DSN.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DSN.Name = "DSN";
            this.DSN.Size = new System.Drawing.Size(547, 24);
            this.DSN.TabIndex = 20;
            this.DSN.SelectedIndexChanged += new System.EventHandler(this.DSN_SelectedIndexChanged);
            // 
            // ODBCPanel
            // 
            this.ODBCPanel.AutoEllipsis = true;
            this.ODBCPanel.Location = new System.Drawing.Point(568, 16);
            this.ODBCPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ODBCPanel.Name = "ODBCPanel";
            this.ODBCPanel.Size = new System.Drawing.Size(21, 23);
            this.ODBCPanel.TabIndex = 30;
            this.ODBCPanel.Text = "...";
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(12, -2);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(216, 23);
            this.Label2.TabIndex = 10;
            this.Label2.Text = "&ODBC Data Source Name";
            // 
            // ReportPathDialog
            // 
            this.ReportPathDialog.AutoEllipsis = true;
            this.ReportPathDialog.Location = new System.Drawing.Point(572, 108);
            this.ReportPathDialog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ReportPathDialog.Name = "ReportPathDialog";
            this.ReportPathDialog.Size = new System.Drawing.Size(21, 23);
            this.ReportPathDialog.TabIndex = 90;
            this.ReportPathDialog.Text = "...";
            this.ReportPathDialog.Click += new System.EventHandler(this.ReportPathDialog_Click);
            // 
            // ReportPath
            // 
            this.ReportPath.Location = new System.Drawing.Point(16, 107);
            this.ReportPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ReportPath.Multiline = true;
            this.ReportPath.Name = "ReportPath";
            this.ReportPath.Size = new System.Drawing.Size(549, 59);
            this.ReportPath.TabIndex = 85;
            this.ReportPath.TextChanged += new System.EventHandler(this.ReportPath_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 23);
            this.label1.TabIndex = 80;
            this.label1.Text = "&Report file to write";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(216, 23);
            this.label3.TabIndex = 60;
            this.label3.Text = "First-Degree &Database";
            // 
            // FirstDegreeDB
            // 
            this.FirstDegreeDB.Enabled = false;
            this.FirstDegreeDB.FormattingEnabled = true;
            this.FirstDegreeDB.Location = new System.Drawing.Point(16, 58);
            this.FirstDegreeDB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FirstDegreeDB.Name = "FirstDegreeDB";
            this.FirstDegreeDB.Size = new System.Drawing.Size(277, 24);
            this.FirstDegreeDB.TabIndex = 65;
            this.FirstDegreeDB.SelectedIndexChanged += new System.EventHandler(this.FirstDegreeDB_SelectedIndexChanged);
            this.FirstDegreeDB.TextChanged += new System.EventHandler(this.FirstDegreeDB_TextChanged);
            // 
            // SecondDegreeDB
            // 
            this.SecondDegreeDB.Enabled = false;
            this.SecondDegreeDB.FormattingEnabled = true;
            this.SecondDegreeDB.Location = new System.Drawing.Point(309, 58);
            this.SecondDegreeDB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SecondDegreeDB.Name = "SecondDegreeDB";
            this.SecondDegreeDB.Size = new System.Drawing.Size(277, 24);
            this.SecondDegreeDB.TabIndex = 75;
            this.SecondDegreeDB.SelectedIndexChanged += new System.EventHandler(this.SecondDegreeDB_SelectedIndexChanged);
            this.SecondDegreeDB.TextChanged += new System.EventHandler(this.SecondDegreeDB_TextChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(305, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(216, 23);
            this.label4.TabIndex = 70;
            this.label4.Text = "&Second-Degree Database";
            // 
            // WriteReport
            // 
            this.WriteReport.Enabled = false;
            this.WriteReport.Location = new System.Drawing.Point(15, 260);
            this.WriteReport.Margin = new System.Windows.Forms.Padding(4);
            this.WriteReport.Name = "WriteReport";
            this.WriteReport.Size = new System.Drawing.Size(168, 28);
            this.WriteReport.TabIndex = 110;
            this.WriteReport.Text = "Write the Report";
            this.WriteReport.UseVisualStyleBackColor = true;
            this.WriteReport.Click += new System.EventHandler(this.WriteReport_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 321);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(603, 23);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 111;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(55, 18);
            this.toolStripStatusLabel1.Text = "Version";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(528, 18);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 18);
            this.toolStripStatusLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ExcludeFilePathDialog
            // 
            this.ExcludeFilePathDialog.AutoEllipsis = true;
            this.ExcludeFilePathDialog.Location = new System.Drawing.Point(572, 196);
            this.ExcludeFilePathDialog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ExcludeFilePathDialog.Name = "ExcludeFilePathDialog";
            this.ExcludeFilePathDialog.Size = new System.Drawing.Size(21, 23);
            this.ExcludeFilePathDialog.TabIndex = 105;
            this.ExcludeFilePathDialog.Text = "...";
            this.ExcludeFilePathDialog.Click += new System.EventHandler(this.IncludeFilePathDialog_Click);
            // 
            // IncludeFilePath
            // 
            this.IncludeFilePath.Location = new System.Drawing.Point(16, 195);
            this.IncludeFilePath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.IncludeFilePath.Multiline = true;
            this.IncludeFilePath.Name = "IncludeFilePath";
            this.IncludeFilePath.Size = new System.Drawing.Size(549, 59);
            this.IncludeFilePath.TabIndex = 100;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(528, 23);
            this.label5.TabIndex = 95;
            this.label5.Text = "&File with list of colleagues to search for (leave empty to search for all collea" +
                "gues)";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // statusStrip2
            // 
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5});
            this.statusStrip2.Location = new System.Drawing.Point(0, 299);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(603, 22);
            this.statusStrip2.SizingGrip = false;
            this.statusStrip2.TabIndex = 115;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(486, 17);
            this.toolStripStatusLabel4.Spring = true;
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(0, 17);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 344);
            this.Controls.Add(this.statusStrip2);
            this.Controls.Add(this.ExcludeFilePathDialog);
            this.Controls.Add(this.IncludeFilePath);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.WriteReport);
            this.Controls.Add(this.SecondDegreeDB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.FirstDegreeDB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ReportPathDialog);
            this.Controls.Add(this.ReportPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DSN);
            this.Controls.Add(this.ODBCPanel);
            this.Controls.Add(this.Label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Social Networking Report";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox DSN;
        internal System.Windows.Forms.Button ODBCPanel;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button ReportPathDialog;
        private System.Windows.Forms.TextBox ReportPath;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox FirstDegreeDB;
        private System.Windows.Forms.ComboBox SecondDegreeDB;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button WriteReport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        internal System.Windows.Forms.Button ExcludeFilePathDialog;
        private System.Windows.Forms.TextBox IncludeFilePath;
        internal System.Windows.Forms.Label label5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
    }
}

