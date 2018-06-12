using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Pet
{
    public class PetShareViewModel
    {
        public string PetCode { get; set; }

        [Required(ErrorMessage = "Correo es requerido.")]
        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Por favor, ingrese un correo valido.")]
        public string Email { get; set; }
        public List<PetSharedOwnerViewModel> Owners { get; set; }
    }

    public class PetSharedOwnerViewModel
    {
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime RegisteredDate { get; set; }
    }
}