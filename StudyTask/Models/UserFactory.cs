using System;

namespace StudyTask.Models
{
    public class UserFactory
    {
        /// <summary>
        /// Фабрика пользователей: создаёт Curator, Student или Administrator по роли
        /// </summary>
        public User Create(Role role, string login, string passwordHash,
                           string fullName, int? groupId = null)
        {
            if (role is null)
                throw new ArgumentNullException(nameof(role));

            return role.Name switch
            {
                nameof(Curator) => new Curator
                {
                    Login = login,
                    PasswordHash = passwordHash,
                    FullName = fullName,
                    RoleId = role.Id
                },
                nameof(Student) => new Student
                {
                    Login = login,
                    PasswordHash = passwordHash,
                    FullName = fullName,
                    RoleId = role.Id,
                    GroupId = groupId
                },
                nameof(Administrator) => new Administrator
                {
                    Login = login,
                    PasswordHash = passwordHash,
                    FullName = fullName,
                    RoleId = role.Id
                },
                _ => throw new ArgumentException(
                        $"Unknown role '{role.Name}'.", nameof(role))
            };
        }
    }
}