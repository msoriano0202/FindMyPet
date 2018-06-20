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
        PetLostDetails GetPetLostDetails(Guid petCode);
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

        public PetLostDetails GetPetLostDetails(Guid petCode)
        {
            var request = new PetLostDetailsRequest { PetCode = petCode };

            return _petSearchServiceClient.GetPetLostDetails(request);
        }
    }
} 