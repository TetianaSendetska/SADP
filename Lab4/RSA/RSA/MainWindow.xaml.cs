using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.IO;

namespace RSA
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


        private void btnEncRCA_Click(object sender, RoutedEventArgs e)
        {
            string publicKeyPath = "publicKey.txt";
            string privateKeyPath = "privateKey.txt";

            // Шляхи до файлів для шифрування і дешифрування
            string originalFilePath = FileBox.Text;
            string encryptedFilePath = FileOutputBox.Text;

            string result = Handler.EncryptRCA(originalFilePath, encryptedFilePath, publicKeyPath, privateKeyPath);

            Log.Text = Log.Text + result;
        }

        private void btnDecRCA_Click(object sender, RoutedEventArgs e)
        {
            string privateKeyPath = "privateKey.txt";

            string encryptedFilePath = FileOutputBox.Text;
            string decryptedFilePath = FileDeOutputBox.Text;

            string result = Handler.DecryptRCA(encryptedFilePath, decryptedFilePath,privateKeyPath);

            Log.Text = Log.Text + result;
        }

        private void btnEncRC5_Click(object sender, RoutedEventArgs e)
        {
            string key = KeyBox.Text;
            string filepath = FileBox.Text;
            string output = FileOutputBox.Text;

            byte[] iv = new byte[8];
            new Random().NextBytes(iv);

            Log.Text = Log.Text + Handler.GenerateEnc(key, filepath, output, iv);
        }

        private void btnDecRC5_Click(object sender, RoutedEventArgs e)
        {
            string key = KeyBox.Text;
            string output = FileOutputBox.Text;
            string decoutput = FileDeOutputBox.Text;

            Log.Text = Log.Text + Handler.GenerateDec(key, output, decoutput);
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
    }
}