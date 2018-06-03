using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Pet
{
    [Route("/pet", "POST")]
    public class PetCreateRequest : IReturn<Pet>
    {
        public int? OwnerId { get; set; }
        public string OwnerMembershipId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
