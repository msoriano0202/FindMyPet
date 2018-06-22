using FindMyPet.DTO.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.ServiceClients
{
    public interface IAdminServiceClient
    {
        List<AdminFoundAlert> GetFoundAlertsToApprove();
        int ManageComent(AdminManageFoundAlertRequest request);
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

        public List<AdminFoundAlert> GetFoundAlertsToApprove()
        {
            var request = new AdminFoundAlertSearchRequest();

            return _findMyPetClient.JsonClient().Get(request);
        }

        public int ManageComent(AdminManageFoundAlertRequest request)
        {
            return _findMyPetClient.JsonClient().Post(request);
        }
    }
}