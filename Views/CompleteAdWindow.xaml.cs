using System.Windows;

namespace AvitoMiniApp.Views
{
    public partial class CompleteAdWindow : Window
    {
        public decimal FinalPrice { get; private set; }

        public CompleteAdWindow()
        {
            InitializeComponent();
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(FinalPriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную сумму", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            FinalPrice = price;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
