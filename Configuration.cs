namespace Blog;

public static class Configuration
{
    //Token
    public static string JwtKey = "Lcxa8gQ935OJVN6nL4fkOyl4tKoP4wjd";
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "MyEggs_N6nL4fkOyl4tKoP==";
    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string userName { get; set; }
        public string Password { get; set; }
    }
}