using ServiceStack;
using System;
using System.Collections.Generic;

namespace FindMyPet.DTO.PetSearch
{
    [Route("/petalertdetails", "POST")]
    public class PetAlertDetailsRequest : IReturn<PetAlertDetails>
    {
        public Guid AlertCode { get; set; }
    }
}
