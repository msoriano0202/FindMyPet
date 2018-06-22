using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Mappers;
using FindMyPet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindMyPet.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly IAdminDataLoader _adminDataLoader;
        private readonly IAdminMapper _adminMapper;

        public AdminController(IAdminDataLoader adminDataLoader, IAdminMapper adminMapper)
        {
            if (adminDataLoader == null)
                throw new ArgumentNullException(nameof(adminDataLoader));

            if (adminMapper == null)
                throw new ArgumentNullException(nameof(adminMapper));

            _adminDataLoader = adminDataLoader;
            _adminMapper = adminMapper;
        }

        // GET: Admin
        public ActionResult Index()
        {
            this.VerifySessionVariables();

            SetAdminNavBarInfo("Index");
            return View();
        }

        public ActionResult ManagePublicComments()
        {
            this.VerifySessionVariables();

            var comments = _adminDataLoader.GetFoundAlertsToApprove();
            var model = comments.ConvertAll(x => _adminMapper.AdminFoundAlertToViewModel(x));

            SetAdminNavBarInfo("ManagePublicComments");
            return View(model);
        }

        public ActionResult ApproveComment(string id)
        {
            var response = _adminDataLoader.ManageComent(id, (int)ApproveStatusEnum.Approved);
            return RedirectToAction("ManagePublicComments");
        }

        public ActionResult RejectComment(string id)
        {
            var response = _adminDataLoader.ManageComent(id, (int)ApproveStatusEnum.Rejected);
            return RedirectToAction("ManagePublicComments");
        }
    }
}