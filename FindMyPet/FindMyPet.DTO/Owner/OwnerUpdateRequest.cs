using ServiceStack;

namespace FindMyPet.DTO.Owner
{
    [Route("/owner", "PUT")]
    public class OwnerUpdateRequest : IReturn<Owner>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
