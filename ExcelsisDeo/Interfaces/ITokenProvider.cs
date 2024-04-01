using ExcelsisDeo.Persistence.Entities;

namespace ExcelsisDeo.Interfaces
{
    public interface ITokenProvider
    {
        public string GetAccessToken(User user);
    }
}
