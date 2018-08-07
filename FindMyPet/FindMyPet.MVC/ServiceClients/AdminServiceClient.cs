using FindMyPet.DTO.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.ServiceClients
{
    public interface IAdminServiceClient
    {
        AdminDashboardDetails GetDashboardDetails();
        List<AdminFoundAlert> GetFoundAlertsToApprove();
        int ManageComent(AdminManageFoundAlertRequest request);
        List<AdminReportedAlert> GetReportedAlertsToApprove();
        int ManageReportedAlerts(AdminManageReportedAlertRequest request);
    }

    public class AdminServiceClient : IAdminServiceClient
    {
        private readonly IFindMyPetServiceClient _findMyPetClient;

        public AdminServiceClient(IFindMyPetServiceClient findMyPetClient)
        {
            if (findMyPetClient == null)
                throw new ArgumentNullException(nameof(findMyPetClient));

            _findMyPetClient = findMyPetClient;
        }

        public AdminDashboardDetails GetDashboardDetails()
        {
            var request = new AdminDashboardRequest();

            return _findMyPetClient.JsonClient().Get(request);
        }

        public List<AdminFoundAlert> GetFoundAlertsToApprove()
        {
            var request = new AdminFoundAlertSearchRequest();

            return _findMyPetClient.JsonClient().Get(request);
        }

        public int ManageComent(AdminManageFoundAlertRequest request)
        {
            return _findMyPetClient.JsonClient().Post(request);
        }

        public List<AdminReportedAlert> GetReportedAlertsToApprove()
        {
            var request = new AdminReportedAlertSearchRequest();

            return _findMyPetClient.JsonClient().Get(request);
        }

        public int ManageReportedAlerts(AdminManageReportedAlertRequest request)
        {
            return _findMyPetClient.JsonClient().Post(request);
        }
    }
}