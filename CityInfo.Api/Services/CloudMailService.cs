namespace CityInfo.Api.Services
{
    public class CloudMailService : IMailService
    {
        private string _mailto = string.Empty;
        private string _mailfrom = string.Empty;
        public CloudMailService(IConfiguration configuration)
        {
            _mailto = configuration["mailSettings:mailToAddress"];
            _mailfrom = configuration["mailSettings:mailFromAddress"];
        }

        public void send(string subject, string message)
        {
            //send mail - output to console window
            Console.WriteLine($"Mail from {_mailfrom} to {_mailto}, with {nameof(CloudMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message : {message}");
        }
    }
}

