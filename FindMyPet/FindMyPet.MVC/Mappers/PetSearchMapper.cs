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
        PetSuccessStoryViewModel PetSuccessStoryToViewModel(PetSuccessStory data);
        PetLastAlertDetailViewModel PetLastAlertToViewModel(PetLostAlert data);
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

        public PetSuccessStoryViewModel PetSuccessStoryToViewModel(PetSuccessStory data)
        {
            return new PetSuccessStoryViewModel
            {
                OwnerFullName =  data.OwnerFullName,
                OwnerProfileImageUrl = data.OwnerProfileImageUrl,
                PetName = data.PetName,
                PetProfileImageUrl = data.PetProfileImageUrl,
                FoundComment = data.FoundComment,
                LostDateTime = data.LostDateTime.ToString("dd / MMM / yyyy hh:mm:ss tt"),
                FoundDateTime = data.FoundDateTime.ToString("dd / MMM / yyyy hh:mm:ss tt")
            };
        }

        public PetLastAlertDetailViewModel PetLastAlertToViewModel(PetLostAlert data)
        {
            return new PetLastAlertDetailViewModel
            {
                PetId = data.PetId.Value,
                PetCode = data.PetCode.Value.ToString(),
                PetName = data.PetName,
                PetProfileImageUrl = data.PetProfileImageUrl,
                Latitude = data.Latitude,
                Longitude = data.Longitude,
                LostDateTime = data.LostDateTime.ToString("dd / MMM / yyyy hh:mm:ss tt"),
                Description = data.Description,
                LostComment = data.LostComment
            };
        }
    }
}