using FindMyPet.DTO.Admin;
using FindMyPet.MyServiceStack.Providers;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FindMyPet.MyServiceStack.Services
{
    public class AdminService : Service
    {
        private readonly IAdminProvider _adminProvider;

        public AdminService(IAdminProvider adminProvider)
        {
            if (adminProvider == null)
                throw new ArgumentNullException(nameof(adminProvider));

            _adminProvider = adminProvider;
        }

        public async Task<List<AdminFoundAlert>> Get(AdminFoundAlertSearchRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _adminProvider.GetAdminFoundAlertsAsync(request)
                                       .ConfigureAwait(false);
        }
    }
}