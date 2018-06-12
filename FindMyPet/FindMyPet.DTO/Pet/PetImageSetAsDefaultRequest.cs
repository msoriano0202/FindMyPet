using ServiceStack;
using System;

namespace FindMyPet.DTO.Pet
{
    [Route("/petimage", "PUT")]
    public class PetImageSetAsDefaultRequest : IReturn<int>
    {
        public int? Id { get; set; }
        public Guid? Code { get; set; }
    }
}
