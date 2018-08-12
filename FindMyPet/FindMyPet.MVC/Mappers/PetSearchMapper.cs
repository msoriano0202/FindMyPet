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
        PetPublicProfileViewModel GetPetPublicProfileViewModel(PetAlertDetails data);
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

        public PetPublicProfileViewModel GetPetPublicProfileViewModel(PetAlertDetails data)
        {
            var model = new PetPublicProfileViewModel();
            model.PetInfo = GetPetInfoViewModel(data.PetInfo);
            model.OwnersInfo = data.OwnersInfo.ConvertAll(x => GetOwnerDetails(x));

            if (string.IsNullOrEmpty(model.PetInfo.ProfileImageUrl) && model.PetInfo.Images.Count > 0)
                model.PetInfo.ProfileImageUrl = model.PetInfo.Images[0];

            return model;
        }

        private PetInfoViewModel GetPetInfoViewModel(PetDetails petDetails)
        {
            return new PetInfoViewModel
            {
                Name = HttpUtility.HtmlDecode(petDetails.Name),
                ProfileImageUrl = petDetails.ProfileImageUrl,
                DateOfBirth = petDetails.DateOfBirth.Date,
                Description = HttpUtility.HtmlDecode(petDetails.Description),
                LostComment = HttpUtility.HtmlDecode(petDetails.LostComment),
                LostDateTime = petDetails.LostDateTime.ToString("dd / MMM / yyyy  hh:mm:ss tt"),
                PositionImageUrl = petDetails.PositionImageUrl,
                Images = petDetails.Images
            };
        }

        private OwnerInfoViewModel GetOwnerDetails(OwnerDetails ownerDetails)
        {
            return new OwnerInfoViewModel
            {
                FullName = HttpUtility.HtmlDecode(ownerDetails.FullName),
                ProfileImageUrl = ownerDetails.ProfileImageUrl,
                Email = ownerDetails.Email,
                PhoneNumber1 = HttpUtility.HtmlDecode(ownerDetails.PhoneNumber1),
                PhoneNumber2 = HttpUtility.HtmlDecode(ownerDetails.PhoneNumber2),
                Address1 = HttpUtility.HtmlDecode(ownerDetails.Address1),
                Address2 = HttpUtility.HtmlDecode(ownerDetails.Address2)
            };
        }

        public PetSuccessStoryViewModel PetSuccessStoryToViewModel(PetSuccessStory data)
        {
            return new PetSuccessStoryViewModel
            {
                OwnerFullName = HttpUtility.HtmlDecode(data.OwnerFullName),
                OwnerProfileImageUrl = data.OwnerProfileImageUrl,
                PetName = HttpUtility.HtmlDecode(data.PetName),
                PetProfileImageUrl = data.PetProfileImageUrl,
                FoundComment = HttpUtility.HtmlDecode(data.FoundComment),
                LostDateTime = data.LostDateTime.ToString("dd / MMM / yyyy hh:mm:ss tt"),
                FoundDateTime = data.FoundDateTime.ToString("dd / MMM / yyyy hh:mm:ss tt")
            };
        }

        public PetLastAlertDetailViewModel PetLastAlertToViewModel(PetLostAlert data)
        {
            return new PetLastAlertDetailViewModel
            {
                AlertCode = data.AlertCode.ToString(),
                PetId = data.PetId,
                PetCode = data.PetCode?.ToString(),
                PetName = HttpUtility.HtmlDecode(data.PetName),
                PetProfileImageUrl = data.PetProfileImageUrl,
                Latitude = data.Latitude,
                Longitude = data.Longitude,
                LostDateTime = data.LostDateTime.ToString("dd / MMM / yyyy hh:mm:ss tt"),
                Description = HttpUtility.HtmlDecode(data.Description),
                LostComment = HttpUtility.HtmlDecode(data.LostComment)
            };
        }
    }
}