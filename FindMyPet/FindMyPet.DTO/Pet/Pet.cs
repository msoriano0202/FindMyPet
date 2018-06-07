using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Pet
{
    public class Pet
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public List<PetImage> Images { get; set; }
    }
}
