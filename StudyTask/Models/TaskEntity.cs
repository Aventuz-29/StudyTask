using System;
using System.Collections.Generic;

namespace StudyTask.Models
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskType Type { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CuratorId { get; set; }
        public Curator Author { get; set; } = null!;

        public int GroupId { get; set; }
        public Group TargetGroup { get; set; } = null!;

        public int? TargetStudentId { get; set; }
        public Student? TargetStudent { get; set; }

        public ICollection<TaskCompletion> Completions { get; set; } = new List<TaskCompletion>();

        public bool IsOverdue(DateTime now) => Deadline < now;
        public bool IsPersonal() => TargetStudentId.HasValue;
        public void Publish() => IsPublished = true;
    }
}