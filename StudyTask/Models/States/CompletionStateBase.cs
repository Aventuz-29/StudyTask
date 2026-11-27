using System;

namespace StudyTask.Models.States
{
    /// <summary>
    /// База состояний: по умолчанию все действия запрещены
    /// </summary>
    public abstract class CompletionStateBase : ICompletionState
    {
        public abstract WorkStatus Status { get; }
        public abstract string Name { get; }

        public virtual bool CanSubmit => false;
        public virtual bool CanReview => false;

        public virtual void Submit(TaskCompletion completion, string? comment, string? fileLink, DateTime now)
            => throw NotAllowed("submit");

        public virtual void Accept(TaskCompletion completion, int grade, string? comment, DateTime now)
            => throw NotAllowed("accept");

        public virtual void ReturnForRevision(TaskCompletion completion, string? comment, DateTime now)
            => throw NotAllowed("return for revision");

        public virtual void RefreshOverdue(TaskCompletion completion, DateTime now)
        {
            }

        protected InvalidOperationException NotAllowed(string action)
            => new InvalidOperationException(
                $"Action '{action}' is not allowed when the task is in state '{Name}'.");
    }
}