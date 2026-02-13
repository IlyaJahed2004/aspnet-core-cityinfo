namespace CityInfo.Api.Services
{
    public interface IMailService
    {
        void send(string subject, string message);
    }
}