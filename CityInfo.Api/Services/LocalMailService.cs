namespace CityInfo.Api.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailto = "admin@mycompany.com";
        private string _mailfrom = "Ilya@mycompany.com";

        public void send(string subject, string message)
        {
            //send mail - output to console window
            Console.WriteLine($"Mail from {_mailfrom} to {_mailto}, with {nameof(LocalMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message : {message}");
        }
    }
}
