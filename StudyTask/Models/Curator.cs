using System.Collections.Generic;
using System.Linq;

namespace StudyTask.Models
{
    public class Curator : User
    {
        public ICollection<Group> Groups { get; set; } = new List<Group>();

        public bool Supervises(int groupId) => Groups.Any(g => g.Id == groupId);
    }
}