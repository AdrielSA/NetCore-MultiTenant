namespace MultiTenant.Application.Options
{
    public class AuthOptions
    {
        public string SecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpirationTime { get; set; }
    }
}
