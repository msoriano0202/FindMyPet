using FindMyPet.DTO.Shared;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace FindMyPet.DTO.PetSearch
{
    [Route("/petlastalerts", "POST")]
    public class PetLastAlertsRequest : PagedRequest, IReturn<PagedResponse<PetLostAlert>>
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
