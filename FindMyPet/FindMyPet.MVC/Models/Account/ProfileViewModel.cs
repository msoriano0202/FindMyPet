using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindMyPet.MVC.Models.Account
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "Nombre es requerido.")]
        [AllowHtml]
        public string FirstName { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "Apellido es requerido.")]
        [AllowHtml]
        public string LastName { get; set; }
        
        [MaxLength(15)]
        [AllowHtml]
        public string PhoneNumber1 { get; set; }

        [MaxLength(15)]
        [AllowHtml]
        public string PhoneNumber2 { get; set; }

        [MaxLength(100)]
        [AllowHtml]
        public string Address1 { get; set; }

        [MaxLength(100)]
        [AllowHtml]
        public string Address2 { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}