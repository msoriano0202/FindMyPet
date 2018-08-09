using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindMyPet.DTO.PetAlert;

namespace FindMyPet.DTO.Pet
{
    public class Pet
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public int SexType { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ProfileImageUrl { get; set; }
        public List<PetImage> Images { get; set; }
        public List<PetOwner> Owners { get; set; } 
        public List<PetAlert.PetAlert> Alerts { get; set; }
    }
}
