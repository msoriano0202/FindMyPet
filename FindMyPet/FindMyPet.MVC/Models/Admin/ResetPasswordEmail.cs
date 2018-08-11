using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Admin
{
    public class ResetPasswordEmail : BaseEmail
    {
        public string FullName { get; set; }
        public string ResetPasswordLink { get; set; }
    }
}