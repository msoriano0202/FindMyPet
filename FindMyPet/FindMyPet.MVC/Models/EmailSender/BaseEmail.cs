using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.EmailSender
{
    public class BaseEmail : Postal.Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
    }
}