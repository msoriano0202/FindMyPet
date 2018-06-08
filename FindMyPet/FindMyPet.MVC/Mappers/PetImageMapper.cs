using FindMyPet.DTO.Pet;
using FindMyPet.MVC.Helpers;
using FindMyPet.MVC.Models.Pet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Mappers
{
    public interface IPetImageMapper
    {
        PetImageViewModel PetImageToPetImageViewModel(PetImage petImage);
    }

    public class PetImageMapper : IPetImageMapper
    {
        private readonly IGeneralHelper _generalHelper;

        public PetImageMapper(IGeneralHelper generalHelper)
        {
            if (generalHelper == null)
                throw new ArgumentNullException(nameof(generalHelper));

            _generalHelper = generalHelper;
        }

        public PetImageViewModel PetImageToPetImageViewModel(PetImage petImage)
        {
            return new PetImageViewModel
            {
                Id = petImage.Id,
                ImageUrl = _generalHelper.FormatSiteImageUrl(petImage.ImageUrl),
                IsProfileImage = petImage.IsProfileImage
            };
        }
    }
}