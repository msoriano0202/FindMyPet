using ServiceStack;
using System;

namespace FindMyPet.DTO.Pet
{
    [Route("/petshare", "PUT")]
    public class PetShareConfirmRequest : IReturn<int>
    {
        public Guid Token { get; set; }
    }
}
