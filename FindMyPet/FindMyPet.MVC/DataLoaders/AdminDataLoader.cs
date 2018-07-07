using FindMyPet.DTO.Admin;
using FindMyPet.MVC.ServiceClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.DataLoaders
{
    public interface IAdminDataLoader
    {
        AdminDashboardDetails GetDashboardDetails();
        List<AdminFoundAlert> GetFoundAlertsToApprove();
        int ManageComent(string code, int action);
    }

    public class AdminDataLoader : IAdminDataLoader
    {
        private readonly IAdminServiceClient _adminServiceClient;

        public AdminDataLoader(IAdminServiceClient adminServiceClient)
        {
            if (adminServiceClient == null)
                throw new ArgumentNullException(nameof(adminServiceClient));

            _adminServiceClient = adminServiceClient;
        }

        public AdminDashboardDetails GetDashboardDetails()
        {
            return _adminServiceClient.GetDashboardDetails();
        }

        public List<AdminFoundAlert> GetFoundAlertsToApprove()
        {
            return _adminServiceClient.GetFoundAlertsToApprove();
        }

        public int ManageComent(string code, int action)
        {
            var request = new AdminManageFoundAlertRequest { Code = Guid.Parse(code), Action = action };

            return _adminServiceClient.ManageComent(request);
        }
    }
}