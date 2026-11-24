using System;
using System.Collections.Generic;
using System.Linq;

namespace StudyTask.Models
{
    public class TaskFactory
    {
        public TaskEntity CreateTask(string title, string description, TaskType type,
                                     DateTime deadline, Curator author, Group group,
                                     Student? targetStudent = null)
        {
            return new TaskEntity
            {
                Title = title,
                Description = description,
                Type = type,
                Deadline = deadline,
                Author = author,
                CuratorId = author.Id,
                TargetGroup = group,
                GroupId = group.Id,
                TargetStudent = targetStudent,
                TargetStudentId = targetStudent?.Id,
                IsPublished = false
            };
        }

        /// <summary>Выполнения для списка студентов (начальное состояние — Active).</summary>
        public List<TaskCompletion> CreateCompletions(TaskEntity task, IEnumerable<Student> students)
        {
            return students.Select(s => new TaskCompletion
            {
                Task = task,
                Student = s,
                Status = WorkStatus.Active      
            }).ToList();
        }

        /// <summary>Выполнения для группы; если задание персональное — только для одного студента.</summary>
        public List<TaskCompletion> CreateCompletionsForGroup(TaskEntity task, Group group)
        {
            IEnumerable<Student> students = task.TargetStudent != null
                ? new[] { task.TargetStudent }
                : group.Students;

            return CreateCompletions(task, students);
        }
    }
}