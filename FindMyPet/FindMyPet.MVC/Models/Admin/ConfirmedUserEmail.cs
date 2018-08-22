using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Admin
{
    public class ConfirmedUserEmail : BaseEmail
    {
        public string FullName { get; set; }
    }
}