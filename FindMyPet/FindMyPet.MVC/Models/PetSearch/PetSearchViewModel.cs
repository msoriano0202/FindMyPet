using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.PetSearch
{
    public enum SearchOnMapOptionEnum
    {
        All = 0,
        LastWeek = 1,
        LastMonth = 2,
        Custom = 3
    }

    public class PetSearchViewModel
    {
        public List<RadioButtonModel> Options { get; set; }
        //[Required(ErrorMessage = "Fecha Desde es requerida.")]
        public DateTime? From { get; set; }
        //[Required(ErrorMessage = "Fecha Hasta es requerida.")]
        public DateTime? To { get; set; }
        public int OptionSelected { get; set; }
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