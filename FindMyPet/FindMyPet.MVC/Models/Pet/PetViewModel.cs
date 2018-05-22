using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Pet
{
    public class PetViewModel
    {
        public string Code { get; set; }

        [Required(ErrorMessage = "Nombre es requerido.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fecha Nacimiento es requerida.")]
        [DataType("Date")]
        [Display(Name = "Fecha Nacimiento")]
        public DateTime DateOfBirth { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}