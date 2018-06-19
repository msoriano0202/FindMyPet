using FindMyPet.DTO.PetSearch;
using FindMyPet.MVC.ServiceClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.DataLoaders
{
    public interface IPetSearchDataLoader
    {
        List<PetLost> SearchLostPets(DateTime from, DateTime to);
    }

    public class PetSearchDataLoader : IPetSearchDataLoader
    {
        private readonly IPetSearchServiceClient _petSearchServiceClient;

        public PetSearchDataLoader(IPetSearchServiceClient petSearchServiceClient)
        {
            if (petSearchServiceClient == null)
                throw new ArgumentNullException(nameof(petSearchServiceClient));

            _petSearchServiceClient = petSearchServiceClient;
        }

        public List<PetLost> SearchLostPets(DateTime from, DateTime to)
        {
            var request = new PetSearchByDateRequest
            {
                From = from, 
                To = to
            };

            return _petSearchServiceClient.SearchLostPets(request);
        }
    }
} 