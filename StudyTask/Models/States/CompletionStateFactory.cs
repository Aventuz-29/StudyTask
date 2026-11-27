using System;

namespace StudyTask.Models.States
{
    /// <summary>
    /// Выдаёт класс состояния 
    /// </summary>
    public static class CompletionStateFactory
    {
        private static readonly ICompletionState Active = new ActiveState();
        private static readonly ICompletionState Submitted = new SubmittedState();
        private static readonly ICompletionState Accepted = new AcceptedState();
        private static readonly ICompletionState Returned = new ReturnedState();
        private static readonly ICompletionState Overdue = new OverdueState();

        public static ICompletionState Create(WorkStatus status) => status switch
        {
            WorkStatus.Active => Active,
            WorkStatus.Submitted => Submitted,
            WorkStatus.Accepted => Accepted,
            WorkStatus.ReturnedForRevision => Returned,
            WorkStatus.Overdue => Overdue,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown work status.")
        };
    }
}