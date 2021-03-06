﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace FindMyPet.MVC.Models.Home
{
    public class PetPublicAlertViewModel
    {
        public int? OwnerId { get; set; }

        [Required(ErrorMessage = "Seleccione un tipo de Alerta.")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Seleccione un Tipo de Alerta.")]
        public int SelectedAlertTypeId { get; set; }

        public IEnumerable<SelectListItem> AlertTypes { get; set; }

        [Required]
        public IEnumerable<HttpPostedFileBase> Images { get; set; }

        [Required]
        public float? Latitude { get; set; }

        [Required]
        public float? Longitude { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Comentarios es requerido.")]
        public string Commets { get; set; }
        public string StaticMapUrl { get; set; }
    }
}