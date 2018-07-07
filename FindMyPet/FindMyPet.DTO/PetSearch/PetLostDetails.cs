using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.PetSearch
{
    public class PetLostDetails
    {
        public PetDetails PetInfo { get; set; }
        public List<OwnerDetails> OwnersInfo { get; set; }
    }

    public class PetDetails
    {
        public string Name { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Description { get; set; }
        public string LostComment { get; set; }
        public DateTimeOffset LostDateTime { get; set; }
        public List<string> Images { get; set; }
        public String PositionImageUrl { get; set; }
    }

    public class OwnerDetails
    {
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }
}
