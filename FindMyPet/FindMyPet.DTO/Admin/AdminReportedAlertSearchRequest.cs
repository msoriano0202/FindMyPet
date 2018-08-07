using ServiceStack;
using System;
using System.Collections.Generic;

namespace FindMyPet.DTO.Admin
{
    [Route("/adminreportedalertsearch", "GET")]
    public class AdminReportedAlertSearchRequest : IReturn<List<AdminReportedAlert>>
    {
    }
}
