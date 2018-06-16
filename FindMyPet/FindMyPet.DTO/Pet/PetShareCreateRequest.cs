using ServiceStack;
using System;

namespace FindMyPet.DTO.Pet
{
    [Route("/petshare", "POST")]
    public class PetShareCreateRequest : IReturn<string>
    {
        public int OwnerId { get; set; }
        public Guid? PetCode { get; set; }
        public int? PetId { get; set; }
        public string ToOwnerEmail { get; set; }
    }
}
