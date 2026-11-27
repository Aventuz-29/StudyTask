using System;
using System.ComponentModel.DataAnnotations.Schema;
using StudyTask.Models.States;

namespace StudyTask.Models
{
    /// <summary>
    /// Выполнение задания конкретным студентом: статус, оценка, комментарии
    /// </summary>
    public class TaskCompletion
    {
        public int Id { get; set; }

        public int TaskId { get; set; }
        public TaskEntity Task { get; set; } = null!;

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        /// <summary>
        /// Статус выполнения — единственное поле состояния, которое пишется в БД
        /// </summary>
        public WorkStatus Status { get; set; } = WorkStatus.Active;

        public int? Grade { get; set; }
        public string? StudentComment { get; set; }
        public string? FileLink { get; set; }
        public string? CuratorComment { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }

        [NotMapped]
        public ICompletionState CurrentState
        {
            get => CompletionStateFactory.Create(Status);
            set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));
                Status = value.Status;
            }
        }

        public void Submit(string? comment, string? fileLink, DateTime now)
            => CurrentState.Submit(this, comment, fileLink, now);

        public void Accept(int grade, string? comment, DateTime now)
            => CurrentState.Accept(this, grade, comment, now);

        public void ReturnForRevision(string? comment, DateTime now)
            => CurrentState.ReturnForRevision(this, comment, now);

        public void RefreshOverdue(DateTime now)
            => CurrentState.RefreshOverdue(this, now);
    }
}