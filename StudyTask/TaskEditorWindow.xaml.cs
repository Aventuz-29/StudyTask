using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using StudyTask.Data;
using StudyTask.Models;

namespace StudyTask
{
    public partial class TaskEditorWindow : Window
    {
        private readonly AppDbContext _context = new AppDbContext();
        private readonly Curator _curator;

        /// <summary>true, если задача сохранена — тогда список в CuratorWindow перезагрузится.</summary>
        public bool TaskCreated { get; private set; }

        public TaskEditorWindow(Curator curator)
        {
            InitializeComponent();
            _curator = curator;
            LoadTaskTypes();
            LoadGroups();
        }

        private void LoadTaskTypes()
        {
            List<TaskType> types = Enum.GetValues(typeof(TaskType))
                                       .Cast<TaskType>()
                                       .ToList();

            TaskTypeComboBox.ItemsSource = types;
            TaskTypeComboBox.SelectedIndex = 0;
        }

        private void LoadGroups()
        {
            // группы, которые ведёт текущий куратор
            var groups = _context.Groups
                                 .Where(g => g.CuratorId == _curator.Id)
                                 .OrderBy(g => g.Name)
                                 .ToList();

            GroupComboBox.ItemsSource = groups;
            GroupComboBox.DisplayMemberPath = nameof(Group.Name);

            if (groups.Count > 0)
                GroupComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();

            // 1. Проверка полей
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Введите название задачи.", "Задача",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Введите описание задачи.", "Задача",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DeadlinePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дедлайн.", "Задача",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!(TaskTypeComboBox.SelectedItem is TaskType taskType))
            {
                MessageBox.Show("Выберите тип задачи.", "Задача",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!(GroupComboBox.SelectedItem is Group group))
            {
                MessageBox.Show("Выберите группу. Если список пуст — за этой учётной записью не закреплена ни одна группа.",
                                "Задача",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // 2. Дедлайн: дата из DatePicker + время 23:59
            DateTime deadline = DeadlinePicker.SelectedDate.Value.Date
                .AddHours(23)
                .AddMinutes(59);

            try
            {
                // 3. Куратор из контекста этого окна (переданный объект — из уже закрытого контекста)
                Curator trackedCurator = _context.Users
                    .OfType<Curator>()
                    .First(u => u.Id == _curator.Id);

                // 4. Создание задачи через фабрику
                Models.TaskFactory factory = new Models.TaskFactory();
                TaskEntity task = factory.CreateTask(
                    title,
                    description,
                    taskType,
                    deadline,
                    trackedCurator,
                    group);

                // 5. Сохранение
                _context.Tasks.Add(task);
                _context.SaveChanges();

                TaskCreated = true;

                MessageBox.Show("Задача сохранена.", "Задача",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось сохранить задачу:\n" + ex.Message,
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