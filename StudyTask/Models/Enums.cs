using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyTask.Models
{
    /// <summary>
    /// Тип задания
    /// </summary>
    public enum TaskType
    {
        Homework = 1,
        Project = 2,
        Test = 3,
        Other = 4
    }

    /// <summary>
    /// Статус выполнения задания. В БД хранится как int
    /// </summary>
    public enum WorkStatus
    {
        Active = 1,
        Submitted = 2,
        Accepted = 3,
        ReturnedForRevision = 4,
        Overdue = 5
    }
}
