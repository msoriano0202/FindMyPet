using FindMyPet.DTO.PetSearch;

namespace FindMyPet.DTO.Owner
{
    public class OwnerAlert : PetLost
    {
        public string LostComment { get; set; }
    }
}
