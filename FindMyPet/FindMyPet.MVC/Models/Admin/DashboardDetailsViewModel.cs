using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Admin
{
    public class DashboardDetailsViewModel
    {
        public int RegisteredAccounts { get; set; }
        public int RegisteredPets { get; set; }
        public int LostPets { get; set; }
        public int FoundPets { get; set; }
        public int CommentsToApprove { get; set; }
        public int SuccessStories { get; set; }
    }
}