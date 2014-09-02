using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using redrgb.DB;
using System.IO;

namespace LocalVersionControl
{

    public partial class SettingsDialog : Form
    {
        /// <summary>
        /// creates the settigns dialog and loads the values of the settings into the fields
        /// </summary>
        public SettingsDialog()
        {
            InitializeComponent();
            txtDefaultLocation.Text = SettingsStore.Default.DefaultLocation;
            txtDefaultExclude.Text = SettingsStore.Default.DefaultExclude;
        }

        /// <summary>
        /// handles the browse button to select a folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (fdbDefaultLocation.ShowDialog() == DialogResult.OK)
                txtDefaultLocation.Text = fdbDefaultLocation.SelectedPath;
        }

        /// <summary>
        /// handles the close button to close the dialog without saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// handles the OK button to save the settings and close the dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (save())
                this.Close();
        }

        /// <summary>
        /// handles the apply button to save the settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            save();
        }

        /// <summary>
        /// save the settings
        /// </summary>
        /// <returns>succesfully saved</returns>
        private bool save()
        {

            try
            {
                Directory.CreateDirectory(txtDefaultLocation.Text);
                SettingsStore.Default.DefaultLocation = txtDefaultLocation.Text;

            }
            catch
            {
                MessageBox.Show("Unable to create Directory. Check location is valid and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SettingsStore.Default.DefaultExclude = txtDefaultExclude.Text.Replace(".", "");
            SettingsStore.Default.Save();

            return true;
        }
    }
}
