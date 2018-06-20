using System.Configuration;

namespace FindMyPet.MVC.Helpers
{
    public static class UrlFormatHelper
    {
        public static string FormatImageUrl(string imageUrl)
        {
            var uploadsFolder = ConfigurationManager.AppSettings["UploadsFolder"].ToString().Replace("/", "\\");
            var index = imageUrl.IndexOf(uploadsFolder);

            if (index >= 0)
                imageUrl = imageUrl.Substring(index, (imageUrl.Length - index));

            return imageUrl;
        }
    }
}