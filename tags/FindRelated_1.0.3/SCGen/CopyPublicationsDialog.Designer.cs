namespace SCGen
{
    partial class CopyPublicationsDialog
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
            this.Database = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DoCopyPublications = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.colleaguePublicationsTable = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Database
            // 
            this.Database.FormattingEnabled = true;
            this.Database.Location = new System.Drawing.Point(9, 24);
            this.Database.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Database.Name = "Database";
            this.Database.Size = new System.Drawing.Size(202, 21);
            this.Database.TabIndex = 2;
            this.Database.SelectedIndexChanged += new System.EventHandler(this.Database_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a database to copy from";
            // 
            // DoCopyPublications
            // 
            this.DoCopyPublications.Location = new System.Drawing.Point(11, 103);
            this.DoCopyPublications.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DoCopyPublications.Name = "DoCopyPublications";
            this.DoCopyPublications.Size = new System.Drawing.Size(197, 24);
            this.DoCopyPublications.TabIndex = 5;
            this.DoCopyPublications.Text = "Copy Publications";
            this.DoCopyPublications.UseVisualStyleBackColor = true;
            this.DoCopyPublications.Click += new System.EventHandler(this.DoCopyPublications_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 55);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Collague publications table";
            // 
            // colleaguePublicationsTable
            // 
            this.colleaguePublicationsTable.Location = new System.Drawing.Point(9, 71);
            this.colleaguePublicationsTable.Name = "colleaguePublicationsTable";
            this.colleaguePublicationsTable.Size = new System.Drawing.Size(202, 20);
            this.colleaguePublicationsTable.TabIndex = 4;
            this.colleaguePublicationsTable.Text = "ColleaguePublications";
            // 
            // CopyPublicationsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 138);
            this.Controls.Add(this.colleaguePublicationsTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DoCopyPublications);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Database);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyPublicationsDialog";
            this.Text = "Copy Colleague Publications";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Database;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DoCopyPublications;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox colleaguePublicationsTable;

    }
}