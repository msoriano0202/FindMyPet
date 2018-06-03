using ServiceStack;
using System;

namespace FindMyPet.DTO.Pet
{
    [Route("/pet", "DELETE")]
    public class PetDeleteRequest : IReturn<int>
    {
        public int? Id { get; set; }

        public Guid? Code { get; set; }
    }
}
