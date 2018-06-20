using FindMyPet.DTO.PetSearch;
using FindMyPet.MVC.Models.PetSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Mappers
{
    public interface IPetSearchMapper
    {
        PetPublicProfileViewModel GetPetPublicProfileViewModel(PetLostDetails data);
    }

    public class PetSearchMapper : IPetSearchMapper
    {
        public PetPublicProfileViewModel GetPetPublicProfileViewModel(PetLostDetails data)
        {
            var model = new PetPublicProfileViewModel();
            model.PetInfo = GetPetInfoViewModel(data.PetInfo);
            model.OwnersInfo = data.OwnersInfo.ConvertAll(x => GetOwnerDetails(x));

            return model;
        }

        private PetInfoViewModel GetPetInfoViewModel(PetDetails petDetails)
        {
            return new PetInfoViewModel
            {
                Name = petDetails.Name,
                ProfileImageUrl = petDetails.ProfileImageUrl,
                DateOfBirth = petDetails.DateOfBirth.Date,
                Description = petDetails.Description,
                LostComment = petDetails.LostComment,
                LostDateTime = petDetails.LostDateTime.ToString("dd / MMM / yyyy  hh:mm:ss tt"),
                Images = petDetails.Images
            };
        }

        private OwnerInfoViewModel GetOwnerDetails(OwnerDetails ownerDetails)
        {
            return new OwnerInfoViewModel
            {
                FullName = ownerDetails.FullName,
                ProfileImageUrl = ownerDetails.ProfileImageUrl,
                Email = ownerDetails.Email,
                PhoneNumber1 = ownerDetails.PhoneNumber1,
                PhoneNumber2 = ownerDetails.PhoneNumber2,
                Address1 = ownerDetails.Address1,
                Address2 = ownerDetails.Address2
            };
        }
    }
}