using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Pet
{
    public class PetImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsProfileImage { get; set; }
    }
}
