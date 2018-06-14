using ServiceStack;
using System;

namespace FindMyPet.DTO.PetAlert
{
    [Route("/petalert", "PUT")]
    public class PetAlertFoundRequest : IReturn<PetAlert>
    {
        public int OwnerId { get; set; }
        public int? PetId { get; set; }
        public Guid? PetCode { get; set; }
        public string Comment { get; set; }
        public bool MakeItPublic { get; set; }
    }
}
