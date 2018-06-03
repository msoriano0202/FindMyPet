using ServiceStack;
using System.Collections.Generic;

namespace FindMyPet.DTO.Owner
{
    [Route("/searchowner", "POST")]
    public class OwnerSearchRequest : IReturn<List<Owner>>
    {
        public string MembershipId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
