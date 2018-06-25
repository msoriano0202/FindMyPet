﻿using FindMyPet.DTO.Admin;
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
        Task<int> ManageAdminFoundAlertsAsync(AdminManageFoundAlertRequest request);
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

        public async Task<int> ManageAdminFoundAlertsAsync(AdminManageFoundAlertRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Id.HasValue && !request.Code.HasValue)
                throw new ArgumentException("Id and Code are NULL");

            return await _adminDataAccess.ManageAdminFoundAlertsAsync(request)
                                         .ConfigureAwait(false);
        }
    }
}