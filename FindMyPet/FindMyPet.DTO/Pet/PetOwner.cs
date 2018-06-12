using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Pet
{
    public class PetOwner
    {
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTimeOffset RegisteredDate { get; set; }
    }
}
