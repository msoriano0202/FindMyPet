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
        PetViewModel PetToViewModel(Pet pet);
        PetCreateRequest ViewModelToCreateRequest(PetViewModel model);
        PetUpdateRequest ViewModelToUpdateRequest(PetViewModel model);
    }

    public class PetMapper : IPetMapper
    {
        public PetViewModel PetToViewModel(Pet pet)
        {
            return new PetViewModel
            {
                Code = pet.Code.ToString(),
                Name = pet.Name,
                DateOfBirth = pet.DateOfBirth.Date,
                CreatedOn = pet.CreatedOn
            };
        }

        public PetCreateRequest ViewModelToCreateRequest(PetViewModel model)
        {
            return new PetCreateRequest
            {
                Name = model.Name,
                DateOfBirth = model.DateOfBirth
            };
        }

        public PetUpdateRequest ViewModelToUpdateRequest(PetViewModel model)
        {
            return new PetUpdateRequest
            {
                Code = Guid.Parse(model.Code),
                Name = model.Name,
                DateOfBirth = model.DateOfBirth
            };
        }
    }
}