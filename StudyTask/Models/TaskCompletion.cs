using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyTask.Models
{
    public class TaskCompletion
    {
        public int Id { get; set; }

        public int TaskId { get; set; }
        public TaskEntity Task { get; set; } = null!;

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public WorkStatus Status { get; set; } = WorkStatus.Active;

        public int? Grade { get; set; }
        public string? StudentComment { get; set; }
        public string? FileLink { get; set; }
        public string? CuratorComment { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }

    }
}