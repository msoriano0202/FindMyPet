using ServiceStack;
using System;

namespace FindMyPet.DTO.Pet
{
    [Route("/pet", "PUT")]
    public class UpdatePetRequest : IReturn<Pet>
    {
        public int? Id { get; set; }
        public Guid? Code { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}