using System.Windows;
using AvitoMiniApp.Services;

namespace AvitoMiniApp.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService authService;

        public LoginWindow()
        {
            InitializeComponent();
            authService = new AuthService();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text.Trim();
            var password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = authService.Authenticate(login, password);
            if (user != null)
            {
                CurrentUser.User = user;
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
