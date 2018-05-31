using FindMyPet.DTO.Pet;
using FindMyPet.MVC.Models.Shared;
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
        PagedResponseViewModel<Pet> GetPetsPagedByOwnerId(int ownerId, int pageSize, int pageNumber);
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

            return response.Result;
        }

        public PagedResponseViewModel<Pet> GetPetsPagedByOwnerId(int ownerId, int pageSize, int pageNumber)
        {
            var request = new PetsByOwnerRequest { OwnerId = ownerId, PageSize = pageSize, PageNumber = pageNumber };
            var response = _findMyPetClient.JsonClient().Post(request);

            return new PagedResponseViewModel<Pet>
            {
                Result = response.Result,
                TotalPages = response.TotalPages,
                TotalRecords = response.TotalRecords
            };
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