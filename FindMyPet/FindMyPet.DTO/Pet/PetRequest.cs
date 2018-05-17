using ServiceStack;
using System;

namespace FindMyPet.DTO.Pet
{
    [Route("/pet", "GET")]
    public class PetRequest : IReturn<Pet>
    {
        public int? Id { get; set; }

        public Guid? Code { get; set; }
    }
}
