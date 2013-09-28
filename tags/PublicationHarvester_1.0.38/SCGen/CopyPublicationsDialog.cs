using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.StellmanGreene.PubMed;

namespace SCGen
{
    public partial class CopyPublicationsDialog : Form
    {
        public Form1 ParentFormObject;
        public Database DB;
        string PublicationTypes;

        public CopyPublicationsDialog(Form1 form1, Database database, string publicationTypes)
        {
            ParentFormObject = form1;
            DB = database;
            PublicationTypes = publicationTypes;

            InitializeComponent();

            Database.Items.Clear();
            DataTable Results = DB.ExecuteQuery("show databases");
            foreach (DataRow Row in Results.Rows)
            {
                Database.Items.Add(Row[0].ToString());
            }

            DoCopyPublications.Enabled = (Database.Text.ToString() != "");
        }

        private void DoCopyPublications_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                ParentFormObject.AddLogEntry("Copying publications from database '" + Database.Text + "'");
                CopyPublications.DoCopy(DB, Database.Text, this.PublicationTypes);
                ParentFormObject.AddLogEntry("Finished copying publications");
            }
            catch (Exception ex)
            {
                ParentFormObject.AddLogEntry("Error copying publications: " + ex.Message);
            }
            ParentFormObject.UpdateStatus();
            this.Cursor = Cursors.Default;
            this.Enabled = true;
        }

        private void Database_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoCopyPublications.Enabled = (Database.Text.ToString() != "");
        }
    }
}