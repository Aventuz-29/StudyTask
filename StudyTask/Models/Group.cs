using System.Collections.Generic;

namespace StudyTask.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public int Course { get; set; }

        public int? CuratorId { get; set; }
        public Curator? Curator { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();

        public void Enroll(Student student)
        {
            student.Group = this;
            if (Id != 0)
                student.GroupId = Id;

            if (!Students.Contains(student))
                Students.Add(student);
        }

        public void Exclude(Student student)
        {
            student.Group = null;
            student.GroupId = null;
            Students.Remove(student);
        }

        public void SetCurator(Curator curator)
        {
            Curator = curator;
            if (Id != 0)
                CuratorId = curator.Id;

            if (!curator.Groups.Contains(this))
                curator.Groups.Add(this);
        }
    }
}