namespace wppBacklog.Models
{
    public class AppConfigModel
    {
        public SendGridConfigModel SendGrid { get; set; }
        public CmsConfigModel Cms { get; set; }
        public string RootDomain { get; set; }
    }
    public class SendGridConfigModel
    {
        public string ApiKey { get; set; }
    }
    public class CmsConfigModel
    {
        public string Token { get; set; }
    }
}
