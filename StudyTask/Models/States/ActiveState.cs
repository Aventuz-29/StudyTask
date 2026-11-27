using System;

namespace StudyTask.Models.States
{
    /// <summary>
    /// Задание выдано, можно сдать
    /// </summary>
    public class ActiveState : CompletionStateBase
    {
        public override WorkStatus Status => WorkStatus.Active;
        public override string Name => "Active";
        public override bool CanSubmit => true;

        public override void Submit(TaskCompletion completion, string? comment, string? fileLink, DateTime now)
        {
            completion.Status = WorkStatus.Submitted;
            completion.StudentComment = comment;
            completion.FileLink = fileLink;
            completion.SubmittedAt = now;
        }

        public override void RefreshOverdue(TaskCompletion completion, DateTime now)
        {
            if (completion.Task != null && completion.Task.Deadline < now)
                completion.Status = WorkStatus.Overdue;
        }
    }
}