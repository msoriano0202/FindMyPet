using FindMyPet.DTO.Owner;
using FindMyPet.DTO.Pet;
using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Helpers;
using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.Models.Account;
using FindMyPet.MVC.Models.Shared;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using FindMyPet.Shared;

namespace FindMyPet.MVC.Controllers
{
    public class BaseController : Controller
    {
        protected IOwnerDataLoader _ownerDataLoader;
        private IImageHelper _imageHelper;
        protected IOwnerMapper _ownerMapper;
        private IGeneralHelper _generalHelper;
        protected IEmailHelper _emailHelper;

        private List<string> validImageExtensions = new List<string> { "jpg", "png" };
        protected int defaultImageWidthSize = 750;
        protected int defaultImageHeightSize = 750;
        protected string defaultImageExtension = "jpg";

        public BaseController() : this(new OwnerDataLoader(), new ImageHelper(), new OwnerMapper(), new GeneralHelper(), new EmailHelper())
        { }

        public BaseController(IOwnerDataLoader ownerDataLoader, IImageHelper imageHelper, IOwnerMapper ownerMapper, 
                              IGeneralHelper generalHelper, IEmailHelper emailHelper)
        {
            if (ownerDataLoader == null)
                throw new ArgumentNullException(nameof(ownerDataLoader));

            if (imageHelper == null)
                throw new ArgumentNullException(nameof(imageHelper));

            if (ownerMapper == null)
                throw new ArgumentNullException(nameof(ownerMapper));

            if (generalHelper == null)
                throw new ArgumentNullException(nameof(generalHelper));

            if (emailHelper == null)
                throw new ArgumentNullException(nameof(emailHelper));

            _ownerDataLoader = ownerDataLoader;
            _imageHelper = imageHelper;
            _ownerMapper = ownerMapper;
            _generalHelper = generalHelper;
            _emailHelper = emailHelper;
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
                                    User.Identity.GetUserId());
            }
        }

        #endregion

        #region --- Register Owner / Load Owner / Clean ---

        public void RegisterOwner(string membershipId, string firstName, string lastName, string email)
        {
            var owner = _ownerDataLoader.RegisterOwner(membershipId, firstName, lastName, email);

            SetSessionVariables(owner.Id, 
                                string.Format("{0} {1}", firstName, lastName),
                                membershipId);
        }

        private void SetSessionVariables(int ownerId, string ownerName, string membershipId)
        {
            Session["OwnerId"] = ownerId;
            Session["OwnerName"] = ownerName;
            Session["OwnerMembershipId"] = membershipId;
        }

        public void CleanSessionVariables()
        {
            Session.Abandon();
            Session["OwnerName"] = null;
            Session["OwnerId"] = null;
            Session["OwnerMembershipId"] = null;
        }

        #endregion

        #region --- GetuserByMembershipId ---

        public Owner GetUserByMembershipId(string userId)
        {
            return _ownerDataLoader.GetOwnerByMembershipId(userId);
        }

        #endregion

        #region --- Updates Owner ---

        public void UpdateOwnerById(ProfileViewModel model)
        {
            model.Id = (int)Session["OwnerId"];
            var owner = _ownerDataLoader.UpdateOwner(model);
        }

        public void UpdateSettingsOwnerByOwnerId(SettingsViewModel model)
        {
            model.OwnerId = (int)Session["OwnerId"];
            var owner = _ownerDataLoader.UpdateSettingsOwner(model);
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

        #endregion

        #region --- ManageNavBar ---

        public void SetManageNavBarInfo(Owner owner, string navItemSelected)
        {
            ViewBag.FullName = this.GetSessionOwnerName();
            ViewBag.ProfilePictureUrl = GetOwnerImageProfile(owner.ProfileImageUrl);
            ViewBag.SelectedItem = navItemSelected;
        }

        #endregion

        #region -- PetProfileNavBar --
        public void SetPetProfileNavBarInfo(Pet pet, string navItemSelected)
        {
            var activeAlert = pet.Alerts.SingleOrDefault(a => a.Status == (int)AlertStatusEnum.Active);

            ViewBag.PetCode = pet.Code;
            ViewBag.PetFullName = pet.Name;
            ViewBag.PetProfilePictureUrl = GetPetImageProfile(pet);
            ViewBag.PetSelectedItem = navItemSelected;

            ViewBag.PetHasActiveAlerts = (activeAlert != null) ? true : false;
        }
        #endregion

        #region --- Private Helpers ---

        private string GetOwnerImageProfile(string profileImage)
        {
            if (string.IsNullOrEmpty(profileImage))
                return GetDefaultImageProfile();
            else {
                return FormatSiteImageUrl(profileImage);
            }
        }

        private string GetPetImageProfile(Pet pet)
        {
            var imageUrl = GetDefaultPetImageProfile();

            if (pet.Images.Count > 0)
            {
                var profileImage = pet.Images.SingleOrDefault(x => x.IsProfileImage);
                if (profileImage != null)
                    imageUrl = FormatSiteImageUrl(profileImage.ImageUrl);
            }

            return imageUrl;
        }
        
        private string GetDefaultImageProfile()
        {
            return ConfigurationManager.AppSettings["DefaultImageOwnerProfile"].ToString();
        }

        private string GetDefaultPetImageProfile()
        {
            return ConfigurationManager.AppSettings["DefaultImagePetProfile"].ToString();
        }

        private string FormatSiteImageUrl(string imageUrl)
        {
            return _generalHelper.FormatSiteImageUrl(imageUrl);
        }

        #endregion

        #region --- Image helper ---

        public string GetPetImageProfile(string petImage)
        {
            if (string.IsNullOrEmpty(petImage))
                return GetDefaultPetImageProfile();
            else
                return FormatSiteImageUrl(petImage);
        }

        public bool ValidImageExtension(string fileName)
        {
            var result = false;

            var extension = GetFileExtension(fileName);
            if (validImageExtensions.Contains(extension))
                result = true;

            return result;
        }

        public string GetFileExtension(string fileName)
        {
            var imgArr = fileName.Split('.');
            return imgArr[imgArr.Length - 1].ToLower();
        }

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