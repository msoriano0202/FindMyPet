using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.PetSearch
{
    public class PetSearchViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public List<PointAlertViewModel> Points { get; set; }
    }

    public class PointAlertViewModel
    {
        public int PetId { get; set; }
        public string PetCode { get; set; }
        public string PetName { get; set; }
        public string PetProfileImageUrl { get; set; }
        public string LostDateTime { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}