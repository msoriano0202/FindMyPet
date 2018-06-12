using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Configuration;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FindMyPet.MVC.Helpers
{
    public class Message
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public interface IEmailHelper
    {
        void SendEmail(IdentityMessage identityMessage);
        void SendEmailSharePet(string email, string petName, string callbackUrl);
    }

    public class EmailHelper : IEmailHelper
    {
        public void SendEmail(IdentityMessage identityMessage)
        {
            var message = new Message();
            message.From = ConfigurationManager.AppSettings["FromEmail"].ToString();
            message.To = identityMessage.Destination;
            message.Subject = identityMessage.Subject;
            message.Body = identityMessage.Body;

            SendEmail(message);
        }

        public void SendEmailSharePet(string email, string petName, string body)
        {
            var message = new Message();
            message.From = ConfigurationManager.AppSettings["FromEmail"].ToString();
            message.To = email;
            message.Subject = $"Alerta Mascota: Compartir Mascota - {petName}";
            message.Body = body;

            SendEmail(message);
        }

        private void SendEmail(Message message)
        {
            MailMessage msg = new MailMessage(message.From, message.To, message.Subject, message.Body);

            var client = new SmtpClient();
            client.Send(msg);
        }

        #region InitialTest

        //public static void sendMail(Message message)
        //{
        //    //var client = new SmtpClient("smtp.gmail.com", 587)
        //    //{
        //    //    Credentials = new NetworkCredential("msoriano0202@gmail.com", "P@$$w0rd!!!"),
        //    //    EnableSsl = true
        //    //};
        //    //client.Send("msoriano0202@gmail.com", "miguel_mst@hotmail.com", "test", "testbody");


        //    message.Subject = "This is the subject";
        //    message.Body = "This is the body";
        //    message.Destination = "miguel_mst@hotmail.com";

        //    #region formatter
        //    string text = string.Format("Please click on this link to {0}: {1}", message.Subject, message.Body);
        //    string html = "Please confirm your account by clicking this link: <a href=\"" + message.Body + "\">link</a><br/>";

        //    html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser:" + message.Body);
        //    #endregion

        //    MailMessage msg = new MailMessage("testemail@gmail.com", message.Destination, message.Subject, text);
        //    msg.To.Add("msoriano0202@gmail.com");
        //    //msg.From = new MailAddress("miguel_mst@hotmail.com");
        //    //msg.To.Add(new MailAddress(message.Destination));
        //    //msg.Subject = message.Subject;
        //    msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
        //    msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

        //    //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
        //    //NetworkCredential credentials = new NetworkCredential("msoriano0202.gmail.com", "P@$$w0rd!!!");
        //    //smtpClient.Credentials = credentials;
        //    //smtpClient.EnableSsl = true;
        //    //smtpClient.Send(msg);

        //    //var client = new SmtpClient("smtp.gmail.com", 587)
        //    //{
        //    //    Credentials = new NetworkCredential("msoriano0202@gmail.com", "P@$$w0rd!!!"),
        //    //    EnableSsl = true
        //    //};

        //    var client = new SmtpClient();

        //    //client.Send(msg.From.Address, message.Destination, message.Subject, text);
        //    client.Send(msg);
        //}

        #endregion

    }
}