using FindMyPet.DTO.Pet;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.ServiceClients
{
    public interface IPetServiceClient
    {
        List<Pet> GetPetsByOwnerId(int ownerId);
        Pet AddPet(CreatePetRequest request);
        Pet GetPet(PetRequest request);
        Pet UpdatePet(UpdatePetRequest request);
    }

    public class PetServiceClient : IPetServiceClient
    {
        private readonly IFindMyPetServiceClient _findMyPetClient;
        public PetServiceClient(IFindMyPetServiceClient findMyPetClient)
        {
            if (findMyPetClient == null)
                throw new ArgumentNullException(nameof(findMyPetClient));

            _findMyPetClient = findMyPetClient;
        }

        public List<Pet> GetPetsByOwnerId(int ownerId)
        {
            var request = new PetsByOwnerRequest { OwnerId = ownerId };
            var response = _findMyPetClient.JsonClient().Post(request);

            return response;
        }

        public Pet AddPet(CreatePetRequest request)
        {
            var response = _findMyPetClient.JsonClient().Post(request);

            return response;
        }

        public Pet GetPet(PetRequest request)
        {
            var response = _findMyPetClient.JsonClient().Get(request);

            return response;
        }

        public Pet UpdatePet(UpdatePetRequest request)
        {
            var response = _findMyPetClient.JsonClient().Post(request);

            return response;
        }
    }
}