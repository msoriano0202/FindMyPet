using ServiceStack;
using System;

namespace FindMyPet.DTO.Pet
{
    [Route("/petshare", "POST")]
    public class PetShareRequest : IReturn<int>
    {
        public string PetCode { get; set; }
        public string OwnerMembershipId { get; set; }
    }
}
