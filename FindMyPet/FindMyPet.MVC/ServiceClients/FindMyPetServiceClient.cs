using FindMyPet.Shared;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.ServiceClients
{
    public interface IFindMyPetServiceClient
    {
        JsonServiceClient JsonClient();
    }

    public class FindMyPetServiceClient : IFindMyPetServiceClient
    {
        private readonly IGlobalHelper _globalHelper;

        public FindMyPetServiceClient(): this(new GlobalHelper())
        {
        }

        public FindMyPetServiceClient(IGlobalHelper globalHelper)
        {
            if (globalHelper == null)
                throw new ArgumentNullException(nameof(globalHelper));

            _globalHelper = globalHelper;
        }

        public JsonServiceClient JsonClient()
        {
            var serviceAddress = _globalHelper.GetAppSettingByEnvironment("ServiceAddress");
            var client = new JsonServiceClient(serviceAddress);

            return client;
        }
    }
}