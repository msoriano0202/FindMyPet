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
    }
}