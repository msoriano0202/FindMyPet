using FindMyPet.DTO.Owner;
using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Models.Profile;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace FindMyPet.MVC.Controllers
{
    public class BaseController : Controller
    {
        private OwnerDataLoader _ownerDataLoader;

        public BaseController() : this(new OwnerDataLoader())
        { }

        public BaseController(OwnerDataLoader ownerDataLoader)
        {
            if (ownerDataLoader == null)
                throw new ArgumentNullException(nameof(ownerDataLoader));

            _ownerDataLoader = ownerDataLoader;
        }

        #region --- VerifySessionVariables ---

        public void VerifySessionVariables()
        {
            var ownerMembershipId = Session["OwnerMembershipId"]?.ToString();
            if (string.IsNullOrEmpty(ownerMembershipId) && Request.IsAuthenticated)
            {
                var userMembershipId = User.Identity.GetUserId();
                var owner = _ownerDataLoader.GetuserByMembershipId(userMembershipId);

                SetSessionVariables(owner.Id,
                                    string.Format("{0} {1}", owner.FirstName, owner.LastName),
                                    userMembershipId,
                                    string.IsNullOrEmpty(owner.ProfileImageUrl) ? GetDefaultImageProfile() : FormatSiteImageUrl(owner.ProfileImageUrl));
            }
        }

        #endregion

        #region --- Register Owner / Load Owner / Clean ---

        public void RegisterOwner(string membershipId, string firstName, string lastName, string email)
        {
            var ownerId = _ownerDataLoader.RegisterOwner(membershipId, firstName, lastName, email);

            SetSessionVariables(ownerId, 
                                string.Format("{0} {1}", firstName, lastName), 
                                membershipId,
                                GetDefaultImageProfile());
        }

        public void LoadSignedInOwnerInSession()
        {
            var owner = _ownerDataLoader.GetuserByMembershipId(User.Identity.GetUserId());

            SetSessionVariables(owner.Id,
                                string.Format("{0} {1}", owner.FirstName, owner.LastName),
                                User.Identity.GetUserId(),
                                string.IsNullOrEmpty(owner.ProfileImageUrl) ? GetDefaultImageProfile() : FormatSiteImageUrl(owner.ProfileImageUrl));
        }

        private void SetSessionVariables(int ownerId, string ownerName, string membershipId, string profileImageUrl)
        {
            Session["OwnerId"] = ownerId;
            Session["OwnerName"] = ownerName;
            Session["OwnerMembershipId"] = membershipId;
            Session["OwnerProfilePictureUrl"] = profileImageUrl;
        }

        public void CleanSessionVariables()
        {
            Session["OwnerName"] = null;
            Session["OwnerId"] = null;
            Session["OwnerMembershipId"] = null;
            Session["OwnerProfilePictureUrl"] = null;
        }

        #endregion

        #region --- GetuserByMembershipId ---

        public Owner GetuserByMembershipId()
        {
            return _ownerDataLoader.GetuserByMembershipId(User.Identity.GetUserId());
        }

        #endregion

        #region --- Updates Owner ---

        public void UpdateOwnerById(ProfileViewModel model)
        {
            model.Id = (int)Session["OwnerId"];
            _ownerDataLoader.UpdateOwner(model);
        }

        public void UpdateOwnerImageProfile(string imagePath)
        {
            var ownerId = (int)Session["OwnerId"];
            _ownerDataLoader.UpdateOwnerImageProfile(ownerId, imagePath);
        }

        #endregion

        #region --- Set/Get SessionVariables ---

        public string GetSessionOwnerName()
        {
            return Session["OwnerName"].ToString();
        }

        public void SetSessionOwnerName(string firstName, string lastName)
        {
            Session["OwnerName"] = string.Format("{0} {1}", firstName, lastName);
        }

        public string GetSessionOwnerProfilePictureUrl()
        {
            return Session["OwnerProfilePictureUrl"].ToString();
        }

        public void SetSessionOwnerProfilePictureUrl(string profilePictureUrl)
        {
            Session["OwnerProfilePictureUrl"] = FormatSiteImageUrl(profilePictureUrl);
        }

        #endregion

        #region --- ManageNavBar ---

        public void SetManageNavBarInfo()
        {
            ViewBag.FullName = this.GetSessionOwnerName();
            ViewBag.ProfilePictureUrl = this.GetSessionOwnerProfilePictureUrl();
        }

        public void SetManageNavBarItem(string itemName)
        {
            ViewBag.SelectedItem = itemName;
        }

        #endregion 

        #region --- Helpers ---

        private string GetDefaultImageProfile()
        {
            return ConfigurationManager.AppSettings["DefaultImageProfile"].ToString();
        }

        private string FormatSiteImageUrl(string imageUrl)
        {
            var uploadsFolder = ConfigurationManager.AppSettings["UploadsFolder"].ToString().Replace("/","\\");
            var index = imageUrl.IndexOf(uploadsFolder);

            if (index >= 0)
                imageUrl = imageUrl.Substring(index, (imageUrl.Length - index));

            return imageUrl;
        }

        #endregion
    }
}