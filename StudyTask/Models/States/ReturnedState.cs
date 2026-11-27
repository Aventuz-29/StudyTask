using System;

namespace StudyTask.Models.States
{
    /// <summary>
    /// Работа возвращена на доработку - можно сдать снова
    /// </summary>
    public class ReturnedState : CompletionStateBase
    {
        public override WorkStatus Status => WorkStatus.ReturnedForRevision;
        public override string Name => "ReturnedForRevision";
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