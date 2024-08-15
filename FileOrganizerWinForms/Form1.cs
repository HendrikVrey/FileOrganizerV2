using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FileOrganizerWinForms
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> originalPaths = new Dictionary<string, string>();
        private List<string> createdDirectories = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void cmbOrganizationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Show the Organize button only if a valid option is selected (not the placeholder)
            btnOrganize.Visible = cmbOrganizationType.SelectedIndex > 0;
        }

        private void btnOrganize_Click(object sender, EventArgs e)
        {
            string directoryPath = txtDirectoryPath.Text;

            if (Directory.Exists(directoryPath) && Directory.GetFiles(directoryPath).Length > 0)
            {
                try
                {
                    switch (cmbOrganizationType.SelectedItem.ToString())
                    {
                        case "By Type":
                            OrganizeFilesByType(directoryPath);
                            break;
                        case "By Date":
                            OrganizeFilesByDate(directoryPath);
                            break;
                        case "By Size":
                            OrganizeFilesBySize(directoryPath);
                            break;
                        case "By Type and Date":
                            OrganizeFilesByTypeAndDate(directoryPath);
                            break;
                        default:
                            lblStatus.Text = "Please select a valid organization type.";
                            return;
                    }
                    lblStatus.Text = "Files have been organized successfully.";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                lblStatus.Text = "Invalid directory path or directory is empty.";
            }
        }



        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDirectoryPath.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btnUndoSort_Click(object sender, EventArgs e)
        {
            UndoLastSort();
        }

        private void OrganizeFilesByType(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                string originalPath = file;
                string extension = Path.GetExtension(file).TrimStart('.').ToUpper();
                string destinationDirectory = Path.Combine(directoryPath, extension);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    createdDirectories.Add(destinationDirectory);
                }

                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                originalPaths[destinationFile] = originalPath;
                File.Move(file, destinationFile);
            }
        }

        private void OrganizeFilesByDate(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                string originalPath = file;
                DateTime creationDate = File.GetCreationTime(file);
                string yearMonth = creationDate.ToString("yyyy-MM");

                string destinationDirectory = Path.Combine(directoryPath, yearMonth);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    createdDirectories.Add(destinationDirectory);
                }

                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                originalPaths[destinationFile] = originalPath;
                File.Move(file, destinationFile);
            }
        }

        private void OrganizeFilesBySize(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                string originalPath = file;
                long fileSize = new FileInfo(file).Length;
                string sizeCategory = GetSizeCategory(fileSize);

                string destinationDirectory = Path.Combine(directoryPath, sizeCategory);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    createdDirectories.Add(destinationDirectory);
                }

                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                originalPaths[destinationFile] = originalPath;
                File.Move(file, destinationFile);
            }
        }

        private string GetSizeCategory(long fileSize)
        {
            if (fileSize < 1048576)
            {
                return "Small (Less than 1MB)";
            }
            else if (fileSize < 10485760)
            {
                return "Medium (1MB to 10MB)";
            }
            else if (fileSize < 104857600)
            {
                return "Large (10MB to 100MB)";
            }
            else
            {
                return "Extra Large (100MB and above)";
            }
        }

        private void OrganizeFilesByTypeAndDate(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                string originalPath = file;
                string extension = Path.GetExtension(file).TrimStart('.').ToUpper();
                DateTime creationDate = File.GetCreationTime(file);
                string yearMonth = creationDate.ToString("yyyy-MM");

                string destinationDirectory = Path.Combine(directoryPath, extension, yearMonth);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    createdDirectories.Add(destinationDirectory);
                }

                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                originalPaths[destinationFile] = originalPath;
                File.Move(file, destinationFile);
            }
        }

        private void UndoLastSort()
        {
            foreach (var kvp in originalPaths)
            {
                if (File.Exists(kvp.Key))
                {
                    File.Move(kvp.Key, kvp.Value);
                }
            }
            originalPaths.Clear();

            foreach (var dir in createdDirectories)
            {
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                }
            }
            createdDirectories.Clear();

            lblStatus.Text = "Last sort has been undone.";
        }
    }
}
