namespace Infrastructure.Configurations
{
    public class JwtConfigurationOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int Expires { get; set; }
    }
}
