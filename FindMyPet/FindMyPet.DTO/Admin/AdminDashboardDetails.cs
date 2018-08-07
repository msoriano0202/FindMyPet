using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Admin
{
    public class AdminDashboardDetails
    {
        public int RegisteredAccounts { get; set; }
        public int RegisteredPets { get; set; }
        public int LostPets { get; set; }
        public int FoundPets { get; set; }
        public int AlertsToReview { get; set; }
        public int CommentsToApprove { get; set; }
        public int SuccessStories { get; set; }
    }
}
