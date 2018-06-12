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
        Pet GetPet(PetRequest request);
        Pet AddPet(PetCreateRequest request);
        Pet UpdatePet(PetUpdateRequest request);
        int DeletePet(PetDeleteRequest request);
        int SharePet(PetShareRequest request);
        PagedResponseViewModel<Pet> GetPetsPagedByOwnerId(int ownerId, int pageSize, int pageNumber);
        PetImage AddPetImage(string petCode, string imageUrl, bool isImageProfile);
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
            var request = new PetsSearchByOwnerRequest { OwnerId = ownerId };
            var response = _findMyPetClient.JsonClient().Post(request);

            return response.Result;
        }

        public Pet GetPet(PetRequest request)
        {
            var response = _findMyPetClient.JsonClient().Get(request);

            return response;
        }

        public Pet AddPet(PetCreateRequest request)
        {
            var response = _findMyPetClient.JsonClient().Post(request);

            return response;
        }

        public Pet UpdatePet(PetUpdateRequest request)
        {
            var response = _findMyPetClient.JsonClient().Put(request);

            return response;
        }

        public int DeletePet(PetDeleteRequest request)
        {
            var response = _findMyPetClient.JsonClient().Delete(request);

            return response;
        }

        public int SharePet(PetShareRequest request)
        {
            var response = _findMyPetClient.JsonClient().Post(request);

            return response;
        }

        public PagedResponseViewModel<Pet> GetPetsPagedByOwnerId(int ownerId, int pageSize, int pageNumber)
        {
            var request = new PetsSearchByOwnerRequest { OwnerId = ownerId, PageSize = pageSize, PageNumber = pageNumber };
            var response = _findMyPetClient.JsonClient().Post(request);

            return new PagedResponseViewModel<Pet>
            {
                Result = response.Result,
                TotalPages = response.TotalPages,
                TotalRecords = response.TotalRecords
            };
        }

        public PetImage AddPetImage(string petCode, string imageUrl, bool isImageProfile)
        {
            var request = new PetImageAddRequest()
            {
                PetCode = Guid.Parse(petCode),
                ImageUrl = imageUrl,
                IsImageProfile = isImageProfile
            };

            var response = _findMyPetClient.JsonClient().Post(request);
            return response;
        }
    }
}