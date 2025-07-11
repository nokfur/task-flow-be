using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Services.Utils
{
    public class PasswordHasher
    {
        // The number of iterations for PBKDF2
        private const int IterationCount = 10000;

        // Size of the salt in bytes
        private const int SaltSize = 16;

        // Size of the hash in bytes
        private const int HashSize = 32;

        private static string GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        private static string HashPassword(string password, string salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrEmpty(salt))
                throw new ArgumentNullException(nameof(salt));

            // Convert the salt from Base64 to bytes
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Hash the password using PBKDF2 with HMACSHA256
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: IterationCount,
                numBytesRequested: HashSize);

            // Convert the hash to Base64 for storage
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string storedPassword, string storedSalt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrEmpty(storedPassword))
                throw new ArgumentNullException(nameof(storedPassword));

            if (string.IsNullOrEmpty(storedSalt))
                throw new ArgumentNullException(nameof(storedSalt));

            // Hash the input password with the stored salt
            string computedHash = HashPassword(password, storedSalt);

            // Compare the computed hash with the stored hash
            return computedHash == storedPassword;
        }

        public static (string Hash, string Salt) HashNewPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            // Generate a new salt
            string salt = GenerateSalt();

            // Hash the password with the new salt
            string hash = HashPassword(password, salt);

            return (hash, salt);
        }
    }
}
