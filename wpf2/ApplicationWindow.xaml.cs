using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace wpf2
{
    /// <summary>
    /// Logika interakcji dla klasy ApplicationWindow.xaml
    /// </summary>
    public partial class ApplicationWindow : Window
    {
        public ApplicationWindow()
        {
            InitializeComponent();
        }
        private void LoadDataset_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select Folder",
                ValidateNames = false,
                Title = "Select Dataset Folder"
            };

            if (dlg.ShowDialog() == true)
            {
                string folderPath = Path.GetDirectoryName(dlg.FileName);
                LoadDirectory(folderPath);
            }
        }

        private void LoadDirectory(string folderPath)
        {
            var items = new ObservableCollection<FileSystemItem>();

            if (folderPath != null)
            {
                var parentDir = Directory.GetParent(folderPath);
                if (parentDir != null)
                {
                    items.Add(new FileSystemItem
                    {
                        Name = "..",
                        FullPath = parentDir.FullName,
                        Icon = (BitmapImage)Resources["FolderIcon"]
                    });
                }
            }

            foreach (var dir in Directory.GetDirectories(folderPath))
            {
                items.Add(new FileSystemItem
                {
                    Name = Path.GetFileName(dir),
                    FullPath = dir,
                    Icon = (BitmapImage)Resources["FolderIcon"]
                });
            }

            foreach (var file in Directory.GetFiles(folderPath))
            {
                if (IsImageFile(file))
                {
                    items.Add(new FileSystemItem
                    {
                        Name = Path.GetFileName(file),
                        FullPath = file,
                        Icon = new BitmapImage(new Uri(file))
                    });
                }
            }

            FileListView.ItemsSource = items;
        }

        private bool IsImageFile(string filePath)
        {
            string[] extensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            string ext = Path.GetExtension(filePath);
            return Array.Exists(extensions, e => e.Equals(ext, StringComparison.OrdinalIgnoreCase));
        }

        private void FileListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = FileListView.SelectedItem as FileSystemItem;
            if (selectedItem != null && Directory.Exists(selectedItem.FullPath))
            {
                LoadDirectory(selectedItem.FullPath);
            }
        }

        private void StartLabeling_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic to start labeling here
            MessageBox.Show("Start Labeling clicked");
        }
    }
}

