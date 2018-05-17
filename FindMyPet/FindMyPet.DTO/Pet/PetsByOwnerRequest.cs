using ServiceStack;
using System.Collections.Generic;

namespace FindMyPet.DTO.Pet
{
    [Route("/petsbyowner", "POST")]
    public class PetsByOwnerRequest : IReturn<List<Pet>>
    {
        public int OwnerId { get; set; }
    }
}
