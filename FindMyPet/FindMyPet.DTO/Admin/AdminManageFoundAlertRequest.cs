using ServiceStack;
using System;

namespace FindMyPet.DTO.Admin
{
    [Route("/adminmanagefoundalert", "POST")]
    public class AdminManageFoundAlertRequest : IReturn<int>
    {
        public int? Id { get; set; }
        public Guid? Code { get; set; }
        public int Action { get; set; }
    }
}
