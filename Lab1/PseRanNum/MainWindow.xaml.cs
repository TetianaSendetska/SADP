using PseRanNum.Handlers;
using System.Windows;
using System.IO;


namespace PseRanNum
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            tblockResult.Text = string.Empty;

            string iter = tboxIter.Text;
            string m = tboxModM.Text;
            string a = tboxMulA.Text;
            string c = tboxIncC.Text;
            string x0 = tboxInitValX.Text;

            string validation = Validator.Validate(iter, m, a, c, x0);
            if (validation != string.Empty)
            {
                tblockResult.Text = validation;
                return;
            }

            long[] result = Generator.Generate(Int64.Parse(iter), Int64.Parse(m), Int64.Parse(a), Int64.Parse(c), Int64.Parse(x0));

            tblockResult.Text = string.Join(", ", result);
            tblockResult.Text += "\nПовторенн знайдено на індексі " + Generator.CheckPeriod(result);
        }

        private void btnWriteToFile_Click(object sender, RoutedEventArgs e)
        {
            string createText = "Generated numbers are\n" + tblockResult.Text;
            File.WriteAllText("result.txt", createText);
            MessageBox.Show("Запис у файл відбувся успішно");
        }
    }
}