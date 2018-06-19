using ServiceStack;
using System;
using System.Collections.Generic;

namespace FindMyPet.DTO.PetSearch
{
    [Route("/petsearchbydate", "POST")]
    public class PetSearchByDateRequest : IReturn<List<PetLost>>
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
