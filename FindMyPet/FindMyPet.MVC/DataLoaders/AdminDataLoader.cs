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
        List<AdminFoundAlert> GetFoundAlertsToApprove();
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

        public List<AdminFoundAlert> GetFoundAlertsToApprove()
        {
            return _adminServiceClient.GetFoundAlertsToApprove();
        }
    }
}