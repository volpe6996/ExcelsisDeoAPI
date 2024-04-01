namespace ExcelsisDeo.Interfaces
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);
        public bool ValidatePassword(string password, string hashToValidate);
    }
}
