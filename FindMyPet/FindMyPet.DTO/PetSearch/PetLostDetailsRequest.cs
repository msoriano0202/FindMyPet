using ServiceStack;
using System;
using System.Collections.Generic;

namespace FindMyPet.DTO.PetSearch
{
    [Route("/petlostdetails", "POST")]
    public class PetLostDetailsRequest : IReturn<PetLostDetails>
    {
        public int? PetId { get; set; }
        public Guid? PetCode { get; set; }
    }
}
