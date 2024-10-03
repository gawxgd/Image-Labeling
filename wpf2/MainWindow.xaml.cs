using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Point startPoint;
        private System.Windows.Shapes.Rectangle rectangle;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnTextBoxButtonClick(object sender, RoutedEventArgs e)
        {
            string labelText = labelTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(labelText))
            {
                Random random = new Random();
                System.Windows.Media.Color randomColor = System.Windows.Media.Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
                Brush brush = new SolidColorBrush(randomColor);

                LabelViewModel labelViewModel = new LabelViewModel { Name = labelText, BackgroundColor = brush };
                labelListView.Items.Add(labelViewModel);

                labelTextBox.Clear();
            }
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(canvas);
            LabelViewModel item = (LabelViewModel)labelListView.SelectedItem;
            if(item is null)
            {
                MessageBox.Show("label is not selected");
                return;
            }
            Brush brush = item.BackgroundColor;
            SolidColorBrush strokeBrush = brush as SolidColorBrush;

            rectangle = new System.Windows.Shapes.Rectangle
            {
                Stroke = strokeBrush, // Ustaw kolor ramki na odpowiedni kolor etykiety
                StrokeThickness = 2,
                Fill = Brushes.Transparent,
                Tag = item// Ustaw wypełnienie na przezroczyste
            };
            Canvas.SetLeft(rectangle, startPoint.X);
            Canvas.SetTop(rectangle, startPoint.Y);
            canvas.Children.Add(rectangle);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || rectangle == null)
                return;

            System.Windows.Point currentPoint = e.GetPosition(canvas);

            double width = currentPoint.X - startPoint.X;
            double height = currentPoint.Y - startPoint.Y;

            if (width < 0)
            {
                Canvas.SetLeft(rectangle, currentPoint.X);
                width *= -1;
            }

            if (height < 0)
            {
                Canvas.SetTop(rectangle, currentPoint.Y);
                height *= -1;
            }

            rectangle.Width = width;
            rectangle.Height = height;
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rectangle = null;
        }
        private void RemoveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;
            ListViewItem listViewItem = (ListViewItem)contextMenu.PlacementTarget;
            LabelViewModel itemToRemove = (LabelViewModel)listViewItem.DataContext;

            labelListView.Items.Remove(itemToRemove);
            var rectanglesToRemove = new List<System.Windows.Shapes.Rectangle>();

            // Find and remove all rectangles with the same Tag
            foreach (UIElement child in canvas.Children)
            {
                if (child is System.Windows.Shapes.Rectangle rectangle && rectangle.Tag == itemToRemove)
                {
                    rectanglesToRemove.Add(rectangle);
                }
            }

            // Remove all collected rectangles
            foreach (var rectangle in rectanglesToRemove)
            {
                canvas.Children.Remove(rectangle);
            }
        }
        private void EditLabelClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;
            ListViewItem listViewItem = (ListViewItem)contextMenu.PlacementTarget;
            LabelViewModel itemToEdit = (LabelViewModel)listViewItem.DataContext;

            itemToEdit.LabelVisibility = Visibility.Collapsed;
            itemToEdit.TextBoxVisibility = Visibility.Visible;
        }

        private void EditTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = (TextBox)sender;
                LabelViewModel itemToEdit = (LabelViewModel)textBox.DataContext;

                itemToEdit.LabelVisibility = Visibility.Visible;
                itemToEdit.TextBoxVisibility = Visibility.Collapsed;
            }
        }

        private void EditTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            LabelViewModel itemToEdit = (LabelViewModel)textBox.DataContext;

            itemToEdit.LabelVisibility = Visibility.Visible;
            itemToEdit.TextBoxVisibility = Visibility.Collapsed;
        }

    }

    public class LabelViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private string name;
        private Visibility labelVisibility = Visibility.Visible;
        private Visibility textBoxVisibility = Visibility.Collapsed;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public Brush BackgroundColor { get; set; }

        public Visibility LabelVisibility
        {
            get { return labelVisibility; }
            set
            {
                if (labelVisibility != value)
                {
                    labelVisibility = value;
                    OnPropertyChanged("LabelVisibility");
                }
            }
        }

        public Visibility TextBoxVisibility
        {
            get { return textBoxVisibility; }
            set
            {
                if (textBoxVisibility != value)
                {
                    textBoxVisibility = value;
                    OnPropertyChanged("TextBoxVisibility");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

}