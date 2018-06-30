using FindMyPet.MVC.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.PetSearch
{
    public enum LastAlertOptionEnum
    {
        LastWeek = 0,
        LastMonth = 1,
        Custom = 2
    }

    public class RadioButtonModel
    {
        public string ElementId { get; set; }
        public int Value { get; set; }
        public bool Selected { get; set; }
        public string DisplayText { get; set; }
    }

    public class PetLastAlertsPagedListViewModel
    {
        public List<RadioButtonModel> Options { get; set; }
        [Required(ErrorMessage = "Fecha Desde es requerida.")]
        public DateTime? From { get; set; }
        [Required(ErrorMessage = "Fecha Hasta es requerida.")]
        public DateTime? To { get; set; }
        public int OptionSelected { get; set; }
        public List<PetLastAlertDetailViewModel> Records { get; set; }
        public PaginationViewModel Pagination { get; set; }
    }
}