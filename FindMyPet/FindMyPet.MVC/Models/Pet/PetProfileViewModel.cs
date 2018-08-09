using FindMyPet.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FindMyPet.MVC.Models.Pet
{
    public class PetProfileViewModel
    {
        public string Code { get; set; }

        [Required(ErrorMessage = "Sexo es requerido.")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Sexo es requerido.")]
        public int SelectedSexTypeId { get; set; }

        public IEnumerable<SelectListItem> SexTypes { get; set; }

        [Required(ErrorMessage = "Nombre es requerido.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fecha Nacimiento es requerida.")]
        [DataType("Date")]
        [Display(Name = "Fecha Nacimiento")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public string Status { get; set; }
        public PetStatusEnum StatusId { get; set; }

        public string ProfileImageUrl { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}