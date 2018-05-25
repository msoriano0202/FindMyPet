using FindMyPet.DTO.Owner;
using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Models.Profile;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace FindMyPet.MVC.Controllers
{
    public class BaseController : Controller
    {
        private OwnerDataLoader _ownerDataLoader;

        public BaseController() : this(new OwnerDataLoader())
        {

        }

        public BaseController(OwnerDataLoader ownerDataLoader)
        {
            if (ownerDataLoader == null)
                throw new ArgumentNullException(nameof(ownerDataLoader));

            _ownerDataLoader = ownerDataLoader;
        }

        public Owner GetuserByMembershipId()
        {
            var userMembershipId = User.Identity.GetUserId();
            return _ownerDataLoader.GetuserByMembershipId(userMembershipId);
        }

        public void UpdateOwnerById(ProfileViewModel model)
        {
            model.Id = (int)Session["OwnerId"];
            _ownerDataLoader.UpdateOwner(model);
        }

        public void RegisterOwner(string membershipId, string firstName, string lastName, string email)
        {
            var ownerId = _ownerDataLoader.RegisterOwner(membershipId, firstName, lastName, email);

            Session["OwnerId"] = ownerId;
            Session["OwnerName"] = string.Format("{0} {1}", firstName, lastName);
            Session["OwnerMembershipId"] = membershipId;
        }

        public void LoadOwnerByEmail(string email)
        {
            var owner = _ownerDataLoader.GetOwnerByEmail(email);

            Session["OwnerId"] = owner.Id;
            Session["OwnerName"] = string.Format("{0} {1}", owner.FirstName, owner.LastName);
            Session["OwnerMembershipId"] = User.Identity.GetUserId();
        }

        public void CleanSessionVariables()
        {
            Session["OwnerName"] = null;
            Session["OwnerId"] = null;
            Session["OwnerMembershipId"] = null;
        }

        public void VerifySessionVariables()
        {
            var ownerMembershipId = Session["OwnerMembershipId"]?.ToString();
            if (string.IsNullOrEmpty(ownerMembershipId) && Request.IsAuthenticated)
            {
                var userMembershipId = User.Identity.GetUserId();
                var owner = _ownerDataLoader.GetuserByMembershipId(userMembershipId);

                Session["OwnerId"] = owner.Id;
                Session["OwnerName"] = string.Format("{0} {1}", owner.FirstName, owner.LastName);
                Session["OwnerMembershipId"] = userMembershipId;
            }
        }

        public void UpdateSessionOwnerName(string firstName, string lastName)
        {
            Session["OwnerName"] = string.Format("{0} {1}", firstName, lastName);
        }

        public string GetSessionOwnerName()
        {
            return Session["OwnerName"].ToString();
        }

        public void SetManageNavBarInfo()
        {
            ViewBag.FullName = this.GetSessionOwnerName();
            ViewBag.ProfilePictureUrl = "/Content/Images/DefaultProfileOwnerImage.png";
        }

        public void SelectMenuItemInProfilePage(string itemName)
        {
            ViewBag.SelectedItem = itemName;
        }
    }
}