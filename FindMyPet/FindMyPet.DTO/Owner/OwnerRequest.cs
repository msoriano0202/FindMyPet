using ServiceStack;

namespace FindMyPet.DTO.Owner
{
    [Route("/owner", "GET")]
    public class OwnerRequest : IReturn<Owner>
    {
        public int? Id { get; set; }
        public string MembershipId { get; set; }
    }
}
