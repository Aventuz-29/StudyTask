using System;

namespace StudyTask.Models.States
{
    /// <summary>
    /// Работа на проверке: можно принять или вернуть на доработку
    /// </summary>
    public class SubmittedState : CompletionStateBase
    {
        private const int MinGrade = 1;
        private const int MaxGrade = 5;   

        public override WorkStatus Status => WorkStatus.Submitted;
        public override string Name => "Submitted";
        public override bool CanReview => true;

        public override void Accept(TaskCompletion completion, int grade, string? comment, DateTime now)
        {
            if (grade < MinGrade || grade > MaxGrade)
                throw new ArgumentOutOfRangeException(nameof(grade),
                    $"Grade must be between {MinGrade} and {MaxGrade}.");

            completion.Status = WorkStatus.Accepted;
            completion.Grade = grade;
            completion.CuratorComment = comment;
            completion.ReviewedAt = now;
        }

        public override void ReturnForRevision(TaskCompletion completion, string? comment, DateTime now)
        {
            completion.Status = WorkStatus.ReturnedForRevision;
            completion.CuratorComment = comment;
            completion.ReviewedAt = now;
        }
    }
}