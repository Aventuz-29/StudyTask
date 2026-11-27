using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using StudyTask.Data;
using StudyTask.Models;

namespace StudyTask
{
    public partial class CuratorWindow : Window
    {
        private readonly Curator _curator;

        public CuratorWindow(Curator curator)
        {
            InitializeComponent();
            _curator = curator;
            TitleTextBlock.Text = "Задачи куратора: " + _curator.FullName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTasks();
        }

        private void LoadTasks()
        {
            try
            {
                using (AppDbContext context = new AppDbContext())
                {
                    List<TaskEntity> tasks = context.Tasks
                        .Include(t => t.TargetGroup)          // название группы
                        .Where(t => t.CuratorId == _curator.Id)
                        .OrderBy(t => t.Deadline)
                        .ToList();

                    TasksGrid.ItemsSource = tasks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить задачи:\n" + ex.Message,
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            TaskEditorWindow editor = new TaskEditorWindow(_curator)
            {
                Owner = this
            };

            editor.ShowDialog();

            if (editor.TaskCreated)
                LoadTasks();
        }
    }
}