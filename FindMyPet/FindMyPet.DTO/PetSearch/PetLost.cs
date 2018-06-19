using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.PetSearch
{
    public class PetLost
    {
        public int? PetId { get; set; }
        public Guid? PetCode { get; set; }
        public string PetName { get; set; }
        public string PetProfileImageUrl { get; set; }
        //public int OwnerId { get; set; }
        //public Guid OwnerCode { get; set; }
        //public string OwnerName { get; set; }
        //public string OwnerProfileImageUrl { get; set; }
        public int Type { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTimeOffset LostDateTime { get; set; }
    }
}
