using ServiceStack;
using System.Collections.Generic;

namespace FindMyPet.DTO.Owner
{
    [Route("/owneralerts", "GET")]
    public class OwnerAlertSearchRequest : IReturn<List<OwnerAlert>>
    {
        public int? Id { get; set; }
        public string MembershipId { get; set; }
    }
}
