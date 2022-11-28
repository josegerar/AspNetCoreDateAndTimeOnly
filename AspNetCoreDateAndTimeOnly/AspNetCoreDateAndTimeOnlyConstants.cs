namespace AspNetCoreDateAndTimeOnly;
public struct AspNetCoreDateAndTimeOnlyConstants
{
    public struct Format
    {
        public const string DateOnly = "yyyy-MM-dd";
        public const string TimeOnly = "HH:mm:ss.FFFFFFF";
        public const string DateTime = "yyyy-MM-ddTHH:mm:ss.fff";
        public const string DateTimeOffset = "yyyy-MM-ddTHH:mm:ss.fff";
    }

    public struct JWT
    {
        public const string UserName = "username";
        public const string UserId = "user_id";
        public const string Email = "email";
        public const string Jti = "jti";
        public const string Role = "role";
        public const string Module = "module";
        public const string Exp = "exp";
        public const string AccessToken = "access_token";
    }

    public struct Scheme
    {
        public const string Bearer = "Bearer";
        public const string Oauth = "Oauth";
        public const string Basic = "Basic";
        public const string Digest = "Digest";
    }

    public struct Cors
    {
        public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    }
}