using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.EmailSender
{
    public class ResendConfirmationEmail : BaseEmail
    {
        public string ConfirmationLink { get; set; }
    }
}