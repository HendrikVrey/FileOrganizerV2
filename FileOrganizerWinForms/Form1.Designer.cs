using MaterialSkin.Controls;
using System.Drawing;

namespace FileOrganizerWinForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private MaterialTextBox txtDirectoryPath;
        private MaterialButton btnBrowse;
        private MaterialLabel lblStatusText;
        private MaterialLabel lblStatus;
        private MaterialComboBox cmbOrganizationType;
        private MaterialButton btnOrganize;
        private MaterialButton btnUndoSort;
        private MaterialSwitch toggleDarkMode;

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
            txtDirectoryPath = new MaterialTextBox();
            btnBrowse = new MaterialButton();
            lblStatusText = new MaterialLabel();
            lblStatus = new MaterialLabel();
            cmbOrganizationType = new MaterialComboBox();
            btnOrganize = new MaterialButton();
            btnUndoSort = new MaterialButton();
            toggleDarkMode = new MaterialSwitch();
            SuspendLayout();
            // 
            // txtDirectoryPath
            // 
            txtDirectoryPath.AnimateReadOnly = false;
            txtDirectoryPath.BorderStyle = BorderStyle.None;
            txtDirectoryPath.Depth = 0;
            txtDirectoryPath.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtDirectoryPath.LeadingIcon = null;
            txtDirectoryPath.Location = new Point(12, 76);
            txtDirectoryPath.MaxLength = 50;
            txtDirectoryPath.MouseState = MaterialSkin.MouseState.OUT;
            txtDirectoryPath.Multiline = false;
            txtDirectoryPath.Name = "txtDirectoryPath";
            txtDirectoryPath.Size = new Size(366, 50);
            txtDirectoryPath.TabIndex = 0;
            txtDirectoryPath.Text = "";
            txtDirectoryPath.TrailingIcon = null;
            // 
            // btnBrowse
            // 
            btnBrowse.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnBrowse.Density = MaterialButton.MaterialButtonDensity.Default;
            btnBrowse.Depth = 0;
            btnBrowse.HighEmphasis = true;
            btnBrowse.Icon = Image.FromFile("icons/search.png"); 
            btnBrowse.ImageAlign = ContentAlignment.MiddleLeft;
            btnBrowse.Location = new Point(385, 76);
            btnBrowse.Margin = new Padding(4, 6, 4, 6);
            btnBrowse.MouseState = MaterialSkin.MouseState.HOVER;
            btnBrowse.Name = "btnBrowse";
            btnBrowse.NoAccentTextColor = Color.Empty;
            btnBrowse.Size = new Size(100, 36);
            btnBrowse.TabIndex = 7;
            btnBrowse.Text = "Browse";
            btnBrowse.Type = MaterialButton.MaterialButtonType.Contained;
            btnBrowse.UseAccentColor = false;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // lblStatusText
            // 
            lblStatusText.AutoSize = true;
            lblStatusText.Depth = 0;
            lblStatusText.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblStatusText.Location = new Point(12, 230);
            lblStatusText.MouseState = MaterialSkin.MouseState.HOVER;
            lblStatusText.Name = "lblStatusText";
            lblStatusText.Size = new Size(51, 19);
            lblStatusText.TabIndex = 6;
            lblStatusText.Text = "Status:";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Depth = 0;
            lblStatus.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblStatus.Location = new Point(70, 230);
            lblStatus.MouseState = MaterialSkin.MouseState.HOVER;
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1, 0);
            lblStatus.TabIndex = 7;
            // 
            // cmbOrganizationType
            // 
            cmbOrganizationType.AutoResize = false;
            cmbOrganizationType.BackColor = Color.FromArgb(255, 255, 255);
            cmbOrganizationType.Depth = 0;
            cmbOrganizationType.DrawMode = DrawMode.OwnerDrawVariable;
            cmbOrganizationType.DropDownHeight = 174;
            cmbOrganizationType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrganizationType.DropDownWidth = 121;
            cmbOrganizationType.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Pixel);
            cmbOrganizationType.ForeColor = Color.FromArgb(222, 0, 0, 0);
            cmbOrganizationType.FormattingEnabled = true;
            cmbOrganizationType.IntegralHeight = false;
            cmbOrganizationType.ItemHeight = 43;
            cmbOrganizationType.Items.AddRange(new object[] { "Organization Type:", "By Type", "By Date", "By Size", "By Type and Date" });
            cmbOrganizationType.Location = new Point(12, 132);
            cmbOrganizationType.MaxDropDownItems = 4;
            cmbOrganizationType.MouseState = MaterialSkin.MouseState.OUT;
            cmbOrganizationType.Name = "cmbOrganizationType";
            cmbOrganizationType.Size = new Size(200, 49);
            cmbOrganizationType.StartIndex = 0;
            cmbOrganizationType.TabIndex = 8;
            cmbOrganizationType.SelectedIndexChanged += cmbOrganizationType_SelectedIndexChanged;
            // 
            // btnOrganize
            // 
            btnOrganize.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnOrganize.Density = MaterialButton.MaterialButtonDensity.Default;
            btnOrganize.Depth = 0;
            btnOrganize.HighEmphasis = true;
            btnOrganize.Icon = Image.FromFile("icons/sort.png"); 
            btnOrganize.ImageAlign = ContentAlignment.MiddleLeft;
            btnOrganize.Location = new Point(219, 135);
            btnOrganize.Margin = new Padding(4, 6, 4, 6);
            btnOrganize.MouseState = MaterialSkin.MouseState.HOVER;
            btnOrganize.Name = "btnOrganize";
            btnOrganize.NoAccentTextColor = Color.Empty;
            btnOrganize.Size = new Size(120, 36);
            btnOrganize.TabIndex = 9;
            btnOrganize.Text = "Organize";
            btnOrganize.Type = MaterialButton.MaterialButtonType.Contained;
            btnOrganize.UseAccentColor = false;
            btnOrganize.Visible = false;
            btnOrganize.Click += btnOrganize_Click;
            // 
            // btnUndoSort
            // 
            btnUndoSort.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnUndoSort.Density = MaterialButton.MaterialButtonDensity.Default;
            btnUndoSort.Depth = 0;
            btnUndoSort.HighEmphasis = true;
            btnUndoSort.Icon = Image.FromFile("icons/undo.png"); 
            btnUndoSort.ImageAlign = ContentAlignment.MiddleLeft;
            btnUndoSort.Location = new Point(322, 255);
            btnUndoSort.Margin = new Padding(4, 6, 4, 6);
            btnUndoSort.MouseState = MaterialSkin.MouseState.HOVER;
            btnUndoSort.Name = "btnUndoSort";
            btnUndoSort.NoAccentTextColor = Color.Empty;
            btnUndoSort.Size = new Size(160, 36);
            btnUndoSort.TabIndex = 10;
            btnUndoSort.Text = "Undo Last Sort";
            btnUndoSort.Type = MaterialButton.MaterialButtonType.Contained;
            btnUndoSort.UseAccentColor = false;
            btnUndoSort.Click += btnUndoSort_Click;
            // 
            // toggleDarkMode
            // 
            toggleDarkMode.Depth = 0;
            toggleDarkMode.Location = new Point(12, 268);
            toggleDarkMode.Margin = new Padding(0);
            toggleDarkMode.MouseLocation = new Point(-1, -1);
            toggleDarkMode.MouseState = MaterialSkin.MouseState.HOVER;
            toggleDarkMode.Name = "toggleDarkMode";
            toggleDarkMode.Ripple = true;
            toggleDarkMode.Size = new Size(136, 23);
            toggleDarkMode.TabIndex = 11;
            toggleDarkMode.Text = "Dark Mode";
            toggleDarkMode.CheckedChanged += toggleDarkMode_CheckedChanged;
            // 
            // Form1
            // 
            ClientSize = new Size(500, 310);
            MaximizeBox = false;
            Controls.Add(txtDirectoryPath);
            Controls.Add(btnBrowse);
            Controls.Add(lblStatusText);
            Controls.Add(lblStatus);
            Controls.Add(cmbOrganizationType);
            Controls.Add(btnOrganize);
            Controls.Add(btnUndoSort);
            Controls.Add(toggleDarkMode);
            Name = "Form1";
            Text = "File Organizer";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
