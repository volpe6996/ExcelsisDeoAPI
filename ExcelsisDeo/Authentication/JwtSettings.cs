namespace ExcelsisDeo.Authentication
{
    public class JwtSettings
    {
        internal const string SectionName = "JwtSettings";

        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public required string SecureKey { get; init; }
        public required int AccessTokenExpireMinutes { get; init; } = 15;
    }
}
