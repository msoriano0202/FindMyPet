using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Pet
{
    public class PetImageViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ImageUrl { get; set; }
        public bool IsProfileImage { get; set; }
    }
}