using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using StudyTask.Data;
using StudyTask.Infrastructure;
using StudyTask.Models;

namespace StudyTask
{
    /// <summary>
    /// Окно входа: проверяет логин/пароль и открывает кабинет по роли
    /// </summary>
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
                using (AppDbContext context = new AppDbContext())
                {
                    User? user = context.Users
                        .Include(u => u.Role)
                        .FirstOrDefault(u => u.Login == login);

                    if (user == null || !user.IsActive)
                    {
                        ShowLoginError();
                        return;
                    }

                    if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
                    {
                        ShowLoginError();
                        return;
                    }

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

        /// <summary>
        /// Открывает кабинет по RoleId: 1 — куратор, 2 — студент, 3 — админ
        /// </summary>
        private void OpenWindowForRole(User user)
        {
            Window? targetWindow = null;

            switch (user.RoleId)
            {
                case 1 when user is Curator curator:
                    targetWindow = new CuratorWindow(curator);
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

        /// <summary>
        /// Одинаковое сообщение для «нет логина» и «неверный пароль»
        /// </summary>
        private static void ShowLoginError()
        {
            MessageBox.Show("Неверный логин или пароль",
                            "Ошибка входа",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow
            {
                Owner = this
            };

            Hide();                     
            try
            {
                registerWindow.ShowDialog();
            }
            finally
            {
                Show();                 
                PasswordBox.Clear();    
            }
        }
    }
}