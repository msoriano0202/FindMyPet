namespace FindMyPet.MVC.Models.Admin
{
    public class ResendConfirmationEmail : BaseEmail
    {
        public string ConfirmationLink { get; set; }
    }
}