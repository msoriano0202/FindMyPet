using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.PetSearch
{
    public class PetSuccessStory
    {
        public string OwnerFullName { get; set; }
        public string OwnerProfileImageUrl { get; set; }
        public string PetName { get; set; }
        public string PetProfileImageUrl { get; set; }
        public string FoundComment { get; set; }
        public DateTimeOffset LostDateTime { get; set; }
        public DateTimeOffset FoundDateTime { get; set; }
    }
}
