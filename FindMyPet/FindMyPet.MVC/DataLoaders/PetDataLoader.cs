﻿using FindMyPet.DTO.Pet;
using FindMyPet.MVC.Helpers;
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
        Pet GetPetByCode(string code);
        Pet AddPet(string membershipId, PetProfileViewModel model);
        Pet UpdatePet(PetProfileViewModel model);
        int DeletePet(string code);
        string CreateSharePetToken(int ownerId, string petCode, string toOwnerEmail);
        int ConfirmSharePet(string token);
        PagedResponseViewModel<PetProfileViewModel> GetPetsPagedByOwner(int ownerId, int pageSize, int pageNumber);
        PetImage AddPetImage(string petCode, string imageUrl, bool isImageProfile);
        int SetPetImageAsDefault(string code);
        int DeletePetImage(string code);
    }

    public class PetDataLoader : IPetDataLoader
    {
        private readonly IPetServiceClient _petServiceClient;
        private readonly IPetMapper _petMapper;

        public PetDataLoader(IPetServiceClient petServiceClient, IPetMapper petMapper)
        {
            if (petServiceClient == null)
                throw new ArgumentNullException(nameof(petServiceClient));

            if (petMapper == null)
                throw new ArgumentNullException(nameof(petMapper));

            _petServiceClient = petServiceClient;
            _petMapper = petMapper;
        }

        public Pet GetPetByCode(string code)
        {
            var request = new PetRequest { Code = Guid.Parse(code) };
            return _petServiceClient.GetPet(request);
        }

        public Pet AddPet(string membershipId, PetProfileViewModel model)
        {
            var request = _petMapper.ProfileViewModelToCreateRequest(model);
            request.OwnerMembershipId = membershipId;

            var response = _petServiceClient.AddPet(request);
            return response;
        }

        public Pet UpdatePet(PetProfileViewModel model)
        {
            var request = _petMapper.ProfileViewModelToUpdateRequest(model);
            var response = _petServiceClient.UpdatePet(request);
            return response;
        }

        public int DeletePet(string code)
        {
            var request = new PetDeleteRequest { Code = Guid.Parse(code) };
            return _petServiceClient.DeletePet(request);
        }

        public string CreateSharePetToken(int ownerId, string petCode, string toOwnerEmail)
        {
            var request = new PetShareCreateRequest { OwnerId = ownerId, PetCode = Guid.Parse(petCode), ToOwnerEmail = toOwnerEmail };
            return _petServiceClient.CreateSharePetToken(request);
        }

        public int ConfirmSharePet(string token)
        {
            var request = new PetShareConfirmRequest { Token = Guid.Parse(token) };
            return _petServiceClient.ConfirmSharePet(request);
        }

        public PagedResponseViewModel<PetProfileViewModel> GetPetsPagedByOwner(int ownerId, int pageSize, int pageNumber)
        {
            var petsPaged = _petServiceClient.GetPetsPagedByOwnerId(ownerId, pageSize, pageNumber);

            return new PagedResponseViewModel<PetProfileViewModel>
            {
                Result = petsPaged.Result.ConvertAll(x => _petMapper.PetToProfileViewModel(x)),
                TotalPages = petsPaged.TotalPages,
                TotalRecords = petsPaged.TotalRecords
            };
        }

        public PetImage AddPetImage(string petCode, string imageUrl, bool isImageProfile)
        {
            return _petServiceClient.AddPetImage(petCode, imageUrl, isImageProfile);
        }

        public int SetPetImageAsDefault(string code)
        {
            var request = new PetImageSetAsDefaultRequest { Code = Guid.Parse(code) };
            return _petServiceClient.SetPetImageAsDefault(request);
        }

        public int DeletePetImage(string code)
        {
            var request = new PetImageDeleteRequest { Code = Guid.Parse(code) };
            return _petServiceClient.DeletePetImage(request);
        }
    }
}