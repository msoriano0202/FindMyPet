using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Owner
{
    [Route("/owner", "POST")]
    public class OwnerCreateRequest : IReturn<Owner>
    {
        public string MembershipId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
