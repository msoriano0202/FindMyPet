namespace FindMyPet.MVC.Models.Admin
{
    public class BaseEmail : Postal.Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
    }
}