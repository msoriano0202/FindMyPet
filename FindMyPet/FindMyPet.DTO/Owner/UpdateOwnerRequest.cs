using ServiceStack;

namespace FindMyPet.DTO.Owner
{
    [Route("/updateowner", "POST")]
    public class UpdateOwnerRequest : IReturn<Owner>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
