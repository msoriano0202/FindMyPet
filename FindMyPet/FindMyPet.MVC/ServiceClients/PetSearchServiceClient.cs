using FindMyPet.DTO.PetSearch;
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
    }
}