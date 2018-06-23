using FindMyPet.DTO.Shared;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.PetSearch
{
    [Route("/petsuccessstory", "GET")]
    public class PetSuccessStoryRequest : PagedRequest, IReturn<PagedResponse<PetSuccessStory>>
    {
    }
}
