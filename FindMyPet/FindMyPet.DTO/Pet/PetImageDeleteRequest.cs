using ServiceStack;
using System;


namespace FindMyPet.DTO.Pet
{
    [Route("/petimage", "DELETE")]
    public class PetImageDeleteRequest : IReturn<int>
    {
        public int? Id { get; set; }
        public Guid? Code { get; set; }
    }
}
