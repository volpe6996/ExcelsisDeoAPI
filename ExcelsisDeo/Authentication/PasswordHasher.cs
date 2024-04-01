using ExcelsisDeo.Interfaces;

namespace ExcelsisDeo.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(8);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }

        public bool ValidatePassword(string password, string hashToValidate)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashToValidate);
        }
    }
}
