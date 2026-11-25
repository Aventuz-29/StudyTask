using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StudyTask.Data;
using StudyTask.Infrastructure;
using StudyTask.Models;

namespace StudyTask
{
    public partial class RegisterWindow : Window
    {
        // один контекст на время жизни окна: роли и группы остаются отслеживаемыми,
        // а новый пользователь добавляется в тот же контекст
        private readonly AppDbContext _context = new AppDbContext();

        public RegisterWindow()
        {
            InitializeComponent();
            LoadRoles();
            LoadGroups();
        }

        private void LoadRoles()
        {
            List<Role> roles = _context.Roles
                                       .OrderBy(r => r.Id)
                                       .ToList();

            RoleComboBox.ItemsSource = roles;
            RoleComboBox.DisplayMemberPath = nameof(Role.Name);
            RoleComboBox.SelectedValuePath = nameof(Role.Id);
            RoleComboBox.SelectedIndex = -1;
        }

        private void LoadGroups()
        {
            List<Group> groups = _context.Groups
                                         .OrderBy(g => g.Name)
                                         .ToList();

            GroupComboBox.ItemsSource = groups;
            GroupComboBox.DisplayMemberPath = nameof(Group.Name);
            GroupComboBox.SelectedValuePath = nameof(Group.Id);
            GroupComboBox.SelectedIndex = -1;
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // RoleId = 2 — Student
            bool isStudent = RoleComboBox.SelectedValue is int roleId && roleId == 2;

            GroupComboBox.IsEnabled = isStudent;

            if (!isStudent)
                GroupComboBox.SelectedIndex = -1;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text.Trim();
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // 1. Заполненность полей
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните ФИО, логин и пароль.",
                                "Регистрация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // 2. Совпадение паролей
            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.",
                                "Регистрация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // 3. Выбрана роль
            if (!(RoleComboBox.SelectedItem is Role selectedRole))
            {
                MessageBox.Show("Выберите роль.",
                                "Регистрация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // 4. Для студента обязательна группа
            int? groupId = null;

            if (selectedRole.Id == 2)
            {
                if (!(GroupComboBox.SelectedItem is Group selectedGroup))
                {
                    MessageBox.Show("Для студента выберите группу.",
                                    "Регистрация",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                groupId = selectedGroup.Id;
            }

            // 5. Логин должен быть свободен (проверка ДО вызова фабрики)
            if (_context.Users.Any(u => u.Login == login))
            {
                MessageBox.Show("Логин '" + login + "' уже занят. Выберите другой.",
                                "Регистрация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 6. Создание пользователя через фабрику
                string passwordHash = PasswordHasher.HashPassword(password);

                UserFactory factory = new UserFactory();
                User newUser = factory.Create(selectedRole, login, passwordHash, fullName, groupId);

                // 7. Сохранение
                _context.Users.Add(newUser);
                _context.SaveChanges();

                MessageBox.Show("Пользователь '" + login + "' успешно зарегистрирован.",
                                "Регистрация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось сохранить пользователя:\n" + ex.Message,
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _context.Dispose();
            base.OnClosed(e);
        }
    }
}