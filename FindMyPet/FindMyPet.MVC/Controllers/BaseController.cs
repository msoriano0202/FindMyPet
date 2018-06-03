using FindMyPet.DTO.Owner;
using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Helpers;
using FindMyPet.MVC.Models.Profile;
using FindMyPet.MVC.Models.Shared;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace FindMyPet.MVC.Controllers
{
    public class BaseController : Controller
    {
        private IOwnerDataLoader _ownerDataLoader;
        private IImageHelper _imageHelper;

        public BaseController() : this(new OwnerDataLoader(), new ImageHelper())
        { }

        public BaseController(IOwnerDataLoader ownerDataLoader, IImageHelper imageHelper)
        {
            if (ownerDataLoader == null)
                throw new ArgumentNullException(nameof(ownerDataLoader));

            if (imageHelper == null)
                throw new ArgumentNullException(nameof(imageHelper));

            _ownerDataLoader = ownerDataLoader;
            _imageHelper = imageHelper;
        }

        #region --- VerifySessionVariables ---

        public void VerifySessionVariables()
        {
            int ownerId = 0;
            var ownerIdSession = Session["OwnerId"]?.ToString();
            if (
                (ownerIdSession == null || !Int32.TryParse(ownerIdSession, out ownerId)) && Request.IsAuthenticated
               )
            {
                var membershipId = User.Identity.GetUserId();
                var owner = _ownerDataLoader.GetOwnerByMembershipId(membershipId);

                SetSessionVariables(owner.Id,
                                    string.Format("{0} {1}", owner.FirstName, owner.LastName),
                                    User.Identity.GetUserId(),
                                    string.IsNullOrEmpty(owner.ProfileImageUrl) ? GetDefaultImageProfile() : FormatSiteImageUrl(owner.ProfileImageUrl));
            }
        }

        #endregion

        #region --- Register Owner / Load Owner / Clean ---

        public void RegisterOwner(string membershipId, string firstName, string lastName, string email)
        {
            var owner = _ownerDataLoader.RegisterOwner(membershipId, firstName, lastName, email);

            SetSessionVariables(owner.Id, 
                                string.Format("{0} {1}", firstName, lastName), 
                                membershipId,
                                GetDefaultImageProfile());
        }

        //public void LoadSignedInOwnerInSession()
        //{
        //    var membershipId = User.Identity.GetUserId();
        //    var owner = _ownerDataLoader.GetOwnerByMembershipId(membershipId);

        //    SetSessionVariables(owner.Id,
        //                        string.Format("{0} {1}", owner.FirstName, owner.LastName),
        //                        User.Identity.GetUserId(),
        //                        string.IsNullOrEmpty(owner.ProfileImageUrl) ? GetDefaultImageProfile() : FormatSiteImageUrl(owner.ProfileImageUrl));
        //}

        private void SetSessionVariables(int ownerId, string ownerName, string membershipId, string profileImageUrl)
        {
            Session["OwnerId"] = ownerId;
            Session["OwnerName"] = ownerName;
            Session["OwnerMembershipId"] = membershipId;
            Session["OwnerProfilePictureUrl"] = profileImageUrl;
        }

        public void CleanSessionVariables()
        {
            Session.Abandon();
            Session["OwnerName"] = null;
            Session["OwnerId"] = null;
            Session["OwnerMembershipId"] = null;
            Session["OwnerProfilePictureUrl"] = null;
        }

        #endregion

        #region --- GetuserByMembershipId ---

        public Owner GetUserByMembershipId()
        {
            return _ownerDataLoader.GetOwnerByMembershipId(User.Identity.GetUserId());
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

        public int GetSessionOwnerId()
        {
            return Convert.ToInt32(Session["OwnerId"]);
        }

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

        #region --- Private Helpers ---

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

        #region --- Image helper ---

        public void PerformImageResizeAndPutOnCanvas(string pFilePath, string pFileName, int pWidth, int pHeight, string pOutputFileName)
        {
            _imageHelper.PerformImageResizeAndPutOnCanvas(pFilePath, pFileName, pWidth, pHeight, pOutputFileName);
        }

        #endregion

        #region --- Pagination ---

        public PaginationViewModel SetPaginationViewModel(string actionUrl, int totalRecords, int totalPages, int currentPage, int pageSize)
        {
            return new PaginationViewModel
            {
                ActionUrl = actionUrl,
                EnablePagination = (totalPages > 1),
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize
            };
        }

        #endregion
    }
}