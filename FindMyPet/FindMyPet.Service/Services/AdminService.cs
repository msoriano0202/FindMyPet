﻿using FindMyPet.DTO.Admin;
using FindMyPet.MyServiceStack.Providers;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FindMyPet.MyServiceStack.Services
{
    [Authenticate]
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

        public async Task<int> Post(AdminManageFoundAlertRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _adminProvider.ManageAdminFoundAlertsAsync(request)
                                       .ConfigureAwait(false);
        }

        public async Task<AdminDashboardDetails> Get(AdminDashboardRequest request)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            return await _adminProvider.GetAdminDashboardAsync(request)
                                       .ConfigureAwait(false);
        }

        public async Task<List<AdminReportedAlert>> Get(AdminReportedAlertSearchRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _adminProvider.GetAdminReportedAlertsAsync(request)
                                       .ConfigureAwait(false);
        }

        public async Task<int> Post(AdminManageReportedAlertRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _adminProvider.ManageReportedAlertsAsync(request)
                                       .ConfigureAwait(false);
        }
    }
}