using System;

namespace FindMyPet.DTO.PetSearch
{
    public class PetLost
    {
        public Guid AlertCode { get; set; }
        public int? PetId { get; set; }
        public Guid? PetCode { get; set; }
        public string PetName { get; set; }
        public string PetProfileImageUrl { get; set; }
        public int Type { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTimeOffset LostDateTime { get; set; }
    }
}
