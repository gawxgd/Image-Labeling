using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace wpf2
{
    public class FileSystemItem : INotifyPropertyChanged
    {
        private ObservableCollection<FileSystemItem> _children;
        public string Name { get; set; }
        public string FullPath { get; set; }
        public BitmapImage Icon { get; set; }
        public ObservableCollection<FileSystemItem> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                OnPropertyChanged("Children");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
