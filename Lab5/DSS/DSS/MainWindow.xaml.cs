using Microsoft.Win32;
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

namespace DSS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void LoadFile_btn(object sender, RoutedEventArgs e)
        {
            FileBox.Text = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*",
                Title = "Виберіть файл для підпису"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                FileBox.Text = filePath;
            }
        }

        private void btnSignFile_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileBox.Text;
            string fileSignPath = FileOutputBox.Text;

            string result = Handler.SignFile(filePath, fileSignPath);
            Log.Text = Log.Text + result + "Підписано!\n";
        }

        private void btnSignText_Click(object sender, RoutedEventArgs e)
        {
            string text = TextBox.Text;
            string textSignPath = TextOutputBox.Text;

            string result = Handler.SignText(text, textSignPath);
            Log.Text = Log.Text + result + "Підписано!\n";
        }

        private void btnCheckSignFile_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileBox.Text;
            string fileSignPath = FileOutputBox.Text;

            string result = Handler.VerifyFile(filePath, fileSignPath);
            Log.Text = Log.Text + result;
        }

        private void btnCheckSignText_Click(object sender, RoutedEventArgs e)
        {
            string text = TextBox.Text;
            string textSignPath = TextOutputBox.Text;

            string result = Handler.VerifyText(text, textSignPath);
            Log.Text = Log.Text + result;
        }
    }
}