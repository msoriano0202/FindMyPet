using FindMyPet.DTO.Owner;
using FindMyPet.DTO.Pet;
using FindMyPet.MVC.Helpers;
using FindMyPet.MVC.Models.Pet;
using FindMyPet.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Mappers
{
    public interface IPetMapper
    {
        PetProfileViewModel PetToProfileViewModel(Pet pet);
        PetShareViewModel PetToPetShareViewModel(Pet pet);
        PetCreateRequest ProfileViewModelToCreateRequest(PetProfileViewModel model);
        PetUpdateRequest ProfileViewModelToUpdateRequest(PetProfileViewModel model);
    }

    public class PetMapper : IPetMapper
    {
        private readonly IGeneralHelper _generalHelper;

        public PetMapper(IGeneralHelper generalHelper)
        {
            if (generalHelper == null)
                throw new ArgumentNullException(nameof(generalHelper));

            _generalHelper = generalHelper;
        }

        public PetProfileViewModel PetToProfileViewModel(Pet pet)
        {
            var defaultPetImage = ConfigurationManager.AppSettings["DefaultImagePetProfile"].ToString();

            return new PetProfileViewModel
            {
                Code = pet.Code.ToString(),
                Name = pet.Name,
                DateOfBirth = pet.DateOfBirth.Date,
                Description = pet.Description,
                ProfileImageUrl = !string.IsNullOrEmpty(pet.ProfileImageUrl) 
                                        ? _generalHelper.FormatSiteImageUrl(pet.ProfileImageUrl) 
                                        : defaultPetImage,
                Status = _generalHelper.GetPetStatus(pet.Status),
                StatusId = (PetStatusEnum)pet.Status
            };
        }

        public PetShareViewModel PetToPetShareViewModel(Pet pet)
        {
            var defaultOwnerImage = ConfigurationManager.AppSettings["DefaultImageOwnerProfile"].ToString();

            return new PetShareViewModel
            {
                PetCode = pet.Code.ToString(),
                Owners = pet.Owners.ConvertAll(x => new PetSharedOwnerViewModel {
                    FullName = x.FullName,
                    ProfileImageUrl = !string.IsNullOrEmpty(x.ProfileImageUrl)
                                        ? _generalHelper.FormatSiteImageUrl(x.ProfileImageUrl)
                                        : defaultOwnerImage,
                    RegisteredDate = x.RegisteredDate.Date
                }).OrderBy(x => x.FullName).ToList()
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