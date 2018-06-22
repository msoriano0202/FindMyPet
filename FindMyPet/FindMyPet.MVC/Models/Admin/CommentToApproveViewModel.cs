using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Admin
{
    public class CommentToApproveViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string OwnerName { get; set; }
        public string ownerProfileImageUrl { get; set; }
        public string PetName { get; set; }
        public string PetProfileImageUrl { get; set; }
        public string FoundComment { get; set; }
        public string FoundDateTime { get; set; }
    }
}