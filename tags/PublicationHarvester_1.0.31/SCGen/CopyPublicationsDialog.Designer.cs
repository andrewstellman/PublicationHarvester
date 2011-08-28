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
            this.SuspendLayout();
            // 
            // Database
            // 
            this.Database.FormattingEnabled = true;
            this.Database.Location = new System.Drawing.Point(12, 29);
            this.Database.Name = "Database";
            this.Database.Size = new System.Drawing.Size(268, 24);
            this.Database.TabIndex = 0;
            this.Database.SelectedIndexChanged += new System.EventHandler(this.Database_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a database to copy from";
            // 
            // DoCopyPublications
            // 
            this.DoCopyPublications.Location = new System.Drawing.Point(12, 59);
            this.DoCopyPublications.Name = "DoCopyPublications";
            this.DoCopyPublications.Size = new System.Drawing.Size(268, 29);
            this.DoCopyPublications.TabIndex = 2;
            this.DoCopyPublications.Text = "Copy Publications";
            this.DoCopyPublications.UseVisualStyleBackColor = true;
            this.DoCopyPublications.Click += new System.EventHandler(this.DoCopyPublications_Click);
            // 
            // CopyPublicationsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 103);
            this.Controls.Add(this.DoCopyPublications);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Database);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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

    }
}