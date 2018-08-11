using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Admin
{
    public class SharePetEmail : BaseEmail
    {
        public string PetName { get; set; }
        public string SharePetLink { get; set; }
    }
}