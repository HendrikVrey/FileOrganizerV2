using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace FileOrganizerWinForms
{
    public partial class Form1 : MaterialForm
    {
        //Dictionary to store original file paths before organizing
        private readonly Dictionary<string, string> originalPaths = new Dictionary<string, string>();
        //List to keep track of directories created during file organization
        private readonly List<string> createdDirectories = new List<string>();
        //Instance of MaterialSkinManager to manage themes and colors
        private readonly MaterialSkinManager materialSkinManager;
        //File path of settings
        private readonly string settingsFilePath;

        public Form1()
        {
            InitializeComponent();

            //Path to save the settings file in the Documents folder
            settingsFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "FileOrganizerSettings.json"
            );

            //Initialize MaterialSkinManager with the current form
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);

            //Load saved settings (if available) on startup
            LoadSettings();

            // Set the color scheme for the application
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.LightBlue300,
                Primary.LightBlue500,
                Primary.LightBlue200,
                Accent.LightGreen400,
                TextShade.BLACK
            );
        }

        //Event handler when the organization type is changed in the ComboBox
        private void cmbOrganizationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Enable the "Organize" button only if a valid option is selected
            btnOrganize.Visible = cmbOrganizationType.SelectedIndex > 0;
        }

        //Event handler for the "Organize" button click
        private void btnOrganize_Click(object sender, EventArgs e)
        {
            string directoryPath = txtDirectoryPath.Text.Trim();

            //Validate directory path
            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                lblStatus.Text = "Invalid directory path.";
                return;
            }

            //Check if the directory contains any files
            if (!Directory.EnumerateFiles(directoryPath).Any())
            {
                lblStatus.Text = "Directory is empty.";
                return;
            }

            //Ensure an organization type is selected
            if (cmbOrganizationType.SelectedItem == null)
            {
                lblStatus.Text = "Please select an organization type.";
                return;
            }

            try
            {
                //Perform file organization based on the selected option
                switch (cmbOrganizationType.SelectedItem.ToString())
                {
                    case "By Type":
                        OrganizeFiles(directoryPath, GetTypeDirectory);
                        break;
                    case "By Date":
                        OrganizeFiles(directoryPath, GetDateDirectory);
                        break;
                    case "By Size":
                        OrganizeFiles(directoryPath, GetSizeDirectory);
                        break;
                    case "By Type and Date":
                        OrganizeFiles(directoryPath, GetTypeAndDateDirectory);
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

        //Event handler for the "Browse" button to select a folder
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                //Show folder browser dialog and update the path if selected
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDirectoryPath.Text = folderDialog.SelectedPath;
                }
            }
        }

        //Event handler for the "Undo Sort" button to undo the last organization
        private void btnUndoSort_Click(object sender, EventArgs e)
        {
            UndoLastSort();
        }

        //Method to organize files based on a provided criterion
        private void OrganizeFiles(string directoryPath, Func<string, string, string> getDestinationDirectory)
        {
            var files = Directory.GetFiles(directoryPath);
            var errors = new StringBuilder();

            foreach (var file in files)
            {
                try
                {
                    string destinationDirectory = getDestinationDirectory(directoryPath, file);

                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                        createdDirectories.Add(destinationDirectory); //Keep track of created directories
                    }

                    //Move the file to its new location
                    string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                    originalPaths[destinationFile] = file; //Store original path

                    if (File.Exists(destinationFile))
                    {
                        //Handle the case where the destination file already exists
                        destinationFile = GetUniqueFileName(destinationFile);
                    }

                    File.Move(file, destinationFile); //Move the file
                }
                catch (Exception ex)
                {
                    //Collect error messages
                    errors.AppendLine($"Error moving file {file}: {ex.Message}");
                }
            }

            //Display errors if any
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Errors Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Helper method to get destination directory based on file type
        private string GetTypeDirectory(string directoryPath, string file)
        {
            string extension = Path.GetExtension(file).TrimStart('.').ToUpperInvariant();
            return Path.Combine(directoryPath, extension);
        }

        //Helper method to get destination directory based on file date
        private string GetDateDirectory(string directoryPath, string file)
        {
            DateTime creationDate = File.GetCreationTime(file);
            string yearMonth = creationDate.ToString("yyyy-MM");
            return Path.Combine(directoryPath, yearMonth);
        }

        //Helper method to get destination directory based on file size
        private string GetSizeDirectory(string directoryPath, string file)
        {
            long fileSize = new FileInfo(file).Length;
            string sizeCategory = GetSizeCategory(fileSize);
            return Path.Combine(directoryPath, sizeCategory);
        }

        //Helper method to get destination directory based on file type and date
        private string GetTypeAndDateDirectory(string directoryPath, string file)
        {
            string extension = Path.GetExtension(file).TrimStart('.').ToUpperInvariant();
            DateTime creationDate = File.GetCreationTime(file);
            string yearMonth = creationDate.ToString("yyyy-MM");
            return Path.Combine(directoryPath, extension, yearMonth);
        }

        //Helper method to categorize files by size
        private string GetSizeCategory(long fileSize)
        {
            if (fileSize < 1_048_576) //Less than 1MB
                return "Small (Less than 1MB)";
            else if (fileSize < 10_485_760) //1MB to 10MB
                return "Medium (1MB to 10MB)";
            else if (fileSize < 104_857_600) //10MB to 100MB
                return "Large (10MB to 100MB)";
            else //100MB and above
                return "Extra Large (100MB and above)";
        }

        //Helper method to get a unique file name if the file already exists
        private string GetUniqueFileName(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            int count = 1;
            string newFilePath;
            do
            {
                string newFileName = $"{fileNameWithoutExtension} ({count++}){extension}";
                newFilePath = Path.Combine(directory, newFileName);
            } while (File.Exists(newFilePath));

            return newFilePath;
        }

        //Method to undo the last file organization by restoring original paths
        private void UndoLastSort()
        {
            var errors = new StringBuilder();

            //Move each file back to its original path
            foreach (var kvp in originalPaths)
            {
                try
                {
                    if (File.Exists(kvp.Key))
                    {
                        string destinationDirectory = Path.GetDirectoryName(kvp.Value);
                        if (!Directory.Exists(destinationDirectory))
                        {
                            Directory.CreateDirectory(destinationDirectory);
                        }

                        string destinationFile = kvp.Value;

                        if (File.Exists(destinationFile))
                        {
                            //Handle if the original file exists
                            destinationFile = GetUniqueFileName(destinationFile);
                        }

                        File.Move(kvp.Key, destinationFile);
                    }
                }
                catch (Exception ex)
                {
                    //Collect error messages
                    errors.AppendLine($"Error restoring file {kvp.Key}: {ex.Message}");
                }
            }
            originalPaths.Clear();

            //Delete any directories created during the sort
            foreach (var dir in createdDirectories)
            {
                try
                {
                    if (Directory.Exists(dir))
                    {
                        Directory.Delete(dir, true);
                    }
                }
                catch (Exception ex)
                {
                    errors.AppendLine($"Error deleting directory {dir}: {ex.Message}");
                }
            }
            createdDirectories.Clear();

            lblStatus.Text = "Last sort has been undone.";

            //Display errors if any
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Errors Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Event handler to toggle between dark and light modes
        private void toggleDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            //Change the theme based on the toggle switch state
            materialSkinManager.Theme = toggleDarkMode.Checked
                ? MaterialSkinManager.Themes.DARK
                : MaterialSkinManager.Themes.LIGHT;

            //Save the current theme state to a file
            SaveSettings();
        }

        //Method to save the current theme state to a JSON file
        private void SaveSettings()
        {
            var settings = new Settings
            {
                IsDarkMode = toggleDarkMode.Checked
            };

            try
            {
                string json = JsonSerializer.Serialize(settings);
                File.WriteAllText(settingsFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Method to load the theme state from the JSON file
        private void LoadSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                try
                {
                    string json = File.ReadAllText(settingsFilePath);
                    var settings = JsonSerializer.Deserialize<Settings>(json);

                    //Apply the saved theme state
                    toggleDarkMode.Checked = settings.IsDarkMode;
                    materialSkinManager.Theme = settings.IsDarkMode
                        ? MaterialSkinManager.Themes.DARK
                        : MaterialSkinManager.Themes.LIGHT;
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Error parsing settings file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Class to represent the settings data
        private class Settings
        {
            public bool IsDarkMode { get; set; }
        }
    }
}
