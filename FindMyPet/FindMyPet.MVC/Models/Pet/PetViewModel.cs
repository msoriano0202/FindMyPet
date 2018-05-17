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

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType("Date")]
        public DateTime DateOfBirth { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}