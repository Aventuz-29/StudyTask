using System;

namespace StudyTask.Models.States
{
    /// <summary>
    /// Время конца сроков работы прошло, но сдать работу можно
    /// </summary>
    public class OverdueState : CompletionStateBase
    {
        public override WorkStatus Status => WorkStatus.Overdue;
        public override string Name => "Overdue";
        public override bool CanSubmit => true;

        public override void Submit(TaskCompletion completion, string? comment, string? fileLink, DateTime now)
        {
            completion.Status = WorkStatus.Submitted;
            completion.StudentComment = comment;
            completion.FileLink = fileLink;
            completion.SubmittedAt = now;
        }
    }
}