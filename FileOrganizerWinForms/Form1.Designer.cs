namespace FileOrganizerWinForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtDirectoryPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblStatusText;  // Label for "Status:"
        private System.Windows.Forms.Label lblStatus;      // Label for the dynamic status message
        private System.Windows.Forms.ComboBox cmbOrganizationType;
        private System.Windows.Forms.Button btnOrganize;
        private System.Windows.Forms.Button btnUndoSort;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtDirectoryPath = new TextBox();
            btnBrowse = new Button();
            lblStatusText = new Label();
            lblStatus = new Label();
            cmbOrganizationType = new ComboBox();
            btnOrganize = new Button();
            btnUndoSort = new Button();
            SuspendLayout();
            // 
            // txtDirectoryPath
            // 
            txtDirectoryPath.Location = new Point(12, 12);
            txtDirectoryPath.Name = "txtDirectoryPath";
            txtDirectoryPath.Size = new Size(371, 23);
            txtDirectoryPath.TabIndex = 0;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(389, 12);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(75, 23);
            btnBrowse.TabIndex = 7;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // lblStatusText
            // 
            lblStatusText.AutoSize = true;
            lblStatusText.Location = new Point(12, 64);
            lblStatusText.Name = "lblStatusText";
            lblStatusText.Size = new Size(42, 15);
            lblStatusText.TabIndex = 6;
            lblStatusText.Text = "Status:";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(55, 64);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.TabIndex = 7;
            // 
            // cmbOrganizationType
            // 
            this.cmbOrganizationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrganizationType.FormattingEnabled = true;
            this.cmbOrganizationType.Items.AddRange(new object[] {
            "Organization Type:",
            "By Type",
            "By Date",
            "By Size",
            "By Type and Date"});
            this.cmbOrganizationType.Location = new System.Drawing.Point(12, 38);
            this.cmbOrganizationType.Name = "cmbOrganizationType";
            this.cmbOrganizationType.Size = new System.Drawing.Size(200, 21);
            this.cmbOrganizationType.TabIndex = 8;
            this.cmbOrganizationType.SelectedIndex = 0; // Set default selection to "Organization Type:"
            this.cmbOrganizationType.SelectedIndexChanged += new System.EventHandler(this.cmbOrganizationType_SelectedIndexChanged);
            // 
            // btnOrganize
            // 
            btnOrganize.Location = new Point(145, 38);
            btnOrganize.Name = "btnOrganize";
            btnOrganize.Size = new Size(120, 23);
            btnOrganize.TabIndex = 9;
            btnOrganize.Text = "Organize";
            btnOrganize.UseVisualStyleBackColor = true;
            btnOrganize.Visible = false;
            btnOrganize.Click += btnOrganize_Click;
            // 
            // btnUndoSort
            // 
            btnUndoSort.Location = new Point(344, 65);
            btnUndoSort.Name = "btnUndoSort";
            btnUndoSort.Size = new Size(120, 23);
            btnUndoSort.TabIndex = 10;
            btnUndoSort.Text = "Undo Last Sort";
            btnUndoSort.UseVisualStyleBackColor = true;
            btnUndoSort.Click += btnUndoSort_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(474, 100);
            Controls.Add(btnUndoSort);
            Controls.Add(btnOrganize);
            Controls.Add(cmbOrganizationType);
            Controls.Add(lblStatusText);
            Controls.Add(lblStatus);
            Controls.Add(btnBrowse);
            Controls.Add(txtDirectoryPath);
            Name = "Form1";
            Text = "File Organizer";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
