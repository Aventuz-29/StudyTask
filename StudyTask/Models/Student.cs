using System.Collections.Generic;

namespace StudyTask.Models
{
    public class Student : User
    {
        public int? GroupId { get; set; }
        public Group? Group { get; set; }

        public ICollection<TaskCompletion> Completions { get; set; } = new List<TaskCompletion>();

        public bool IsEnrolled() => GroupId.HasValue;
    }
}