using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using StudyTask.Data;
using StudyTask.Infrastructure;
using StudyTask.Models;

namespace StudyTask
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // 1. Проверка заполнения полей
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль.",
                                "Вход",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 2. Поиск пользователя в базе вместе с ролью
                using (AppDbContext context = new AppDbContext())
                {
                    User? user = context.Users
                        .Include(u => u.Role)
                        .FirstOrDefault(u => u.Login == login);

                    // 3. Пользователь не найден
                    if (user == null || !user.IsActive)
                    {
                        ShowLoginError();
                        return;
                    }

                    // 4. Проверка пароля
                    if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
                    {
                        ShowLoginError();
                        return;
                    }

                    // 5. Открытие окна по роли
                    OpenWindowForRole(user);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось подключиться к базе данных:\n" + ex.Message,
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void OpenWindowForRole(User user)
        {
            // RoleId: 1 = Curator, 2 = Student, 3 = Administrator (см. Seed Data в AppDbContext)
            Window? targetWindow = null;

            switch (user.RoleId)
            {
                case 1:
                    targetWindow = new CuratorWindow();
                    break;
                case 2:
                    targetWindow = new StudentWindow();
                    break;
                case 3:
                    targetWindow = new AdminWindow();
                    break;
                default:
                    MessageBox.Show("Для пользователя не настроена роль: " +
                                    (user.Role != null ? user.Role.Name : user.RoleId.ToString()),
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
            }

            targetWindow.Show();
            Close();
        }

        private static void ShowLoginError()
        {
            MessageBox.Show("Неверный логин или пароль",
                            "Ошибка входа",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Регистрация будет реализована позже",
                            "StudyTask",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
    }
}