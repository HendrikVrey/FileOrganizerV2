using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace FileOrganizerWinForms
{
    public partial class Form1 : MaterialForm
    {
        //Dictionary to store original file paths before organizing
        private Dictionary<string, string> originalPaths = new Dictionary<string, string>();
        //List to keep track of directories created during file organization
        private List<string> createdDirectories = new List<string>();
        //Instance of MaterialSkinManager to manage themes and colors
        private readonly MaterialSkinManager materialSkinManager;
        //File path of settings
        private readonly string settingsFilePath;

        public Form1()
        {
            InitializeComponent();

            //Path to save the settings file in the Documents folder
            settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FileOrganizerSettings.json");

            //Initialize MaterialSkinManager with the current form
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);

            //Load saved settings (if available) on startup
            LoadSettings();

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
            //Show the "Organize" button only if a valid option is selected
            btnOrganize.Visible = cmbOrganizationType.SelectedIndex > 0;
        }

        //Event handler for the "Organize" button click
        private void btnOrganize_Click(object sender, EventArgs e)
        {
            string directoryPath = txtDirectoryPath.Text;

            //Check if the directory exists and contains files
            if (Directory.Exists(directoryPath) && Directory.GetFiles(directoryPath).Length > 0)
            {
                try
                {
                    //Perform file organization based on the selected option
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
                            //Invalid option message
                            lblStatus.Text = "Please select a valid organization type.";
                            return;
                    }
                    lblStatus.Text = "Files have been organized successfully.";
                }
                catch (Exception ex)
                {
                    //Display an error message if something goes wrong
                    lblStatus.Text = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                //Invalid directory or empty directory message
                lblStatus.Text = "Invalid directory path or directory is empty.";
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

        //Method to organize files by their type (extension)
        private void OrganizeFilesByType(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                string originalPath = file;
                //Get the file extension and convert it to uppercase
                string extension = Path.GetExtension(file).TrimStart('.').ToUpper();
                //Create a directory for the file type (extension)
                string destinationDirectory = Path.Combine(directoryPath, extension);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    createdDirectories.Add(destinationDirectory); //Keep track of created directories
                }

                //Move the file to its new location
                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                originalPaths[destinationFile] = originalPath; //Store original path
                File.Move(file, destinationFile); //Move the file
            }
        }

        //Method to organize files by their creation date (year and month)
        private void OrganizeFilesByDate(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                string originalPath = file;
                //Get the file's creation date and format it as "yyyy-MM"
                DateTime creationDate = File.GetCreationTime(file);
                string yearMonth = creationDate.ToString("yyyy-MM");

                //Create a directory based on the creation date
                string destinationDirectory = Path.Combine(directoryPath, yearMonth);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    createdDirectories.Add(destinationDirectory); //Track created directories
                }

                //Move the file to the new date-based directory
                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                originalPaths[destinationFile] = originalPath; //Store original path
                File.Move(file, destinationFile);
            }
        }

        //Method to organize files by size category (small, medium, large, etc.)
        private void OrganizeFilesBySize(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                string originalPath = file;
                //Get the size of the file
                long fileSize = new FileInfo(file).Length;
                //Determine the size category (e.g., Small, Medium, Large)
                string sizeCategory = GetSizeCategory(fileSize);

                //Create a directory based on the file size category
                string destinationDirectory = Path.Combine(directoryPath, sizeCategory);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    createdDirectories.Add(destinationDirectory); //Track created directories
                }

                //Move the file to the new size-based directory
                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                originalPaths[destinationFile] = originalPath; //Store original path
                File.Move(file, destinationFile);
            }
        }

        //Helper method to categorize files by size
        private string GetSizeCategory(long fileSize)
        {
            if (fileSize < 1048576) //Less than 1MB
            {
                return "Small (Less than 1MB)";
            }
            else if (fileSize < 10485760) //1MB to 10MB
            {
                return "Medium (1MB to 10MB)";
            }
            else if (fileSize < 104857600) //10MB to 100MB
            {
                return "Large (10MB to 100MB)";
            }
            else //100MB and above
            {
                return "Extra Large (100MB and above)";
            }
        }

        //Method to organize files by both type (extension) and creation date
        private void OrganizeFilesByTypeAndDate(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                string originalPath = file;
                //Get the file extension and creation date
                string extension = Path.GetExtension(file).TrimStart('.').ToUpper();
                DateTime creationDate = File.GetCreationTime(file);
                string yearMonth = creationDate.ToString("yyyy-MM");

                //Create a directory based on file type and date
                string destinationDirectory = Path.Combine(directoryPath, extension, yearMonth);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    createdDirectories.Add(destinationDirectory); //Track created directories
                }

                //Move the file to the new type-and-date-based directory
                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                originalPaths[destinationFile] = originalPath; //Store original path
                File.Move(file, destinationFile);
            }
        }

        //Method to undo the last file organization by restoring original paths
        private void UndoLastSort()
        {
            //Move each file back to its original path
            foreach (var kvp in originalPaths)
            {
                if (File.Exists(kvp.Key))
                {
                    File.Move(kvp.Key, kvp.Value);
                }
            }
            originalPaths.Clear();

            //Delete any directories created during the sort
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

        //Event handler to toggle between dark and light modes
        private void toggleDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            //Change the theme based on the toggle switch state
            if (toggleDarkMode.Checked)
            {
                materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            }
            else
            {
                materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            }

            //Save the current theme state to a file
            SaveSettings();
        }

        //Method to save the current theme state to a JSON file
        private void SaveSettings()
        {
            var settings = new
            {
                IsDarkMode = toggleDarkMode.Checked
            };

            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(settingsFilePath, json);
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

                    // Apply the saved theme state
                    toggleDarkMode.Checked = settings.IsDarkMode;
                    materialSkinManager.Theme = settings.IsDarkMode ? MaterialSkinManager.Themes.DARK : MaterialSkinManager.Themes.LIGHT;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading settings: {ex.Message}");
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
