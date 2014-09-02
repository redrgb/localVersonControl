namespace LocalVersionControl
{
    partial class SettingsDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtDefaultLocation = new System.Windows.Forms.TextBox();
            this.fdbDefaultLocation = new System.Windows.Forms.FolderBrowserDialog();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.txtDefaultExclude = new System.Windows.Forms.TextBox();
            this.lblDefaultExclude = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default Location";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(405, 25);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 10;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtDefaultLocation
            // 
            this.txtDefaultLocation.Location = new System.Drawing.Point(147, 27);
            this.txtDefaultLocation.Name = "txtDefaultLocation";
            this.txtDefaultLocation.Size = new System.Drawing.Size(246, 20);
            this.txtDefaultLocation.TabIndex = 9;
            // 
            // fdbDefaultLocation
            // 
            this.fdbDefaultLocation.RootFolder = System.Environment.SpecialFolder.CommonDocuments;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(245, 95);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(325, 95);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(405, 95);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 13;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // txtDefaultExclude
            // 
            this.txtDefaultExclude.Location = new System.Drawing.Point(146, 61);
            this.txtDefaultExclude.Name = "txtDefaultExclude";
            this.txtDefaultExclude.Size = new System.Drawing.Size(246, 20);
            this.txtDefaultExclude.TabIndex = 15;
            // 
            // lblDefaultExclude
            // 
            this.lblDefaultExclude.AutoSize = true;
            this.lblDefaultExclude.Location = new System.Drawing.Point(-1, 64);
            this.lblDefaultExclude.Name = "lblDefaultExclude";
            this.lblDefaultExclude.Size = new System.Drawing.Size(141, 13);
            this.lblDefaultExclude.TabIndex = 14;
            this.lblDefaultExclude.Text = "List of Extensions to exclude";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "(Colon (:)  seprated list)";
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 130);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDefaultExclude);
            this.Controls.Add(this.lblDefaultExclude);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtDefaultLocation);
            this.Controls.Add(this.label1);
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtDefaultLocation;
        private System.Windows.Forms.FolderBrowserDialog fdbDefaultLocation;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TextBox txtDefaultExclude;
        private System.Windows.Forms.Label lblDefaultExclude;
        private System.Windows.Forms.Label label2;
    }
}