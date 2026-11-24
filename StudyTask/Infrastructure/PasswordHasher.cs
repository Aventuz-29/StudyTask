using System;
using System.Security.Cryptography;
using System.Text;

namespace StudyTask.Infrastructure
{
    public static class PasswordHasher
    {
        /// <summary>SHA-256</summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            byte[] bytes = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToHexString(hash);
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
                return false;

            string inputHash = HashPassword(password);
            return string.Equals(inputHash, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}