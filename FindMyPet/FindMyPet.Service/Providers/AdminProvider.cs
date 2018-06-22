using FindMyPet.DTO.Admin;
using FindMyPet.MyServiceStack.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FindMyPet.MyServiceStack.Providers
{
    public interface IAdminProvider
    {
        Task<List<AdminFoundAlert>> GetAdminFoundAlertsAsync(AdminFoundAlertSearchRequest request);
    }

    public class AdminProvider : IAdminProvider
    {
        private readonly IAdminDataAccess _adminDataAccess;

        public AdminProvider(IAdminDataAccess adminDataAccess)
        {
            if (adminDataAccess == null)
                throw new ArgumentNullException(nameof(adminDataAccess));

            _adminDataAccess = adminDataAccess;
        }

        public async Task<List<AdminFoundAlert>> GetAdminFoundAlertsAsync(AdminFoundAlertSearchRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _adminDataAccess.GetAdminFoundAlertsAsync()
                                         .ConfigureAwait(false);

        }
    }
}