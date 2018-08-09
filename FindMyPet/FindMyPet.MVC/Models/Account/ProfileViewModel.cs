using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Account
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "Nombre es requerido.")]
        public string FirstName { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "Apellido es requerido.")]
        public string LastName { get; set; }
        
        [MaxLength(15)]
        public string PhoneNumber1 { get; set; }

        [MaxLength(15)]
        public string PhoneNumber2 { get; set; }

        [MaxLength(100)]
        public string Address1 { get; set; }

        [MaxLength(100)]
        public string Address2 { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}