using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Pet
{
    public class PetAlertViewModel
    {
        public int OwnerId { get; set; }
        public string PetCode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Commets { get; set; }
        public int Type { get; set; }
        public bool MakeItPublic { get; set; }
    }
}