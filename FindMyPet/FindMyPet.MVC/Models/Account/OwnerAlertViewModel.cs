﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Account
{
    public class OwnerAlertViewModel
    {
        public string AlertCode { get; set; }
        public int? PetId { get; set; }
        public string PetCode { get; set; }
        public string PetName { get; set; }
        public string PetProfileImageUrl { get; set; }
        public string LostDateTime { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string LostComment { get; set; }
    }
}