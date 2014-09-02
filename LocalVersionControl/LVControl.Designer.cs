namespace LocalVersionControl
{
    partial class LVControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LVControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lsvProjectList = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.cmbSort = new System.Windows.Forms.ComboBox();
            this.lblStart = new System.Windows.Forms.Label();
            this.tbcProject = new System.Windows.Forms.TabControl();
            this.tabStart = new System.Windows.Forms.TabPage();
            this.btnClone = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.lblDateValue = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnMonitoring = new System.Windows.Forms.Button();
            this.lblDescriptionValue = new System.Windows.Forms.Label();
            this.lblLocationValue = new System.Windows.Forms.Label();
            this.lblNameValue = new System.Windows.Forms.Label();
            this.lblStartLocation = new System.Windows.Forms.Label();
            this.lblStartDescription = new System.Windows.Forms.Label();
            this.lblStartProjectName = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.tabChanges = new System.Windows.Forms.TabPage();
            this.lsvChanges = new System.Windows.Forms.ListView();
            this.colDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSetting = new System.Windows.Forms.ToolStripButton();
            this.tsbRestoreClone = new System.Windows.Forms.ToolStripButton();
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.btnReset = new System.Windows.Forms.Button();
            this.dgvExcludedFolders = new System.Windows.Forms.DataGridView();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFolder = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colFile = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lblOpt = new System.Windows.Forms.Label();
            this.chkAutomatic = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblProjectDescription = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.fbdFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.rcmProject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cloneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdFileBrowser = new System.Windows.Forms.OpenFileDialog();
            this.rcmChanges = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreToCloneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblExcludeList = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tbcProject.SuspendLayout();
            this.tabStart.SuspendLayout();
            this.tabChanges.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExcludedFolders)).BeginInit();
            this.rcmProject.SuspendLayout();
            this.rcmChanges.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lsvProjectList);
            this.splitContainer1.Panel1.Controls.Add(this.btnAdd);
            this.splitContainer1.Panel1.Controls.Add(this.btnSettings);
            this.splitContainer1.Panel1.Controls.Add(this.cmbSort);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblStart);
            this.splitContainer1.Panel2.Controls.Add(this.tbcProject);
            this.splitContainer1.Size = new System.Drawing.Size(747, 528);
            this.splitContainer1.SplitterDistance = 151;
            this.splitContainer1.TabIndex = 0;
            // 
            // lsvProjectList
            // 
            this.lsvProjectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvProjectList.FullRowSelect = true;
            this.lsvProjectList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvProjectList.Location = new System.Drawing.Point(0, 21);
            this.lsvProjectList.MultiSelect = false;
            this.lsvProjectList.Name = "lsvProjectList";
            this.lsvProjectList.Size = new System.Drawing.Size(151, 461);
            this.lsvProjectList.TabIndex = 1;
            this.lsvProjectList.UseCompatibleStateImageBehavior = false;
            this.lsvProjectList.View = System.Windows.Forms.View.List;
            this.lsvProjectList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lsvProjectList_ItemSelectionChanged);
            this.lsvProjectList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lsvProjectList_MouseClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAdd.Location = new System.Drawing.Point(0, 482);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(151, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSettings.Location = new System.Drawing.Point(0, 505);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(151, 23);
            this.btnSettings.TabIndex = 1;
            this.btnSettings.Text = "Setting";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // cmbSort
            // 
            this.cmbSort.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbSort.FormattingEnabled = true;
            this.cmbSort.Items.AddRange(new object[] {
            "Name",
            "Creation Date",
            "Last update"});
            this.cmbSort.Location = new System.Drawing.Point(0, 0);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(151, 21);
            this.cmbSort.TabIndex = 2;
            this.cmbSort.Text = "Name";
            this.cmbSort.SelectedIndexChanged += new System.EventHandler(this.cmbSort_SelectedIndexChanged);
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(198, 244);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(236, 52);
            this.lblStart.TabIndex = 19;
            this.lblStart.Text = "There are no current projects\r\n\r\n\r\nCreate a project by clicking Add on the left p" +
    "anel";
            // 
            // tbcProject
            // 
            this.tbcProject.Controls.Add(this.tabStart);
            this.tbcProject.Controls.Add(this.tabChanges);
            this.tbcProject.Controls.Add(this.tabConfig);
            this.tbcProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcProject.HotTrack = true;
            this.tbcProject.Location = new System.Drawing.Point(0, 0);
            this.tbcProject.Name = "tbcProject";
            this.tbcProject.SelectedIndex = 0;
            this.tbcProject.Size = new System.Drawing.Size(592, 528);
            this.tbcProject.TabIndex = 1;
            this.tbcProject.Visible = false;
            this.tbcProject.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tbcProject_Deselecting);
            // 
            // tabStart
            // 
            this.tabStart.Controls.Add(this.btnClone);
            this.tabStart.Controls.Add(this.lblStatus);
            this.tabStart.Controls.Add(this.btnOpenFolder);
            this.tabStart.Controls.Add(this.lblDateValue);
            this.tabStart.Controls.Add(this.lblDate);
            this.tabStart.Controls.Add(this.btnUpdate);
            this.tabStart.Controls.Add(this.btnMonitoring);
            this.tabStart.Controls.Add(this.lblDescriptionValue);
            this.tabStart.Controls.Add(this.lblLocationValue);
            this.tabStart.Controls.Add(this.lblNameValue);
            this.tabStart.Controls.Add(this.lblStartLocation);
            this.tabStart.Controls.Add(this.lblStartDescription);
            this.tabStart.Controls.Add(this.lblStartProjectName);
            this.tabStart.Controls.Add(this.txtNotes);
            this.tabStart.Controls.Add(this.lblNotes);
            this.tabStart.Location = new System.Drawing.Point(4, 22);
            this.tabStart.Name = "tabStart";
            this.tabStart.Padding = new System.Windows.Forms.Padding(3);
            this.tabStart.Size = new System.Drawing.Size(584, 502);
            this.tabStart.TabIndex = 3;
            this.tabStart.Text = "Start";
            this.tabStart.UseVisualStyleBackColor = true;
            // 
            // btnClone
            // 
            this.btnClone.Location = new System.Drawing.Point(37, 337);
            this.btnClone.Name = "btnClone";
            this.btnClone.Size = new System.Drawing.Size(143, 23);
            this.btnClone.TabIndex = 21;
            this.btnClone.Text = "Make Clone";
            this.btnClone.UseVisualStyleBackColor = true;
            this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(239, 270);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 20;
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Location = new System.Drawing.Point(37, 297);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(143, 23);
            this.btnOpenFolder.TabIndex = 19;
            this.btnOpenFolder.Text = "Open Folder";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // lblDateValue
            // 
            this.lblDateValue.AutoSize = true;
            this.lblDateValue.Location = new System.Drawing.Point(119, 82);
            this.lblDateValue.Name = "lblDateValue";
            this.lblDateValue.Size = new System.Drawing.Size(0, 13);
            this.lblDateValue.TabIndex = 18;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(7, 82);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(73, 13);
            this.lblDate.TabIndex = 17;
            this.lblDate.Text = "Date Created:";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(37, 257);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(143, 23);
            this.btnUpdate.TabIndex = 16;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnMonitoring
            // 
            this.btnMonitoring.Location = new System.Drawing.Point(37, 217);
            this.btnMonitoring.Name = "btnMonitoring";
            this.btnMonitoring.Size = new System.Drawing.Size(143, 23);
            this.btnMonitoring.TabIndex = 15;
            this.btnMonitoring.Text = "Start Monitoring";
            this.btnMonitoring.UseVisualStyleBackColor = true;
            this.btnMonitoring.Click += new System.EventHandler(this.btnMonitoring_Click);
            // 
            // lblDescriptionValue
            // 
            this.lblDescriptionValue.AutoSize = true;
            this.lblDescriptionValue.Location = new System.Drawing.Point(119, 115);
            this.lblDescriptionValue.Name = "lblDescriptionValue";
            this.lblDescriptionValue.Size = new System.Drawing.Size(35, 13);
            this.lblDescriptionValue.TabIndex = 14;
            this.lblDescriptionValue.Text = "label6";
            // 
            // lblLocationValue
            // 
            this.lblLocationValue.AutoSize = true;
            this.lblLocationValue.Location = new System.Drawing.Point(119, 49);
            this.lblLocationValue.Name = "lblLocationValue";
            this.lblLocationValue.Size = new System.Drawing.Size(35, 13);
            this.lblLocationValue.TabIndex = 13;
            this.lblLocationValue.Text = "label5";
            // 
            // lblNameValue
            // 
            this.lblNameValue.AutoSize = true;
            this.lblNameValue.Location = new System.Drawing.Point(119, 16);
            this.lblNameValue.Name = "lblNameValue";
            this.lblNameValue.Size = new System.Drawing.Size(35, 13);
            this.lblNameValue.TabIndex = 12;
            this.lblNameValue.Text = "label4";
            // 
            // lblStartLocation
            // 
            this.lblStartLocation.AutoSize = true;
            this.lblStartLocation.Location = new System.Drawing.Point(7, 49);
            this.lblStartLocation.Name = "lblStartLocation";
            this.lblStartLocation.Size = new System.Drawing.Size(51, 13);
            this.lblStartLocation.TabIndex = 11;
            this.lblStartLocation.Text = "Location:";
            // 
            // lblStartDescription
            // 
            this.lblStartDescription.AutoSize = true;
            this.lblStartDescription.Location = new System.Drawing.Point(7, 115);
            this.lblStartDescription.Name = "lblStartDescription";
            this.lblStartDescription.Size = new System.Drawing.Size(99, 13);
            this.lblStartDescription.TabIndex = 10;
            this.lblStartDescription.Text = "Project Description:";
            // 
            // lblStartProjectName
            // 
            this.lblStartProjectName.AutoSize = true;
            this.lblStartProjectName.Location = new System.Drawing.Point(7, 16);
            this.lblStartProjectName.Name = "lblStartProjectName";
            this.lblStartProjectName.Size = new System.Drawing.Size(74, 13);
            this.lblStartProjectName.TabIndex = 9;
            this.lblStartProjectName.Text = "Project Name:";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(110, 382);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(466, 87);
            this.txtNotes.TabIndex = 8;
            this.txtNotes.TextChanged += new System.EventHandler(this.txtNotes_TextChanged);
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(7, 390);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(38, 13);
            this.lblNotes.TabIndex = 7;
            this.lblNotes.Text = "Notes:";
            // 
            // tabChanges
            // 
            this.tabChanges.Controls.Add(this.lsvChanges);
            this.tabChanges.Controls.Add(this.toolStrip1);
            this.tabChanges.Location = new System.Drawing.Point(4, 22);
            this.tabChanges.Name = "tabChanges";
            this.tabChanges.Size = new System.Drawing.Size(584, 502);
            this.tabChanges.TabIndex = 2;
            this.tabChanges.Text = "Changes";
            this.tabChanges.UseVisualStyleBackColor = true;
            // 
            // lsvChanges
            // 
            this.lsvChanges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDate,
            this.colName,
            this.colType});
            this.lsvChanges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvChanges.FullRowSelect = true;
            this.lsvChanges.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvChanges.Location = new System.Drawing.Point(0, 25);
            this.lsvChanges.Name = "lsvChanges";
            this.lsvChanges.Size = new System.Drawing.Size(584, 477);
            this.lsvChanges.TabIndex = 0;
            this.lsvChanges.UseCompatibleStateImageBehavior = false;
            this.lsvChanges.View = System.Windows.Forms.View.Details;
            this.lsvChanges.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lsvChanges_MouseClick);
            // 
            // colDate
            // 
            this.colDate.Text = "Date";
            this.colDate.Width = 120;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 300;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 163;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSetting,
            this.tsbRestoreClone});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(584, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSetting
            // 
            this.btnSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnSetting.Image")));
            this.btnSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(50, 22);
            this.btnSetting.Text = "Restore";
            this.btnSetting.Click += new System.EventHandler(this.tsmRestore_Click);
            // 
            // tsbRestoreClone
            // 
            this.tsbRestoreClone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRestoreClone.Image = ((System.Drawing.Image)(resources.GetObject("tsbRestoreClone.Image")));
            this.tsbRestoreClone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRestoreClone.Name = "tsbRestoreClone";
            this.tsbRestoreClone.Size = new System.Drawing.Size(96, 22);
            this.tsbRestoreClone.Text = "Restore to clone";
            this.tsbRestoreClone.Click += new System.EventHandler(this.tsbRestoreClone_Click);
            // 
            // tabConfig
            // 
            this.tabConfig.Controls.Add(this.lblExcludeList);
            this.tabConfig.Controls.Add(this.btnReset);
            this.tabConfig.Controls.Add(this.dgvExcludedFolders);
            this.tabConfig.Controls.Add(this.lblOpt);
            this.tabConfig.Controls.Add(this.chkAutomatic);
            this.tabConfig.Controls.Add(this.btnDelete);
            this.tabConfig.Controls.Add(this.btnApply);
            this.tabConfig.Controls.Add(this.btnBrowse);
            this.tabConfig.Controls.Add(this.txtLocation);
            this.tabConfig.Controls.Add(this.txtDescription);
            this.tabConfig.Controls.Add(this.txtName);
            this.tabConfig.Controls.Add(this.lblLocation);
            this.tabConfig.Controls.Add(this.lblProjectDescription);
            this.tabConfig.Controls.Add(this.lblProjectName);
            this.tabConfig.Location = new System.Drawing.Point(4, 22);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfig.Size = new System.Drawing.Size(584, 502);
            this.tabConfig.TabIndex = 0;
            this.tabConfig.Text = "Config";
            this.tabConfig.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(215, 441);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 15;
            this.btnReset.Text = "Reset Logs";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // dgvExcludedFolders
            // 
            this.dgvExcludedFolders.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvExcludedFolders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExcludedFolders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPath,
            this.colFolder,
            this.colFile,
            this.colRemove});
            this.dgvExcludedFolders.Location = new System.Drawing.Point(97, 200);
            this.dgvExcludedFolders.Name = "dgvExcludedFolders";
            this.dgvExcludedFolders.RowHeadersVisible = false;
            this.dgvExcludedFolders.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvExcludedFolders.Size = new System.Drawing.Size(479, 150);
            this.dgvExcludedFolders.TabIndex = 14;
            this.dgvExcludedFolders.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvExcludedFolders_CellContentClick);
            // 
            // colPath
            // 
            this.colPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPath.HeaderText = "Path";
            this.colPath.MinimumWidth = 20;
            this.colPath.Name = "colPath";
            // 
            // colFolder
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "Folder";
            this.colFolder.DefaultCellStyle = dataGridViewCellStyle1;
            this.colFolder.HeaderText = "Folder";
            this.colFolder.Name = "colFolder";
            this.colFolder.ReadOnly = true;
            this.colFolder.Text = "Folder";
            this.colFolder.UseColumnTextForButtonValue = true;
            // 
            // colFile
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "File";
            this.colFile.DefaultCellStyle = dataGridViewCellStyle2;
            this.colFile.HeaderText = "File";
            this.colFile.Name = "colFile";
            this.colFile.ReadOnly = true;
            this.colFile.Text = "File";
            this.colFile.UseColumnTextForButtonValue = true;
            // 
            // colRemove
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "X";
            this.colRemove.DefaultCellStyle = dataGridViewCellStyle3;
            this.colRemove.HeaderText = "X";
            this.colRemove.MinimumWidth = 20;
            this.colRemove.Name = "colRemove";
            this.colRemove.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRemove.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colRemove.Text = "X";
            this.colRemove.UseColumnTextForButtonValue = true;
            this.colRemove.Width = 20;
            // 
            // lblOpt
            // 
            this.lblOpt.AutoSize = true;
            this.lblOpt.Location = new System.Drawing.Point(10, 122);
            this.lblOpt.Name = "lblOpt";
            this.lblOpt.Size = new System.Drawing.Size(52, 13);
            this.lblOpt.TabIndex = 13;
            this.lblOpt.Text = "(Optional)";
            // 
            // chkAutomatic
            // 
            this.chkAutomatic.AutoSize = true;
            this.chkAutomatic.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAutomatic.Location = new System.Drawing.Point(13, 406);
            this.chkAutomatic.Name = "chkAutomatic";
            this.chkAutomatic.Size = new System.Drawing.Size(150, 17);
            this.chkAutomatic.TabIndex = 12;
            this.chkAutomatic.Text = "Start Monitoring on launch";
            this.chkAutomatic.UseVisualStyleBackColor = true;
            this.chkAutomatic.CheckedChanged += new System.EventHandler(this.config_Changed);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(113, 442);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(9, 442);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 9;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(503, 59);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(110, 63);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(386, 20);
            this.txtLocation.TabIndex = 7;
            this.txtLocation.TextChanged += new System.EventHandler(this.config_Changed);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(110, 107);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(466, 87);
            this.txtDescription.TabIndex = 5;
            this.txtDescription.TextChanged += new System.EventHandler(this.config_Changed);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(110, 30);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(466, 20);
            this.txtName.TabIndex = 4;
            this.txtName.Text = "Project Name";
            this.txtName.TextChanged += new System.EventHandler(this.config_Changed);
            this.txtName.Enter += new System.EventHandler(this.configText_Enter);
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(7, 63);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 3;
            this.lblLocation.Text = "Location:";
            // 
            // lblProjectDescription
            // 
            this.lblProjectDescription.AutoSize = true;
            this.lblProjectDescription.Location = new System.Drawing.Point(7, 107);
            this.lblProjectDescription.Name = "lblProjectDescription";
            this.lblProjectDescription.Size = new System.Drawing.Size(99, 13);
            this.lblProjectDescription.TabIndex = 1;
            this.lblProjectDescription.Text = "Project Description:";
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Location = new System.Drawing.Point(7, 37);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(74, 13);
            this.lblProjectName.TabIndex = 0;
            this.lblProjectName.Text = "Project Name:";
            // 
            // rcmProject
            // 
            this.rcmProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmUpdate,
            this.tsmMonitor,
            this.tsmDelete,
            this.cloneToolStripMenuItem});
            this.rcmProject.Name = "rcmProject";
            this.rcmProject.Size = new System.Drawing.Size(118, 92);
            // 
            // tsmUpdate
            // 
            this.tsmUpdate.Name = "tsmUpdate";
            this.tsmUpdate.Size = new System.Drawing.Size(117, 22);
            this.tsmUpdate.Text = "Update";
            // 
            // tsmMonitor
            // 
            this.tsmMonitor.Name = "tsmMonitor";
            this.tsmMonitor.Size = new System.Drawing.Size(117, 22);
            this.tsmMonitor.Text = "Monitor";
            this.tsmMonitor.Click += new System.EventHandler(this.tsmMonitor_Click);
            // 
            // tsmDelete
            // 
            this.tsmDelete.Name = "tsmDelete";
            this.tsmDelete.Size = new System.Drawing.Size(117, 22);
            this.tsmDelete.Text = "Delete";
            this.tsmDelete.Click += new System.EventHandler(this.tsmDelete_Click);
            // 
            // cloneToolStripMenuItem
            // 
            this.cloneToolStripMenuItem.Name = "cloneToolStripMenuItem";
            this.cloneToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.cloneToolStripMenuItem.Text = "Clone";
            this.cloneToolStripMenuItem.Click += new System.EventHandler(this.cloneToolStripMenuItem_Click);
            // 
            // rcmChanges
            // 
            this.rcmChanges.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmRestore,
            this.restoreToCloneToolStripMenuItem});
            this.rcmChanges.Name = "rcmProject";
            this.rcmChanges.Size = new System.Drawing.Size(160, 48);
            // 
            // tsmRestore
            // 
            this.tsmRestore.Name = "tsmRestore";
            this.tsmRestore.Size = new System.Drawing.Size(159, 22);
            this.tsmRestore.Text = "Undo Changes";
            this.tsmRestore.Click += new System.EventHandler(this.tsmRestore_Click);
            // 
            // restoreToCloneToolStripMenuItem
            // 
            this.restoreToCloneToolStripMenuItem.Name = "restoreToCloneToolStripMenuItem";
            this.restoreToCloneToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.restoreToCloneToolStripMenuItem.Text = "Restore to clone";
            this.restoreToCloneToolStripMenuItem.Click += new System.EventHandler(this.restoreToCloneToolStripMenuItem_Click);
            // 
            // lblExcludeList
            // 
            this.lblExcludeList.AutoSize = true;
            this.lblExcludeList.Location = new System.Drawing.Point(3, 200);
            this.lblExcludeList.Name = "lblExcludeList";
            this.lblExcludeList.Size = new System.Drawing.Size(64, 26);
            this.lblExcludeList.TabIndex = 16;
            this.lblExcludeList.Text = "Files/folders\r\n to exclude:";
            // 
            // LVControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 528);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LVControl";
            this.Text = "LVControl";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LVControl_FormClosing);
            this.Load += new System.EventHandler(this.LVControl_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tbcProject.ResumeLayout(false);
            this.tabStart.ResumeLayout(false);
            this.tabStart.PerformLayout();
            this.tabChanges.ResumeLayout(false);
            this.tabChanges.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabConfig.ResumeLayout(false);
            this.tabConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExcludedFolders)).EndInit();
            this.rcmProject.ResumeLayout(false);
            this.rcmChanges.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tbcProject;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblProjectDescription;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.TabPage tabChanges;
        private System.Windows.Forms.ListView lsvChanges;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TabPage tabStart;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.Label lblDescriptionValue;
        private System.Windows.Forms.Label lblLocationValue;
        private System.Windows.Forms.Label lblNameValue;
        private System.Windows.Forms.Label lblStartLocation;
        private System.Windows.Forms.Label lblStartDescription;
        private System.Windows.Forms.Label lblStartProjectName;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnMonitoring;
        private System.Windows.Forms.CheckBox chkAutomatic;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.FolderBrowserDialog fbdFolderBrowser;
        private System.Windows.Forms.Label lblOpt;
        private System.Windows.Forms.ListView lsvProjectList;
        private System.Windows.Forms.Label lblDateValue;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.ComboBox cmbSort;
        private System.Windows.Forms.ContextMenuStrip rcmProject;
        private System.Windows.Forms.ToolStripMenuItem tsmUpdate;
        private System.Windows.Forms.ToolStripMenuItem tsmMonitor;
        private System.Windows.Forms.ToolStripMenuItem tsmDelete;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.DataGridView dgvExcludedFolders;
        private System.Windows.Forms.OpenFileDialog ofdFileBrowser;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ContextMenuStrip rcmChanges;
        private System.Windows.Forms.ToolStripMenuItem tsmRestore;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSetting;
        private System.Windows.Forms.ToolStripButton tsbRestoreClone;
        private System.Windows.Forms.ToolStripMenuItem cloneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreToCloneToolStripMenuItem;
        private System.Windows.Forms.Button btnClone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPath;
        private System.Windows.Forms.DataGridViewButtonColumn colFolder;
        private System.Windows.Forms.DataGridViewButtonColumn colFile;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
        private System.Windows.Forms.Label lblExcludeList;
    }
}

