using ServiceStack;
using System;

namespace FindMyPet.DTO.Pet
{
    [Route("/petimage", "POST")]
    public class PetImageAddRequest : IReturn<PetImage>
    {
        public int? PetId { get; set; }
        public Guid? PetCode { get; set; }
        public string ImageUrl { get; set; }
        public bool IsImageProfile { get; set; }
    }
}
