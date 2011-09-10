using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.StellmanGreene.PubMed;
using PubMed;

namespace Com.StellmanGreene.FindRelated
{
    public partial class ReportsDialog : Form
    {
        private readonly Database db;

        public ReportsDialog(Database db)
        {
            InitializeComponent();

            this.db = db;

            DataTable matchingTable = db.ExecuteQuery("SHOW TABLES LIKE 'colleaguepublications'");
            if (matchingTable.Rows.Count == 0)
            {
                doIdeaPeer.Checked = false;
                doIdeaPeer.Enabled = false;
                ideaPeerFilename.Enabled = false;
            }
        }

        private void generateReports_Click(object sender, EventArgs e)
        {
            RelatedReports relatedReports;
            try
            {
                string folder = string.IsNullOrEmpty(folderLabel.Text) ? Environment.CurrentDirectory : folderLabel.Text;
                relatedReports = new RelatedReports(db, folder);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (doLinking.Checked) relatedReports.Linking(linkingFilename.Text);
            if (doRelatedPmid.Checked) relatedReports.RelatedPMID(relatedPmidFilename.Text);
            if (doRelatedMesh.Checked) relatedReports.RelatedMeSH(relatedMeshFilename.Text);
            if (doIdeaPeer.Checked) relatedReports.IdeaPeer(ideaPeerFilename.Text);
        }

        private void specifyFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(folderLabel.Text))
                folderBrowserDialog1.SelectedPath = folderLabel.Text;
            else
                folderBrowserDialog1.SelectedPath = Environment.CurrentDirectory;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            folderLabel.Text = folderBrowserDialog1.SelectedPath;
            Settings.SetValue("ReportsDialog_Folder", folderLabel.Text);
        }

        private void ReportsDialog_Load(object sender, EventArgs e)
        {
            folderLabel.Text = Settings.GetValueString("ReportsDialog_Folder", Environment.CurrentDirectory);
        }
    }
}
