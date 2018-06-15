using FindMyPet.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Helpers
{
    public interface IGeneralHelper
    {
        string FormatSiteImageUrl(string imageUrl);
        string GetPetStatus(int status);
    }

    public class GeneralHelper : IGeneralHelper
    {
        public string FormatSiteImageUrl(string imageUrl)
        {
            var uploadsFolder = ConfigurationManager.AppSettings["UploadsFolder"].ToString().Replace("/", "\\");
            var index = imageUrl.IndexOf(uploadsFolder);

            if (index >= 0)
                imageUrl = imageUrl.Substring(index, (imageUrl.Length - index));

            return imageUrl;
        }

        public string GetPetStatus(int status)
        {
            var result = string.Empty;

            switch (status)
            {
                case (int)PetStatusEnum.Active:
                    result = "Activo";
                    break;
                case (int)PetStatusEnum.Lost:
                    result = "Perdido";
                    break;
                case (int)PetStatusEnum.Found:
                    result = "Encontrado";
                    break;
            }

            return result;
        }
    }
}