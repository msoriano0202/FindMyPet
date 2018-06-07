using ServiceStack;
using System;

namespace FindMyPet.DTO.Pet
{
    [Route("/pet", "POST")]
    public class PetCreateRequest : IReturn<Pet>
    {
        public int? OwnerId { get; set; }
        public string OwnerMembershipId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Description { get; set; }
    }
}
