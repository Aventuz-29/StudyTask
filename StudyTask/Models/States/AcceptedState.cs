namespace StudyTask.Models.States
{
    /// <summary>
    /// Работа принята — действий больше нет
    /// </summary>
    public class AcceptedState : CompletionStateBase
    {
        public override WorkStatus Status => WorkStatus.Accepted;
        public override string Name => "Accepted";
    }
}