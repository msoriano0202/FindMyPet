using ServiceStack;
using System;

namespace FindMyPet.DTO.Pet
{
    [Route("/pet", "PUT")]
    public class PetUpdateRequest : IReturn<Pet>
    {
        public int? Id { get; set; }
        public Guid? Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}