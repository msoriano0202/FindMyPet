using ServiceStack;
using System;

namespace FindMyPet.DTO.PetSearch
{
    [Route("/petalertreport", "POST")]
    public class PetAlertReportManageRequest : IReturn<int>
    {
        public Guid Code { get; set; }
        public int Action { get; set; }
    }
}
