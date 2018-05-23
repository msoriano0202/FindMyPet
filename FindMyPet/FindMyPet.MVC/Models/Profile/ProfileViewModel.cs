using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Profile
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es requerido.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Apellido es requerido.")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}