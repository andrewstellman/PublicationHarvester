namespace Com.StellmanGreene.FindRelated
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
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.generateReports = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.linkingFilename = new System.Windows.Forms.TextBox();
            this.relatedPmidFilename = new System.Windows.Forms.TextBox();
            this.relatedMeshFilename = new System.Windows.Forms.TextBox();
            this.doRelatedMesh = new System.Windows.Forms.CheckBox();
            this.doRelatedPmid = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.specifyFolder = new System.Windows.Forms.Button();
            this.folderLabel = new System.Windows.Forms.TextBox();
            this.doLinking = new System.Windows.Forms.CheckBox();
            this.ideaPeerFilename = new System.Windows.Forms.TextBox();
            this.doIdeaPeer = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mostRelevantFilename = new System.Windows.Forms.TextBox();
            this.mostRelevant = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(190, 18);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 273);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(428, 23);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 109;
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
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // generateReports
            // 
            this.generateReports.Location = new System.Drawing.Point(115, 239);
            this.generateReports.Margin = new System.Windows.Forms.Padding(2);
            this.generateReports.Name = "generateReports";
            this.generateReports.Size = new System.Drawing.Size(186, 23);
            this.generateReports.TabIndex = 103;
            this.generateReports.Text = "&Generate Reports";
            this.generateReports.UseVisualStyleBackColor = true;
            this.generateReports.Click += new System.EventHandler(this.generateReports_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 179);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 114;
            this.label2.Text = "Specify a &Folder to Write To";
            // 
            // linkingFilename
            // 
            this.linkingFilename.Location = new System.Drawing.Point(129, 20);
            this.linkingFilename.Margin = new System.Windows.Forms.Padding(2);
            this.linkingFilename.Name = "linkingFilename";
            this.linkingFilename.Size = new System.Drawing.Size(288, 20);
            this.linkingFilename.TabIndex = 106;
            this.linkingFilename.Text = "linking.csv";
            // 
            // relatedPmidFilename
            // 
            this.relatedPmidFilename.Location = new System.Drawing.Point(129, 44);
            this.relatedPmidFilename.Margin = new System.Windows.Forms.Padding(2);
            this.relatedPmidFilename.Name = "relatedPmidFilename";
            this.relatedPmidFilename.Size = new System.Drawing.Size(288, 20);
            this.relatedPmidFilename.TabIndex = 111;
            this.relatedPmidFilename.Text = "relatedpmid.csv";
            // 
            // relatedMeshFilename
            // 
            this.relatedMeshFilename.Location = new System.Drawing.Point(129, 68);
            this.relatedMeshFilename.Margin = new System.Windows.Forms.Padding(2);
            this.relatedMeshFilename.Name = "relatedMeshFilename";
            this.relatedMeshFilename.Size = new System.Drawing.Size(288, 20);
            this.relatedMeshFilename.TabIndex = 113;
            this.relatedMeshFilename.Text = "relatedmesh.csv";
            // 
            // doRelatedMesh
            // 
            this.doRelatedMesh.AutoSize = true;
            this.doRelatedMesh.Checked = true;
            this.doRelatedMesh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doRelatedMesh.Location = new System.Drawing.Point(11, 70);
            this.doRelatedMesh.Margin = new System.Windows.Forms.Padding(2);
            this.doRelatedMesh.Name = "doRelatedMesh";
            this.doRelatedMesh.Size = new System.Drawing.Size(96, 17);
            this.doRelatedMesh.TabIndex = 112;
            this.doRelatedMesh.Text = "Related &MeSH";
            this.doRelatedMesh.UseVisualStyleBackColor = true;
            // 
            // doRelatedPmid
            // 
            this.doRelatedPmid.AutoSize = true;
            this.doRelatedPmid.Checked = true;
            this.doRelatedPmid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doRelatedPmid.Location = new System.Drawing.Point(11, 46);
            this.doRelatedPmid.Margin = new System.Windows.Forms.Padding(2);
            this.doRelatedPmid.Name = "doRelatedPmid";
            this.doRelatedPmid.Size = new System.Drawing.Size(93, 17);
            this.doRelatedPmid.TabIndex = 110;
            this.doRelatedPmid.Text = "&Related PMID";
            this.doRelatedPmid.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 104;
            this.label1.Text = "Select Reports to Write";
            // 
            // specifyFolder
            // 
            this.specifyFolder.AutoEllipsis = true;
            this.specifyFolder.Location = new System.Drawing.Point(396, 209);
            this.specifyFolder.Margin = new System.Windows.Forms.Padding(2);
            this.specifyFolder.Name = "specifyFolder";
            this.specifyFolder.Size = new System.Drawing.Size(20, 20);
            this.specifyFolder.TabIndex = 115;
            this.specifyFolder.Text = "...";
            this.specifyFolder.UseVisualStyleBackColor = true;
            this.specifyFolder.Click += new System.EventHandler(this.specifyFolder_Click);
            // 
            // folderLabel
            // 
            this.folderLabel.Location = new System.Drawing.Point(11, 195);
            this.folderLabel.Margin = new System.Windows.Forms.Padding(2);
            this.folderLabel.Multiline = true;
            this.folderLabel.Name = "folderLabel";
            this.folderLabel.ReadOnly = true;
            this.folderLabel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.folderLabel.Size = new System.Drawing.Size(381, 34);
            this.folderLabel.TabIndex = 116;
            // 
            // doLinking
            // 
            this.doLinking.AutoSize = true;
            this.doLinking.Checked = true;
            this.doLinking.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doLinking.Location = new System.Drawing.Point(11, 22);
            this.doLinking.Margin = new System.Windows.Forms.Padding(2);
            this.doLinking.Name = "doLinking";
            this.doLinking.Size = new System.Drawing.Size(60, 17);
            this.doLinking.TabIndex = 105;
            this.doLinking.Text = "&Linking";
            this.doLinking.UseVisualStyleBackColor = true;
            // 
            // ideaPeerFilename
            // 
            this.ideaPeerFilename.Location = new System.Drawing.Point(129, 92);
            this.ideaPeerFilename.Margin = new System.Windows.Forms.Padding(2);
            this.ideaPeerFilename.Name = "ideaPeerFilename";
            this.ideaPeerFilename.Size = new System.Drawing.Size(288, 20);
            this.ideaPeerFilename.TabIndex = 118;
            this.ideaPeerFilename.Text = "ideapeer.csv";
            // 
            // doIdeaPeer
            // 
            this.doIdeaPeer.AutoSize = true;
            this.doIdeaPeer.Checked = true;
            this.doIdeaPeer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doIdeaPeer.Location = new System.Drawing.Point(11, 94);
            this.doIdeaPeer.Margin = new System.Windows.Forms.Padding(2);
            this.doIdeaPeer.Name = "doIdeaPeer";
            this.doIdeaPeer.Size = new System.Drawing.Size(71, 17);
            this.doIdeaPeer.TabIndex = 117;
            this.doIdeaPeer.Text = "&Idea peer";
            this.doIdeaPeer.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 114);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(348, 13);
            this.label3.TabIndex = 119;
            this.label3.Text = "(Idea peer report is only available after colleagues have been generated)";
            // 
            // mostRelevantFilename
            // 
            this.mostRelevantFilename.Location = new System.Drawing.Point(129, 138);
            this.mostRelevantFilename.Margin = new System.Windows.Forms.Padding(2);
            this.mostRelevantFilename.Name = "mostRelevantFilename";
            this.mostRelevantFilename.Size = new System.Drawing.Size(288, 20);
            this.mostRelevantFilename.TabIndex = 121;
            this.mostRelevantFilename.Text = "extremerelevance.csv";
            // 
            // mostRelevant
            // 
            this.mostRelevant.AutoSize = true;
            this.mostRelevant.Checked = true;
            this.mostRelevant.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mostRelevant.Location = new System.Drawing.Point(11, 140);
            this.mostRelevant.Margin = new System.Windows.Forms.Padding(2);
            this.mostRelevant.Name = "mostRelevant";
            this.mostRelevant.Size = new System.Drawing.Size(114, 17);
            this.mostRelevant.TabIndex = 120;
            this.mostRelevant.Text = "E&xtreme relevance";
            this.mostRelevant.UseVisualStyleBackColor = true;
            // 
            // ReportsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 296);
            this.Controls.Add(this.mostRelevantFilename);
            this.Controls.Add(this.mostRelevant);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ideaPeerFilename);
            this.Controls.Add(this.doIdeaPeer);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.generateReports);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkingFilename);
            this.Controls.Add(this.relatedPmidFilename);
            this.Controls.Add(this.relatedMeshFilename);
            this.Controls.Add(this.doRelatedMesh);
            this.Controls.Add(this.doRelatedPmid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.specifyFolder);
            this.Controls.Add(this.folderLabel);
            this.Controls.Add(this.doLinking);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportsDialog";
            this.Text = "ReportsForm";
            this.Load += new System.EventHandler(this.ReportsDialog_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button generateReports;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox linkingFilename;
        private System.Windows.Forms.TextBox relatedPmidFilename;
        private System.Windows.Forms.TextBox relatedMeshFilename;
        private System.Windows.Forms.CheckBox doRelatedMesh;
        private System.Windows.Forms.CheckBox doRelatedPmid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button specifyFolder;
        private System.Windows.Forms.TextBox folderLabel;
        private System.Windows.Forms.CheckBox doLinking;
        private System.Windows.Forms.TextBox ideaPeerFilename;
        private System.Windows.Forms.CheckBox doIdeaPeer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox mostRelevantFilename;
        private System.Windows.Forms.CheckBox mostRelevant;
    }
}