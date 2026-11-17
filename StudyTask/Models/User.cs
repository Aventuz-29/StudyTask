using System;

namespace StudyTask.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string GetDisplayName() => $"{FullName} ({Login})";

        public bool HasRole(string roleName) => Role != null && Role.Name == roleName;
    }
}