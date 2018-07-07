using ServiceStack;

namespace FindMyPet.DTO.Admin
{
    [Route("/admindashboard", "GET")]
    public class AdminDashboardRequest : IReturn<AdminDashboardDetails>
    {
    }
}
