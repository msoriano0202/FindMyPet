using FindMyPet.DTO.Shared;
using ServiceStack;
using System.Collections.Generic;

namespace FindMyPet.DTO.Pet
{
    [Route("/petsbyowner", "POST")]
    public class PetsSearchByOwnerRequest : PagedRequest, IReturn<PagedResponse<Pet>>
    {
        public int OwnerId { get; set; }
    }
}
