using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FindMyPet.MVC.Models.Admin
{
    public class EmailSenderViewModel
    {
        [Required(ErrorMessage = "Correo es requerido.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo valido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Seleccione un tipo de Correo.")]
        public int SelectedEmailTypeId { get; set; }
        public IEnumerable<SelectListItem> EmailTypes { get; set; }

        [Required(ErrorMessage = "Seleccione un tipo de Visualizacion.")]
        public int SelectedViewTypeId { get; set; }
        public IEnumerable<SelectListItem> ViewTypes { get; set; }
    }
}