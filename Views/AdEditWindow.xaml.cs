using System.Collections.ObjectModel;
using System.Windows;
using AvitoMiniApp.Models;
using AvitoMiniApp.Services;

namespace AvitoMiniApp.Views
{
    public partial class AdEditWindow : Window
    {
        private readonly AdService adService;
        private readonly ObservableCollection<Category> categories;
        private Ad? existingAd;

        public AdEditWindow(ObservableCollection<Category> categories, Ad? ad = null)
        {
            InitializeComponent();
            adService = new AdService();
            this.categories = categories;
            this.existingAd = ad;

            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.DisplayMemberPath = "Name";
            CategoryComboBox.SelectedValuePath = "CategoryId";

            if (ad != null)
            {
                TitleText.Text = "Редактирование объявления";
                TitleTextBox.Text = ad.Title;
                DescriptionTextBox.Text = ad.Description;
                PriceTextBox.Text = ad.Price.ToString();
                CityTextBox.Text = ad.City;
                ImagePathTextBox.Text = ad.ImagePath;
                CategoryComboBox.SelectedValue = ad.CategoryId;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Введите название объявления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CategoryComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите категорию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var ad = new Ad
                {
                    UserId = CurrentUser.User?.UserId ?? 0,
                    Title = TitleTextBox.Text.Trim(),
                    Description = DescriptionTextBox.Text.Trim(),
                    Price = price,
                    City = CityTextBox.Text.Trim(),
                    ImagePath = ImagePathTextBox.Text.Trim(),
                    CategoryId = (int)CategoryComboBox.SelectedValue
                };

                if (existingAd != null)
                {
                    ad.AdId = existingAd.AdId;
                    adService.UpdateAd(ad);
                    MessageBox.Show("Объявление обновлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    adService.AddAd(ad);
                    MessageBox.Show("Объявление создано", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DialogResult = true;
                Close();
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении объявления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
