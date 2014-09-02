using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace LocalVersionControl
{
    public partial class LVControl : Form
    {
        private ProjectManager projectMan;
        private bool changed;
        private bool PathChanged;
        private bool excludeChanged;
        //private ListViewItem reSelect;
        projectSorter sorter;
        private ListViewItem selProject;
        private int updateing;
        // private Dictionary<long, string> currentExcludeLocations;

        /// <summary>
        /// Creates the main form
        /// </summary>
        public LVControl()
        {
            InitializeComponent();
            projectMan = new ProjectManager();
            //reSelect = null;
        }

        /// <summary>
        /// Handles the pressing of the browse button on theconfig tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {

            if (fbdFolderBrowser.ShowDialog() == DialogResult.OK)
                txtLocation.Text = fbdFolderBrowser.SelectedPath;
        }

        /// <summary>
        /// loads the projects from the database and inserts them into the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LVControl_Load(object sender, EventArgs e)
        {
            projectMan.loadProjects();
            foreach (Project pro in projectMan.Projects)
            {
                ListViewItem item = new ListViewItem(new string[] { pro.Name, pro.Time.ToString("u"), pro.LastDate.ToString("u") });
                item.Tag = pro;

                lsvProjectList.Items.Add(item);
                pro.updateProjectModDate += new EventHandler(updateProjectModDate);
                if (pro.AutoMonitor)
                {
                    pro.ModRecorder.updateChangeList += new EventHandler(ModRecorder_updateChangeList);
                    pro.WillMonitor = true;
                    updateing++;
                    pro.update();
                }
            }
            PathChanged = true;
            sorter = new projectSorter();
            lsvProjectList.ListViewItemSorter = sorter;
            if (lsvProjectList.Items.Count > 0)
            {
                    selectProject(lsvProjectList.Items[0]);
            }
        }

        /// <summary>
        /// Handles the add button by adding a new project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                if (!confirmSaveResults())
                {
                    return;
                }
            }
            foreach (ListViewItem selected in lsvProjectList.SelectedItems)
            {
                selected.Selected = false;
            }
            Project pro = projectMan.createProject();
            ListViewItem item = new ListViewItem(new string[] { "Project Name", pro.Time.ToString("u"), pro.LastDate.ToString("u") });
            // item.
            item.Tag = pro;
            tbcProject.SelectedTab = tabConfig;
            selectProject(item);
            txtName.Text = "Project Name";
            txtDescription.Text = "";
            txtLocation.Text = SettingsStore.Default.DefaultLocation + @"\" + txtName.Text;
            chkAutomatic.Checked = false;

            lsvProjectList.Items.Add(item);
            pro.updateProjectModDate += new EventHandler(updateProjectModDate);
            PathChanged = false;
            lsvProjectList.Refresh();
        }

        /// <summary>
        /// changes the selected project
        /// </summary>
        /// <param name="proItem"></param>
        private void selectProject(ListViewItem proItem)
        {
            PathChanged = true;
            foreach (ListViewItem selected in lsvProjectList.Items)
            {
                //selected.Selected = false;
                selected.BackColor = Color.Transparent;
            }
            selProject = proItem;
            selProject.BackColor = SystemColors.Highlight;
            selProject.Selected = true;

            Project pro = (Project)proItem.Tag;
            //if(!pro.NewEntry)
            //    pro.ModRecorder.updateChangeList -= new EventHandler(ModRecorder_updateChangeList);

            tbcProject.Visible = true;
            lblStart.Visible = false;
            if (pro.NewEntry)
            {
                txtLocation.Text = SettingsStore.Default.DefaultLocation;
                txtName.Tag = 1;
                txtDescription.Tag = 0;
                txtLocation.Tag = 0;
                txtNotes.Text = "";
            }
            else
            {
                lblNameValue.Text = pro.Name;
                txtName.Text = pro.Name;
                txtName.Tag = 0;
                chkAutomatic.Checked = pro.AutoMonitor;
                lblDescriptionValue.Text = pro.Description;
                txtDescription.Text = pro.Description;

                lblLocationValue.Text = pro.Location;
                txtLocation.Text = pro.Location;

                txtNotes.Text = pro.Note;

                lblDateValue.Text = pro.Time.ToString("F");
            }

            dgvExcludedFolders.CellValueChanged -= new DataGridViewCellEventHandler(dgvExcludedFolders_CellValueChanged);
            dgvExcludedFolders.Rows.Clear();
            if (pro.ExcludeLocations != null)
            {
                foreach (string path in pro.ExcludeLocations)
                {
                    int dgvrow = dgvExcludedFolders.Rows.Add();

                    dgvExcludedFolders.Rows[dgvrow].Cells[0].Value = path;
                    // dgvExcludedFolders.Rows.Add(dgvrow);
                }
            }
            dgvExcludedFolders.CellValueChanged += new DataGridViewCellEventHandler(dgvExcludedFolders_CellValueChanged);
            changed = false;
            excludeChanged = false;


            lsvChanges.Items.Clear();
            if (!pro.NewEntry)
            {
                try
                {
                    //pro.ModRecorder.updateChangeList += new EventHandler(ModRecorder_updateChangeList);
                    foreach (changeLogEntry entry in pro.ModRecorder.Log.Entrys.Reverse())
                    {
                        ListViewItem item = new ListViewItem(new string[] { entry.Date.ToString("u"), entry.Name, entry.Type.ToString() });
                        item.Tag = entry;
                        lsvChanges.Items.Add(item);
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to access or create projects folder. Check projects location is valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (pro.isMonitoring)
            {
                btnMonitoring.Text = "Stop Monitoring";
                lblStatus.Text = "Monitoring";
                btnUpdate.Enabled = true;
                btnMonitoring.Enabled = true;
                pro.WillMonitor = false;
            }
            else if (pro.WillMonitor)
            {
                btnUpdate.Enabled = false;
                btnMonitoring.Enabled = false;
                lblStatus.Text = "Updating before Starting to Monitor files for changes";
            }
            else if (pro.ModrecorderCreated && pro.ModRecorder.isUpdating)
            {
                lblStatus.Text = "Updating";
                btnUpdate.Enabled = false;
                btnMonitoring.Enabled = false;
            }
            else
            {
                lblStatus.Text = "";
                btnUpdate.Enabled = true;
                btnMonitoring.Enabled = true;
                btnMonitoring.Text = "Start Monitoring";
                lblStatus.Text = "";
            }


        }

        /// <summary>
        /// marks the config as changed so that it knows that the cahnges need saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void config_Changed(object sender, EventArgs e)
        {
            Control txt = (Control)sender;

            txt.Tag = 0;

            changed = true;
            if (txt == txtName && selProject != null)
            {
                selProject.Text = txt.Text;
                if (!PathChanged)
                {
                    txtLocation.Text = SettingsStore.Default.DefaultLocation + @"\" + makeValidFileName(txt.Text);
                }
            }
        }
        /// <summary>
        /// Removes invalid character from the string given
        /// </summary>
        /// <param name="input">Filename</param>
        /// <returns></returns>
        private string makeValidFileName(string input)
        {
            StringBuilder builder = new StringBuilder();
            char[] invalid = System.IO.Path.GetInvalidFileNameChars();
            foreach (var cur in input)
            {
                if (!invalid.Contains(cur))
                {
                    builder.Append(cur);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Handles the selection of the projects name textbox to select all characters when creating a new project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configText_Enter(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if ((int)txt.Tag == 1)
            {
                BeginInvoke((Action)delegate
                {
                    ((TextBox)sender).SelectAll();
                });
            }
        }

        /// <summary>
        /// saves the changes to the project
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        private bool saveResults(Project pro)
        {
            bool wasMonitoring = false;
            if (pro.Location != null && !pro.Location.Equals(txtLocation.Text))
            {
                if (pro.ModrecorderCreated && pro.ModRecorder.isUpdating)
                {
                    MessageBox.Show("Unable to Change Location. Update is in progress please wait for the update to finish", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLocation.Select();
                    return false;
                }
                else if (pro.isMonitoring)
                {
                    wasMonitoring = true;
                    pro.stopMonitoring();
                }

                try
                {
                    if (Directory.Exists(pro.Location))
                    {
                        if (Directory.GetDirectories(pro.Location).Length > 0 || Directory.GetFiles(pro.Location).Length > 0)
                        {
                            DialogResult res = MessageBox.Show("Do you want to move the contents of the previous location to the new location?", "Confim", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (res == DialogResult.Yes)
                            {
                                try
                                {
                                    copyDir(pro.Location, txtLocation.Text);
                                    Directory.Delete(pro.Location,true);
                                }
                                catch
                                {
                                    MessageBox.Show("Unable to Move Directory. Check location is valid and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtLocation.Select();
                                    return false;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Failed processing Directory check path is valid and try restarting", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (!PathChanged)
            {
                try
                {
                    Directory.CreateDirectory(txtLocation.Text);
                    PathChanged = true;
                }
                catch
                {
                    MessageBox.Show("Unable to create Directory. Check location is valid and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLocation.Select();
                    return false;
                }
            }
            else
            {
                if (!Directory.Exists(txtLocation.Text))
                {
                    DialogResult res = MessageBox.Show("Directory does not exist. Do you want to create the Directory?", "Confim", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (res == DialogResult.Yes)
                    {
                        try
                        {
                            Directory.CreateDirectory(txtLocation.Text);
                            PathChanged = true;
                        }
                        catch
                        {
                            MessageBox.Show("Unable to create Directory. Check location is valid and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtLocation.Select();
                            return false;
                        }

                    }
                    else
                    {
                        txtLocation.Select();
                        return false;
                    }
                }

            }

            pro.Name = txtName.Text;
            pro.Location = txtLocation.Text;
            pro.Description = txtDescription.Text;
            pro.Note = txtNotes.Text;
            pro.AutoMonitor = chkAutomatic.Checked;
            if (pro.NewEntry)
                pro.ModRecorder.updateChangeList += new EventHandler(ModRecorder_updateChangeList);

            if (excludeChanged)
            {
                List<string> excludeList = new List<string>();
                foreach (DataGridViewRow row in dgvExcludedFolders.Rows)
                {
                    if (!row.IsNewRow)
                        excludeList.Add(Convert.ToString(row.Cells[0].Value));
                }
                pro.ExcludeLocations = excludeList;
            }

            projectMan.updateProject(pro);
            changed = false;
            lsvProjectList.Sort();
            if (pro.ModrecorderCreated)
                pro.ModRecorder.reload(pro);
            if (wasMonitoring)
                pro.startMonitoring();
            return true;
        }

        /// <summary>
        /// shows a confirmation box before saving changes to project
        /// </summary>
        /// <returns></returns>
        private bool confirmSaveResults()
        {
            Project pro = (Project)selProject.Tag;
            string message = "Do you want to save the changes made to the projects settings?";
            if (pro.NewEntry)
                message = "Do you want to save the changes and create the project with the current settings? (Selecting no will remove the new project)";

            DialogResult res = MessageBox.Show(message, "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                if (!saveResults(pro))
                {
                    return false;
                }
            }
            else if (res == DialogResult.No)
            {
                if (pro.NewEntry)
                {
                    projectMan.removeProject(pro);
                    lsvProjectList.Items.Remove(selProject);
                    if (lsvProjectList.Items.Count > 0)
                    {
                        selectProject(lsvProjectList.Items[0]);
                    }
                    else
                    {
                        tbcProject.Visible = false;
                        lblStart.Visible = true;
                        changed = false;
                    }

                    // selProject = lsvProjectList.Items[0];
                    //selectProject(selProject);
                    //lsvProjectList.Refresh();
                }
                else
                {
                    selectProject(selProject);
                    selProject.Text = pro.Name;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// handles the changing of tab to allow the saving of settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbcProject_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (changed && selProject != null)
            {
                e.Cancel = !confirmSaveResults();
                if (!e.Cancel)
                {
                    selectProject(selProject);
                }
            }
        }

        /// <summary>
        /// Unselects the project to allow the applcation to keeps it own track of selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsvProjectList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
            }
        }

        /// <summary>
        /// handles the changes of the notes by saving the changes when editing the box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNotes_TextChanged(object sender, EventArgs e)
        {
            if (selProject != null)
            {
                Project pro = (Project)selProject.Tag;
                pro.Note = txtNotes.Text;
                projectMan.updateNote(pro);
            }
        }

        /// <summary>
        /// handles the selection of how to sort the project lsit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            sorter.SortColumn = cmbSort.SelectedIndex;
            if (sorter.SortColumn == 0)
            {
                sorter.Order = SortOrder.Ascending;
            }
            else
            {
                sorter.Order = SortOrder.Descending;
            }
            lsvProjectList.Sort();
        }

        /// <summary>
        /// applys the changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (selProject != null)
            {
                saveResults((Project)selProject.Tag);
                selectProject(selProject);
            }
        }

        /// <summary>
        /// Delete the given project
        /// </summary>
        /// <param name="item"></param>
        public void deleteProject(ListViewItem item)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to Delete this project? (This Cannot be undone. All records of changes will be removed)", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                projectMan.removeProject((Project)item.Tag);
                lsvProjectList.Items.Remove(item);
                if (lsvProjectList.Items.Count > 0)
                {
                    if (item == selProject)
                    {
                        selectProject(lsvProjectList.Items[0]);
                    }
                }
                else
                {
                    tbcProject.Visible = false;
                    lblStart.Visible = true;
                    changed = false;
                }
            }
        }

        /// <summary>
        ///  handles the click of the delete button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selProject != null)
            {
                deleteProject(selProject);
            }

        }

        /// <summary>
        /// handles any clicks that occur in the project list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsvProjectList_MouseClick(object sender, MouseEventArgs e)
        {
            if (lsvProjectList.FocusedItem.Bounds.Contains(e.Location) == true)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (((Project)lsvProjectList.FocusedItem.Tag).isMonitoring)
                    {
                        tsmMonitor.Checked = true;
                    }
                    else
                    {
                        tsmMonitor.Checked = false;
                    }
                    rcmProject.Show(Cursor.Position);

                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (changed)
                    {
                        if (confirmSaveResults())
                        {
                            selectProject(lsvProjectList.FocusedItem);
                        }
                    }
                    else
                    {
                        selectProject(lsvProjectList.FocusedItem);
                    }
                }
            }
        }

        /// <summary>
        /// handle the request to start Monitoring for changes fro mthe right click menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmMonitor_Click(object sender, EventArgs e)
        {
            if (lsvProjectList.FocusedItem != null)
            {
                if (selProject != null)
                {
                    Project pro = ((Project)lsvProjectList.FocusedItem.Tag);
                    if (pro.isMonitoring)
                    {
                        if (pro.stopMonitoring())
                        {
                            btnMonitoring.Text = "Start Monitoring";
                        }
                        else
                        {
                            MessageBox.Show("Error encountered while trying to stop Monitoring Directory. Try again and restart the application if it doesn't work", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        btnUpdate.Enabled = true;
                        lblStatus.Text = "";
                    }
                    else
                    {
                        pro.ModRecorder.updateChangeList += new EventHandler(ModRecorder_updateChangeList);
                        btnMonitoring.Text = "Stop Monitoring";
                        pro.WillMonitor = true;
                        btnUpdate.Enabled = false;
                        btnMonitoring.Enabled = false;
                        lblStatus.Text = "Updating before Starting to Monitor file for changes";
                        updateing++;
                        pro.update();
                    }
                }
            }
        }

        /// <summary>
        /// handles the right click event to delete the project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmDelete_Click(object sender, EventArgs e)
        {
            if (lsvProjectList.FocusedItem != null)
            {
                deleteProject(lsvProjectList.FocusedItem);
            }
        }

        /// <summary>
        /// handles the closing of the form by checking for backgroud taks and giveing the user the option to cancel them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LVControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (updateing > 0)
            {
                DialogResult res = MessageBox.Show("Background process in progress stopping is likely to damage recording of changes. Are you sure you want to continue?", "Closeing", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (res.Equals(DialogResult.No))
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (changed && selProject != null)
            {
                e.Cancel = !confirmSaveResults();
            }
        }

        /// <summary>
        /// handles the request to open the folder in explorer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(((Project)selProject.Tag).Location);
            }
            catch
            {
                MessageBox.Show("Unable to Open Directory. Check location path is valid and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// handle the request to start Monitoring for changes from the start tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonitoring_Click(object sender, EventArgs e)
        {
            if (selProject != null)
            {
                Project pro = ((Project)selProject.Tag);
                if (pro.isMonitoring)
                {
                    if (pro.stopMonitoring())
                    {
                        btnMonitoring.Text = "Start Monitoring";
                    }
                    else
                    {
                        MessageBox.Show("Error encountered while trying to stop Monitoring Directory. Try again and restart the application if it doesn't work", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    btnUpdate.Enabled = true;
                    lblStatus.Text = "";
                }
                else
                {
                    pro.ModRecorder.updateChangeList += new EventHandler(ModRecorder_updateChangeList);
                    btnMonitoring.Text = "Stop Monitoring";
                    pro.WillMonitor = true;
                    btnUpdate.Enabled = false;
                    btnMonitoring.Enabled = false;
                    lblStatus.Text = "Updating before Starting to Monitor file for changes";
                    updateing++;
                    pro.update();
                }
            }
        }

        /// <summary>
        /// handles the clcik inside the exclude
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExcludedFolders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                fbdFolderBrowser.SelectedPath = ((Project)selProject.Tag).Location;
                if (fbdFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    if (dgvExcludedFolders.Rows[e.RowIndex].IsNewRow)
                    {
                        dgvExcludedFolders.Rows.Add(1);
                        dgvExcludedFolders.Rows[e.RowIndex].Tag = -1;

                    }
                    dgvExcludedFolders.Rows[e.RowIndex].Cells[0].Value = fbdFolderBrowser.SelectedPath;
                    //dgvExcludedFolders.Rows[e.RowIndex].Cells[4].Value = true;
                    //changed = true;
                }
            }
            else if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                ofdFileBrowser.InitialDirectory = ((Project)selProject.Tag).Location;

                if (ofdFileBrowser.ShowDialog() == DialogResult.OK)
                {
                    DataGridViewCell rmCell = dgvExcludedFolders.Rows[e.RowIndex].Cells[3];
                    if (dgvExcludedFolders.Rows[e.RowIndex].IsNewRow)
                    {
                        dgvExcludedFolders.Rows.Add(1);
                        dgvExcludedFolders.Rows[e.RowIndex].Tag = -1;

                    }
                    dgvExcludedFolders.Rows[e.RowIndex].Cells[0].Value = ofdFileBrowser.FileName;
                    //dgvExcludedFolders.Rows[e.RowIndex].Cells[4].Value = true;
                    //changed = true;
                }
            }
            else if (e.ColumnIndex == 3 && e.RowIndex >= 0 && !dgvExcludedFolders.Rows[e.RowIndex].IsNewRow)
            {
                dgvExcludedFolders.Rows.RemoveAt(e.RowIndex);
                changed = true;
                excludeChanged = true;
            }

        }

        /// <summary>
        /// handles what happen when a change occurs on the exclude list to tell the application it needs saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExcludedFolders_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            changed = true;
            excludeChanged = true;
        }

        /// <summary>
        /// handles the press of the update button on the start tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ((Project)selProject.Tag).ModRecorder.updateChangeList += new EventHandler(ModRecorder_updateChangeList);
            lblStatus.Text = "Updating";
            btnUpdate.Enabled = false;
            btnMonitoring.Enabled = false;
            btnMonitoring.Text = "Start Monitoring";
            updateing++;
            ((Project)selProject.Tag).update();


        }

        /// <summary>
        /// handles the mosue click event on the changes list to show the right click menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsvChanges_MouseClick(object sender, MouseEventArgs e)
        {
            if (lsvChanges.FocusedItem.Bounds.Contains(e.Location) == true)
            {
                if (e.Button == MouseButtons.Right)
                {
                    rcmChanges.Show(Cursor.Position);
                }
            }
        }

        /// <summary>
        /// event to update the changes list
        /// </summary>
        /// <param name="sender">ModificationRecorder needing update</param>
        /// <param name="e">empty</param>
        private void ModRecorder_updateChangeList(object sender, EventArgs e)
        {
            if (this.lsvChanges.InvokeRequired)
            {
                if (((ModificationRecorder)sender).isUpdating)
                {
                    updateing--;
                }
                this.lsvChanges.BeginInvoke(new MethodInvoker(() => ModRecorder_updateChangeList(sender, e)));
            }
            else
            {

                ModificationRecorder proMod = (ModificationRecorder)sender;

                if (proMod.getProject.WillMonitor)
                {
                    btnMonitoring.Text = "Stop Monitoring";
                    if (!proMod.getProject.startMonitoring())
                    {
                        MessageBox.Show("Unable to Start Monitoring Directory. Check location path is valid and exists and then try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    proMod.getProject.WillMonitor = false;
                }
                if (proMod.getProject.Equals(((Project)selProject.Tag)))
                {
                    lsvChanges.Items.Clear();
                    foreach (changeLogEntry entry in proMod.Log.Entrys.Reverse())
                    {
                        ListViewItem item = new ListViewItem(new string[] { entry.Date.ToString("u"), entry.Name, entry.Type.ToString() });
                        item.Tag = entry;
                        lsvChanges.Items.Add(item);
                    }
                    if (proMod.getProject.isMonitoring)
                    {
                        btnMonitoring.Enabled = true;
                        btnUpdate.Enabled = true;
                        lblStatus.Text = "Monitoring";
                    }
                    else
                    {
                        btnMonitoring.Enabled = true;
                        btnUpdate.Enabled = true;
                        lblStatus.Text = "";
                    }

                }
            }
        }

        /// <summary>
        /// handles the request to restore
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmRestore_Click(object sender, EventArgs e)
        {
            if (lsvChanges.SelectedIndices.Count > 0)
            {
                restore((Project)selProject.Tag, lsvChanges.SelectedIndices[0]);
            }
        }

        /// <summary>
        /// restores the files to prevoius state
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="itemIndex"></param>
        private void restore(Project pro, int itemIndex)
        {
            if (lsvChanges.InvokeRequired)
            {
                lsvChanges.BeginInvoke(new MethodInvoker(() => restore(pro, itemIndex)));
            }
            else
            {
                selectProject(selProject);
                changeLogEntry lastEntry = (changeLogEntry)lsvChanges.Items[itemIndex].Tag;
                Thread restoreThread = new Thread(delegate()
                {
                    updateing++;
                    bool monitoringState = pro.isMonitoring;
                    if (monitoringState)
                        pro.stopMonitoring();
                    pro.ModRecorder.updateChangeList -= new EventHandler(ModRecorder_updateChangeList);

                    changeLogEntry lastRemoved = pro.ModRecorder.revertsChange();
                    while (lastRemoved != null && !lastRemoved.Equals(lastEntry))
                    {
                        lastRemoved = pro.ModRecorder.revertsChange();
                    }
                    ModRecorder_updateChangeList(pro.ModRecorder, EventArgs.Empty);
                    pro.ModRecorder.updateChangeList += new EventHandler(ModRecorder_updateChangeList);
                    if (monitoringState)
                        pro.startMonitoring();
                    updateing--;
                });
                restoreThread.Start();
            }
        }

        /// <summary>
        /// handles the button to reset the tracking of file changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to remove all tracking of changes? (This action cannot be undone)";


            DialogResult res = MessageBox.Show(message, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                Project pro = ((Project)selProject.Tag);
                pro.ModRecorder.removeTrack();
            }

        }


        /// <summary>
        /// displays the setting dialogue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettings_Click(object sender, EventArgs e)
        {
            SettingsDialog SetDialog = new SettingsDialog();
            SetDialog.Show();
        }

        /// <summary>
        /// handles the button press to make a clone from the start tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClone_Click(object sender, EventArgs e)
        {
            clone();
        }

        /// <summary>
        /// handles the button press to make a clone from right click menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clone();
        }

        /// <summary>
        ///cloning the selected project and restores to a given change
        /// </summary>
        /// <param name="restoreVersion"></param>
        private void clone(bool restoreVersion = false)
        {
            Project curPro = (Project)selProject.Tag;
            Project newPro = projectMan.createProject();
            newPro.Name = curPro.Name + "_" + DateTime.Now.ToString("yyyy-MM-d--HH-mm-ss");
            newPro.Location = Path.Combine(SettingsStore.Default.DefaultLocation, newPro.Name);
            newPro.Description = curPro.Description;
            newPro.Note = curPro.Note;
            newPro.AutoMonitor = curPro.AutoMonitor;
            newPro.ExcludeLocations = curPro.ExcludeLocations;
            projectMan.updateProject(newPro);
            ListViewItem lvItem = new ListViewItem(new string[] { newPro.Name, newPro.Time.ToString("u"), newPro.Time.ToString("u") });
            lvItem.Tag = newPro;
            selProject = lvItem;
            lsvProjectList.Items.Add(lvItem);
            newPro.updateProjectModDate += new EventHandler(updateProjectModDate);
            updateing++;
            int lastIndex = -1;
            if (lsvChanges.SelectedItems.Count > 0)
            {
                lastIndex = lsvChanges.SelectedIndices[0];
                //selectProject(lvItem);
                //lastEntry = (changeLogEntry)lsvChanges.Items[i].Tag;
            }

            Thread thr = new Thread(delegate()
            {
                copyDir(curPro.Location, newPro.Location);

                if (lastIndex >= 0 && restoreVersion)
                {
                    restore(newPro, lastIndex);
                }
                newPro.ModRecorder.reload(newPro);
                // ModRecorder_updateChangeList(newPro.ModRecorder, EventArgs.Empty);
                updateing--;
            });
            thr.IsBackground = true;
            thr.Start();
        }

        /// <summary>
        /// copys the given directory using recursion
        /// </summary>
        /// <param name="source">directory to copy</param>
        /// <param name="destination">directory to put copys</param>
        private void copyDir(string source, string destination)
        {
            if (!Directory.Exists(destination))
                Directory.CreateDirectory(destination);

            DirectoryInfo sourceInfo = new DirectoryInfo(source);

            // Process the list of files found in the directory.
            FileInfo[] fileEntries = sourceInfo.GetFiles();
            foreach (FileInfo fileName in fileEntries)
            {
                try
                {
                    File.Copy(fileName.FullName, Path.Combine(destination, fileName.Name), true);
                }
                catch
                {

                }
            }

            // Recurse into subdirectories of this directory.
            DirectoryInfo[] subdirEntries = sourceInfo.GetDirectories();
            foreach (DirectoryInfo subdir in subdirEntries)
            {
                copyDir(subdir.FullName, Path.Combine(destination, subdir.Name));
            }

        }

        /// <summary>
        /// handles the restore to clone button press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRestoreClone_Click(object sender, EventArgs e)
        {
            clone(true);
        }


        /// <summary>
        /// handles the restore to clone right click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restoreToCloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clone(true);
        }

        /// <summary>
        /// updates the modification data of a project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateProjectModDate(object sender, EventArgs e)
        {
            if (lsvProjectList.InvokeRequired)
            {
                lsvProjectList.BeginInvoke(new MethodInvoker(() => updateProjectModDate(sender, e)));
            }
            else
            {
                foreach (ListViewItem item in lsvProjectList.Items)
                {
                    item.SubItems[2].Text = ((Project)item.Tag).LastDate.ToString("u");
                }
            }
        }

    }
}
