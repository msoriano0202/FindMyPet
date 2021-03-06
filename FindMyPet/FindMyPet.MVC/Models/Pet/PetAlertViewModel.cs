﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindMyPet.MVC.Models.Pet
{
    public class PetAlertViewModel
    {
        public int OwnerId { get; set; }
        public string PetCode { get; set; }
        [Required]
        public float? Latitude { get; set; }
        [Required]
        public float? Longitude { get; set; }
        [AllowHtml]
        public string Commets { get; set; }
        public int Type { get; set; }
        public bool MakeItPublic { get; set; }
        public string StaticMapUrl { get; set; }
    }
}