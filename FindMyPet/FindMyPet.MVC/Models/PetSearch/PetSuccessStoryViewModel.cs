using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.PetSearch
{
    public class PetSuccessStoryViewModel
    {
        public string OwnerFullName { get; set; }
        public string OwnerProfileImageUrl { get; set; }
        public string PetName { get; set; }
        public string PetProfileImageUrl { get; set; }
        public string FoundComment { get; set; }
        public string LostDateTime { get; set; }
        public string FoundDateTime { get; set; }
    }
}