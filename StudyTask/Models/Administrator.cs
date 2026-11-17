namespace StudyTask.Models
{
    public class Administrator : User
    {
        public bool CanManageUsers() => true;
        public bool CanManageGroups() => true;
    }
}