namespace CityInfo.Api.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailto = string.Empty;
        private string _mailfrom = string.Empty;

        public LocalMailService(IConfiguration configuration)
        {
            _mailto = configuration["mailSettings:mailToAddress"];
            _mailfrom = configuration["mailSettings:mailFromAddress"];
        }
        public void send(string subject, string message)
        {
            //send mail - output to console window
            Console.WriteLine($"Mail from {_mailfrom} to {_mailto}, with {nameof(LocalMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message : {message}");
        }
    }
}
