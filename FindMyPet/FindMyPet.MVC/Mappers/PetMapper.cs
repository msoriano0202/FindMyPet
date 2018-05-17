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
        CreatePetRequest ViewModelToCreateRequest(PetViewModel model);
        UpdatePetRequest ViewModelToUpdateRequest(PetViewModel model);
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

        public CreatePetRequest ViewModelToCreateRequest(PetViewModel model)
        {
            return new CreatePetRequest
            {
                Name = model.Name,
                DateOfBirth = model.DateOfBirth
            };
        }

        public UpdatePetRequest ViewModelToUpdateRequest(PetViewModel model)
        {
            return new UpdatePetRequest
            {
                Code = Guid.Parse(model.Code),
                Name = model.Name,
                DateOfBirth = model.DateOfBirth
            };
        }
    }
}