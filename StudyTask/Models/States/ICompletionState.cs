using System;

namespace StudyTask.Models.States
{
    public interface ICompletionState
    {
        /// <summary>
        /// Состояние выполнения задания
        /// </summary>
        WorkStatus Status { get; }
        string Name { get; }

        bool CanSubmit { get; }
        bool CanReview { get; }

        void Submit(TaskCompletion completion, string? comment, string? fileLink, DateTime now);
        void Accept(TaskCompletion completion, int grade, string? comment, DateTime now);
        void ReturnForRevision(TaskCompletion completion, string? comment, DateTime now);
        void RefreshOverdue(TaskCompletion completion, DateTime now);
    }
}