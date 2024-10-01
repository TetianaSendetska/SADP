using MB5.Handlers;
using Microsoft.Win32;
using System.Text;
using System.Windows;


namespace MB5
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
        private void HashButton_Click(object sender, RoutedEventArgs e)
        {
            string text = textbox.Text;
            byte[] inputBytes = Encoding.UTF8.GetBytes(text);
            string hash = MD5Hash.GetMd5Hash(inputBytes);

            hashTextBox.Text = string.Empty;
            hashTextBox.Text = hash;
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            hashTextBox.Text = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*",
                Title = "Виберіть файл для хешування"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string hash = MD5Hash.GetMd5HashFromFile(filePath);

                hashTextBox.Text = hash;
            }
        }

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            string hash1 = hashTextBox.Text.ToLowerInvariant(); 
            string hash2 = hashCompare.Text.ToLowerInvariant();

            if (hash1.Equals(hash2, StringComparison.OrdinalIgnoreCase))
            {
               MessageBox.Show("Хеші однакові.", "Результат порівняння", MessageBoxButton.OK,
                                          MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Хеші різні.", "Результат порівняння", MessageBoxButton.OK,
                                           MessageBoxImage.Error);
            }
        }

    }
}