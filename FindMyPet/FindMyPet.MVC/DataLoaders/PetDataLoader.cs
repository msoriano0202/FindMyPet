using FindMyPet.DTO.Pet;
using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.Models.Pet;
using FindMyPet.MVC.Models.Shared;
using FindMyPet.MVC.ServiceClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.DataLoaders
{
    public interface IPetDataLoader
    {
        PetViewModel GetPetByCode(string code);
        Pet AddPet(string membershipId, PetViewModel model);
        Pet UpdatePet(PetViewModel model);
        PagedResponseViewModel<PetViewModel> GetPetsPagedByOwner(int ownerId, int pageSize, int pageNumber);
        //List<PetViewModel> GetPetsByOwner(string membershipId);
    }

    public class PetDataLoader : IPetDataLoader
    {
        private readonly IOwnerServiceClient _ownerServiceClient;
        private readonly IPetServiceClient _petServiceClient;
        private readonly IPetMapper _petMapper;

        public PetDataLoader(IOwnerServiceClient ownerServiceClient, IPetServiceClient petServiceClient, IPetMapper petMapper)
        {
            if (ownerServiceClient == null)
                throw new ArgumentNullException(nameof(ownerServiceClient));

            if (petServiceClient == null)
                throw new ArgumentNullException(nameof(petServiceClient));

            if (petMapper == null)
                throw new ArgumentNullException(nameof(petMapper));

            _ownerServiceClient = ownerServiceClient;
            _petServiceClient = petServiceClient;
            _petMapper = petMapper;
        }

        public PetViewModel GetPetByCode(string code)
        {
            var request = new PetRequest { Code = Guid.Parse(code) };
            var pet = _petServiceClient.GetPet(request);
            return _petMapper.PetToViewModel(pet);
        }

        public Pet AddPet(string membershipId, PetViewModel model)
        {
            var owner = _ownerServiceClient.GetOwnerByMembershipId(membershipId);
            var request = _petMapper.ViewModelToCreateRequest(model);
            request.OwnerId = owner.Id;

            var response = _petServiceClient.AddPet(request);
            return response;
        }

        public Pet UpdatePet(PetViewModel model)
        {
            var request = _petMapper.ViewModelToUpdateRequest(model);
            var response = _petServiceClient.UpdatePet(request);
            return response;
        }

        public PagedResponseViewModel<PetViewModel> GetPetsPagedByOwner(int ownerId, int pageSize, int pageNumber)
        {
            var petsPaged = _petServiceClient.GetPetsPagedByOwnerId(ownerId, pageSize, pageNumber);

            return new PagedResponseViewModel<PetViewModel>
            {
                Result = petsPaged.Result.ConvertAll(x => _petMapper.PetToViewModel(x)),
                TotalPages = petsPaged.TotalPages,
                TotalRecords = petsPaged.TotalRecords
            };
        }

        //public List<PetViewModel> GetPetsByOwner(string membershipId)
        //{
        //    var owner = _ownerServiceClient.GetuserByMembershipId(membershipId);
        //    var pets = _petServiceClient.GetPetsByOwnerId(owner.Id);
        //    var petsViewModel = pets.ConvertAll(x => _petMapper.PetToViewModel(x));

        //    return petsViewModel;
        //}
    }
}