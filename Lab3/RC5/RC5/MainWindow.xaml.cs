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

namespace RC5
{
   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RD5Message_btn(object sender, RoutedEventArgs e)
        {
            string key = KeyBox.Text;
            string message = MessageBox.Text;

            string enc, dec;

            (enc,dec) = Handler.Generate(key, message);

            ScriptedMessageBox.Text = string.Empty;
            DecriptedMessageBox.Text = string.Empty;


            ScriptedMessageBox.Text = enc;
            DecriptedMessageBox.Text = dec;
        }

        private void LoadFile_btn(object sender, RoutedEventArgs e)
        {
            FileBox.Text = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*",
                Title = "Виберіть файл для хешування"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                FileBox.Text = filePath;
            }
        }

        private void RD5File_btn(object sender, RoutedEventArgs e)
        {
            string key = KeyBox.Text;
            string filepath = FileBox.Text;
            string output = FileOutputBox.Text;
            string decoutput = FileDeOutputBox.Text;

            

            Handler.Generate(key, filepath, output, decoutput);

        }
    }
}