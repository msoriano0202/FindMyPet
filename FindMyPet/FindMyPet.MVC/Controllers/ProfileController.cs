using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Models.Profile;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindMyPet.MVC.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        private readonly IOwnerDataLoader _ownerDataLoader;

        public ProfileController(IOwnerDataLoader ownerDataLoader)
        {
            if (ownerDataLoader == null)
                throw new ArgumentNullException(nameof(ownerDataLoader));

            _ownerDataLoader = ownerDataLoader;
        }

        // GET: Profile
        public ActionResult Index()
        {
            VerifySessionVariables();

            var owner = _ownerDataLoader.GetuserByMembershipId(User.Identity.GetUserId());

            var model = new ProfileViewModel();
            model.FirstName = owner.FirstName;
            model.LastName = owner.LastName;
            model.Email = owner.Email;

            ViewBag.FullName = GetSessionOwnerName();
            ViewBag.ProfilePictureUrl = "/Content/Images/DefaultProfileOwnerImage.png";
            //"http://localhost:8081/Content/Images/DefaultProfileOwnerImage.png";
            //http://via.placeholder.com/200x200/eee/777

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ProfileViewModel model)
        {
            try
            {
                model.Id = (int)Session["OwnerId"];
                _ownerDataLoader.UpdateOwner(model);
                UpdateSessionOwnerName(model.FirstName, model.LastName);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}