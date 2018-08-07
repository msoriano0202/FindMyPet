using ServiceStack;
using System;

namespace FindMyPet.DTO.Admin
{
    [Route("/adminmanagereportedalert", "POST")]
    public class AdminManageReportedAlertRequest : IReturn<int>
    {
        public int? Id { get; set; }
        public Guid? Code { get; set; }
        public int Action { get; set; }
    }
}
