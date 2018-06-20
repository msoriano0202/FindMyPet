using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.PetSearch
{
    public class PetPublicProfileViewModel
    {
        public PetInfoViewModel PetInfo { get; set; }
        public List<OwnerInfoViewModel> OwnersInfo { get; set; }
    }

    public class PetInfoViewModel
    {
        public string Name { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Description { get; set; }
        public string LostComment { get; set; }
        public string LostDateTime { get; set; }
        public List<string> Images { get; set; }
    }

    public class OwnerInfoViewModel
    {
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }
}