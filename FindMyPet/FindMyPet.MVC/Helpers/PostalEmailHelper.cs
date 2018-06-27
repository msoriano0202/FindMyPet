using FindMyPet.MVC.Models.Admin;
using System.Configuration;
using System.Threading.Tasks;

namespace FindMyPet.MVC.Helpers
{
    public interface IPostalEmailHelper
    {
        Task<AccountRegisteredEmail> SendConfirmationEmailAsync(string toEmail, string fullName, string password, string callbackUrl, bool sendEmail = true);
        Task<ResendConfirmationEmail> ResendConfirmationEmailAsync(string toEmail, string callbackUrl, bool sendEmail = true);
    }

    public class PostalEmailHelper : IPostalEmailHelper
    {
        public async Task<AccountRegisteredEmail> SendConfirmationEmailAsync(string toEmail, string fullName, string password, string callbackUrl, bool sendEmail = true)
        {
            var email = new AccountRegisteredEmail
            {
                To = toEmail,
                From = ConfigurationManager.AppSettings["FromEmail"].ToString(),
                Subject = ConfigurationManager.AppSettings["ConfirmationEmailSubject"].ToString(),
                FullName = fullName,
                LoginUser = toEmail,
                LoginPassword = password,
                ConfirmationLink = callbackUrl
            };

            if(sendEmail)
                await email.SendAsync();

            return email;
        }

        public async Task<ResendConfirmationEmail> ResendConfirmationEmailAsync(string toEmail, string callbackUrl, bool sendEmail = true)
        {
            var email = new ResendConfirmationEmail
            {
                To = toEmail,
                From = ConfigurationManager.AppSettings["FromEmail"].ToString(),
                Subject = ConfigurationManager.AppSettings["ReSendConfirmationEmailSubject"].ToString(),
                ConfirmationLink = callbackUrl
            };

            if(sendEmail)
                await email.SendAsync();

            return email;
        }
    }
}