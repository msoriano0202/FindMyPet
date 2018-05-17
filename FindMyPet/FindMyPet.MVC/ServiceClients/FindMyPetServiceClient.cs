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
        private string serviceAddress = ConfigurationManager.AppSettings.Get("ServiceAddress");

        public JsonServiceClient JsonClient()
        {
            var client = new JsonServiceClient(serviceAddress);

            return client;
        }
    }
}