using FindMyPet.DTO.Pet;
using FindMyPet.MVC.Models.Pet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Mappers
{
    public interface IPetMapper
    {
        PetProfileViewModel PetToProfileViewModel(Pet pet);
        PetCreateRequest ProfileViewModelToCreateRequest(PetProfileViewModel model);
        PetUpdateRequest ProfileViewModelToUpdateRequest(PetProfileViewModel model);
    }

    public class PetMapper : IPetMapper
    {
        public PetProfileViewModel PetToProfileViewModel(Pet pet)
        {
            return new PetProfileViewModel
            {
                Code = pet.Code.ToString(),
                Name = pet.Name,
                DateOfBirth = pet.DateOfBirth.Date,
                Description = pet.Description
            };
        }

        public PetCreateRequest ProfileViewModelToCreateRequest(PetProfileViewModel model)
        {
            return new PetCreateRequest
            {
                Name = model.Name,
                DateOfBirth = model.DateOfBirth,
                Description = model.Description
            };
        }

        public PetUpdateRequest ProfileViewModelToUpdateRequest(PetProfileViewModel model)
        {
            return new PetUpdateRequest
            {
                Code = Guid.Parse(model.Code),
                Name = model.Name,
                DateOfBirth = model.DateOfBirth,
                Description = model.Description
            };
        }
    }
}