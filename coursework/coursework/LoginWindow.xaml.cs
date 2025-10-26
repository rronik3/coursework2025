using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;

namespace coursework
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly RecipeContext _context;

        public LoginWindow()
        {
            InitializeComponent();
            _context = new RecipeContext();
            _context.Database.EnsureCreated();
        }
   
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = UsernameBox.Text.Trim();
                string password = PasswordBox.Password;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Заповніть усі поля.", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user != null)
                {
                    Application.Current.Properties["CurrentUserId"] = user.Id;

                    var main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неправильне ім'я користувача або пароль.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заповніть усі поля.", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_context.Users.Any(u => u.Username == username))
            {
                MessageBox.Show("Користувач з таким іменем вже існує.", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = new User { Username = username, Password = password };
            _context.Users.Add(user);
            _context.SaveChanges();

            MessageBox.Show("Реєстрація успішна. Тепер увійдіть.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
