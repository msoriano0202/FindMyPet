using FindMyPet.DTO.Pet;
using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.Models.Pet;
using FindMyPet.MVC.ServiceClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.DataLoaders
{
    public interface IPetDataLoader
    {
        List<PetViewModel> GetPetsByOwner(string membershipId);
        void AddPet(string membershipId, PetViewModel model);
        PetViewModel GetPetByCode(string code);
        void UpdatePet(PetViewModel model);
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

        public List<PetViewModel> GetPetsByOwner(string membershipId)
        {
            var owner = _ownerServiceClient.GetuserByMembershipId(membershipId);
            var pets = _petServiceClient.GetPetsByOwnerId(owner.Id);
            var petsViewModel = pets.ConvertAll(x => _petMapper.PetToViewModel(x));

            return petsViewModel;
        }

        public void AddPet(string membershipId, PetViewModel model)
        {
            var owner = _ownerServiceClient.GetuserByMembershipId(membershipId);
            var request = _petMapper.ViewModelToCreateRequest(model);
            request.OwnerId = owner.Id;

            _petServiceClient.AddPet(request);
        }

        public PetViewModel GetPetByCode(string code)
        {
            var request = new PetRequest { Code = Guid.Parse(code)};
            var pet = _petServiceClient.GetPet(request);
            return _petMapper.PetToViewModel(pet);
        }

        public void UpdatePet(PetViewModel model)
        {
            var request = _petMapper.ViewModelToUpdateRequest(model);
            _petServiceClient.UpdatePet(request);
        }
    }
}