using ServiceStack;
using System;
using System.Collections.Generic;

namespace FindMyPet.DTO.PetAlert
{
    [Route("/petalert", "POST")]
    public class PetAlertCreateRequest : IReturn<PetAlert>
    {
        public int? OwnerId { get; set; }
        public int? PetId { get; set; }
        public Guid? PetCode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Comment { get; set; }
        public string PositionImageUrl { get; set; }
        public int Type { get; set; }
        public List<string> UrlImages { get; set; }
    }
}
