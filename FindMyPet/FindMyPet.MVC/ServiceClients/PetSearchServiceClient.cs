using FindMyPet.DTO.PetSearch;
using FindMyPet.MVC.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.ServiceClients
{
    public interface IPetSearchServiceClient
    {
        List<PetLost> SearchLostPets(PetSearchByDateRequest request);
        PetLostDetails GetPetLostDetails(PetLostDetailsRequest request);
        PagedResponseViewModel<PetSuccessStory> GetPetSuccessStories(int pageSize, int pageNumber);
        PagedResponseViewModel<PetLostAlert> GetPetLastAlerts(PetLastAlertsRequest request);
    }

    public class PetSearchServiceClient : IPetSearchServiceClient
    {
        private readonly IFindMyPetServiceClient _findMyPetClient;

        public PetSearchServiceClient(IFindMyPetServiceClient findMyPetClient)
        {
            if (findMyPetClient == null)
                throw new ArgumentNullException(nameof(findMyPetClient));

            _findMyPetClient = findMyPetClient;
        }

        public List<PetLost> SearchLostPets(PetSearchByDateRequest request)
        {
            var response = _findMyPetClient.JsonClient().Post(request);

            return response;
        }

        public PetLostDetails GetPetLostDetails(PetLostDetailsRequest request)
        {
            var response = _findMyPetClient.JsonClient().Post(request);

            return response;
        }

        public PagedResponseViewModel<PetSuccessStory> GetPetSuccessStories(int pageSize, int pageNumber)
        {
            var request = new PetSuccessStoryRequest { PageSize = pageSize, PageNumber = pageNumber };
            var response = _findMyPetClient.JsonClient().Get(request);

            return new PagedResponseViewModel<PetSuccessStory>
            {
                Result = response.Result,
                TotalPages = response.TotalPages,
                TotalRecords = response.TotalRecords
            };
        }

        public PagedResponseViewModel<PetLostAlert> GetPetLastAlerts(PetLastAlertsRequest request)
        {
            var response = _findMyPetClient.JsonClient().Post(request);

            return new PagedResponseViewModel<PetLostAlert>
            {
                Result = response.Result,
                TotalPages = response.TotalPages,
                TotalRecords = response.TotalRecords
            };
        }
    }
}