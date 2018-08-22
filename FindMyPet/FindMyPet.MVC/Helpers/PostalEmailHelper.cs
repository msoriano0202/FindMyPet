using FindMyPet.MVC.Models.Admin;
using System.Configuration;
using System.Threading.Tasks;

namespace FindMyPet.MVC.Helpers
{
    public interface IPostalEmailHelper
    {
        Task<AccountRegisteredEmail> SendConfirmationEmailAsync(string toEmail, string fullName, string password, string callbackUrl, bool sendEmail = true);
        Task<ResendConfirmationEmail> ResendConfirmationEmailAsync(string toEmail, string callbackUrl, bool sendEmail = true);
        Task<ResetPasswordEmail> SendResetPasswordEmailAsync(string toEmail, string fullName, string callbackUrl, bool sendEmail = true);
        Task<SharePetEmail> SendShareEmailEmailAsync(string toEmail, string PetName, string callbackUrl, bool sendEmail = true);
        Task<ConfirmedUserEmail> SendUserConfirmedAdminEmailAsync(string toEmail, string fullName, bool sendEmail = true);
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

        public async Task<ResetPasswordEmail> SendResetPasswordEmailAsync(string toEmail, string fullName, string callbackUrl, bool sendEmail = true)
        {
            var email = new ResetPasswordEmail
            {
                To = toEmail,
                FullName = fullName,
                From = ConfigurationManager.AppSettings["FromEmail"].ToString(),
                Subject = ConfigurationManager.AppSettings["ResetPasswordEmailSubject"].ToString(),
                ResetPasswordLink = callbackUrl
            };

            if (sendEmail)
                await email.SendAsync();

            return email;
        }

        public async Task<SharePetEmail> SendShareEmailEmailAsync(string toEmail, string PetName, string callbackUrl, bool sendEmail = true)
        {
            var email = new SharePetEmail
            {
                To = toEmail,
                PetName = PetName,
                From = ConfigurationManager.AppSettings["FromEmail"].ToString(),
                Subject = ConfigurationManager.AppSettings["SharePetEmailSubject"].ToString(),
                SharePetLink = callbackUrl
            };

            if (sendEmail)
                await email.SendAsync();

            return email;
        }

        public async Task<ConfirmedUserEmail> SendUserConfirmedAdminEmailAsync(string toEmail, string fullName, bool sendEmail = true)
        {
            var email = new ConfirmedUserEmail
            {
                To = toEmail,
                FullName = fullName,
                From = ConfigurationManager.AppSettings["FromEmail"].ToString(),
                Subject = "New User Confirmed Register"
            };

            if (sendEmail)
                await email.SendAsync();

            return email;
        }
    }
}