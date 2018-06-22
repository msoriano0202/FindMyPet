using ServiceStack;
using System;
using System.Collections.Generic;

namespace FindMyPet.DTO.Admin
{
    [Route("/adminfoundalertsearch", "GET")]
    public class AdminFoundAlertSearchRequest : IReturn<List<AdminFoundAlert>>
    {
    }
}
