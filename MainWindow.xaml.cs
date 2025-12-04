using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AvitoMiniApp.Models;
using AvitoMiniApp.Services;

namespace AvitoMiniApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly AdService adService;
        private readonly CategoryService categoryService;
        private ObservableCollection<Category> categories;

        public MainWindow()
        {
            InitializeComponent();
            adService = new AdService();
            categoryService = new CategoryService();

            UserNameText.Text = CurrentUser.User?.Login ?? "User";

            // Change button text for admin
            if (CurrentUser.User?.Login == "admin")
            {
                MyAdsButton.Content = "Ad Management";
            }

            LoadCategories();
            LoadAllAds();
        }

        private void LoadCategories()
        {
            categories = categoryService.GetAllCategories();
            CategoryFilterComboBox.ItemsSource = categories;
            CategoryFilterComboBox.DisplayMemberPath = "Name";
            CategoryFilterComboBox.SelectedValuePath = "CategoryId";
        }

        private void LoadAllAds()
        {
            var ads = adService.GetAllAds();
            AdsListControl.ItemsSource = ads;
        }

        private void LoadUserAds()
        {
            if (CurrentUser.User == null) return;

            // For admin, show all ads (Ad Management)
            var ads = CurrentUser.User.Login == "admin"
                ? adService.GetAllAds()
                : adService.GetUserAds(CurrentUser.User.UserId);

            MyAdsListControl.ItemsSource = ads;
        }

        private void LoadCompletedAds()
        {
            if (CurrentUser.User == null) return;
            var completedAds = adService.GetCompletedAds(CurrentUser.User.UserId);
            CompletedAdsListControl.ItemsSource = completedAds;

            TotalCompletedText.Text = completedAds.Count.ToString();
            var totalProfit = completedAds.Sum(a => a.Profit);
            TotalProfitText.Text = $"{totalProfit:N0} ₽";
        }

        private void AllAdsButton_Click(object sender, RoutedEventArgs e)
        {
            AllAdsView.Visibility = Visibility.Visible;
            MyAdsView.Visibility = Visibility.Collapsed;
            CompletedView.Visibility = Visibility.Collapsed;
            LoadAllAds();
        }

        private void MyAdsButton_Click(object sender, RoutedEventArgs e)
        {
            AllAdsView.Visibility = Visibility.Collapsed;
            MyAdsView.Visibility = Visibility.Visible;
            CompletedView.Visibility = Visibility.Collapsed;
            LoadUserAds();
        }

        private void CompletedButton_Click(object sender, RoutedEventArgs e)
        {
            AllAdsView.Visibility = Visibility.Collapsed;
            MyAdsView.Visibility = Visibility.Collapsed;
            CompletedView.Visibility = Visibility.Visible;
            LoadCompletedAds();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchTextBox.Text.Trim();
            int? categoryId = CategoryFilterComboBox.SelectedValue as int?;

            if (string.IsNullOrEmpty(searchText) && !categoryId.HasValue)
            {
                LoadAllAds();
                return;
            }

            var ads = adService.SearchAds(searchText, categoryId);
            AdsListControl.ItemsSource = ads;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                LoadAllAds();
            }
        }

        private void CategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryFilterComboBox.SelectedValue != null)
            {
                SearchButton_Click(sender, null);
            }
        }

        private void CreateAdButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AdEditWindow(categories);
            if (dialog.ShowDialog() == true)
            {
                LoadUserAds();
            }
        }

        private void EditAdButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var ad = button?.Tag as Ad;
            if (ad == null) return;

            var dialog = new AdEditWindow(categories, ad);
            if (dialog.ShowDialog() == true)
            {
                LoadUserAds();
            }
        }

        private void DeleteAdButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var adId = button?.Tag as int?;
            if (!adId.HasValue) return;

            var result = MessageBox.Show(
                "Вы уверены, что хотите удалить объявление?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    adService.DeleteAd(adId.Value);
                    LoadUserAds();
                    MessageBox.Show("Объявление удалено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Ошибка при удалении объявления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CompleteAdButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var adId = button?.Tag as int?;
            if (!adId.HasValue) return;

            var dialog = new CompleteAdWindow();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    adService.CompleteAd(adId.Value, dialog.FinalPrice);
                    LoadUserAds();
                    MessageBox.Show("Объявление завершено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Ошибка при завершении объявления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.User = null;
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
