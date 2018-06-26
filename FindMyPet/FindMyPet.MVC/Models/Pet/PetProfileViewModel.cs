using FindMyPet.Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace FindMyPet.MVC.Models.Pet
{
    public class PetProfileViewModel
    {
        public string Code { get; set; }

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