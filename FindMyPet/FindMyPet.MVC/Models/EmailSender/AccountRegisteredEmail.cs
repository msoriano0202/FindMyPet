﻿namespace FindMyPet.MVC.Models.EmailSender
{
    public class AccountRegisteredEmail : BaseEmail
    {
        public string FullName { get; set; }
        public string LoginUser { get; set; }
        public string LoginPassword { get; set; }
        public string ConfirmationLink { get; set; }
    }
}