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
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
