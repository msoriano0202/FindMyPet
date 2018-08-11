using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Helpers;
using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.Models.Admin;
using FindMyPet.Shared;
using Postal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            var details = _adminDataLoader.GetDashboardDetails();
            var model = new DashboardDetailsViewModel
            {
                RegisteredAccounts = details.RegisteredAccounts,
                RegisteredPets = details.RegisteredPets,
                LostPets = details.LostPets,
                FoundPets = details.FoundPets,
                AlertsToReview = details.AlertsToReview,
                CommentsToApprove = details.CommentsToApprove,
                SuccessStories = details.SuccessStories,
            };

            SetAdminNavBarInfo("Index");
            return View(model);
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

        public ActionResult ManageReportedAlerts()
        {
            this.VerifySessionVariables();

            var reportedAlerts = _adminDataLoader.GetReportedAlertsToApprove();
            var model = reportedAlerts.ConvertAll(x => _adminMapper.AdminReportedAlertToViewModel(x));

            SetAdminNavBarInfo("ManageReportedAlerts");
            return View(model);
        }

        public ActionResult ApproveAlert(string id)
        {
            var response = _adminDataLoader.ManageReportedAlerts(id, (int)AlertStatusEnum.Active);
            return RedirectToAction("ManageReportedAlerts");
        }

        public ActionResult RejectAlert(string id)
        {
            var response = _adminDataLoader.ManageReportedAlerts(id, (int)AlertStatusEnum.Deleted);
            return RedirectToAction("ManageReportedAlerts");
        }

        private List<SelectListItem> GetEmailTypes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Register Welcome", Value = ((int)EmailTypeEnum.Welcome).ToString() },
                new SelectListItem { Text = "ReSend Confirmation", Value = ((int)EmailTypeEnum.ReSendConfirmation).ToString() },
                new SelectListItem { Text = "Reset Password", Value = ((int)EmailTypeEnum.ResetPassword).ToString() },
                new SelectListItem { Text = "Share Pet", Value = ((int)EmailTypeEnum.PetShare).ToString() }
            };
        }

        private List<SelectListItem> GetViewTypes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Browser", Value = ((int)EmailViewerTypeEnum.Browser).ToString() },
                new SelectListItem { Text = "Email", Value = ((int)EmailViewerTypeEnum.Email).ToString() },
            };
        }

        public ActionResult EmailSender()
        {
            this.VerifySessionVariables();

            var model = new EmailSenderViewModel
            {
                EmailTypes = new SelectList(GetEmailTypes(), "Value", "Text"),
                ViewTypes = new SelectList(GetViewTypes(), "Value", "Text")
            };

            this.SetAlertMessageInViewBag();
            SetAdminNavBarInfo("EmailSender");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EmailSender(EmailSenderViewModel model)
        {

            Postal.Email email = null;
            string callbackUrl;

            switch (model.SelectedEmailTypeId)
            {
                case (int)EmailTypeEnum.Welcome:
                    callbackUrl = "http://localhost:8081/Account/ConfirmEmail?userId=0000000000&code=0000000000";
                    email = await _postalEmailHelper.SendConfirmationEmailAsync(model.Email, "Miguel Soriano", "password", callbackUrl, false);
                    break;
                case (int)EmailTypeEnum.ReSendConfirmation:
                    callbackUrl = "http://localhost:8081/Account/ConfirmEmail?userId=0000000000&code=0000000000";
                    email = await _postalEmailHelper.ResendConfirmationEmailAsync(model.Email, callbackUrl, false);
                    break;
                case (int)EmailTypeEnum.ResetPassword:
                    callbackUrl = "http://localhost:8081/Account/ConfirmEmail?userId=0000000000&code=0000000000";
                    email = await _postalEmailHelper.SendResetPasswordEmailAsync(model.Email, "Miguel Soriano", callbackUrl, false);
                    break;
                case (int)EmailTypeEnum.PetShare:
                    callbackUrl = "http://localhost:8081/Account/ConfirmEmail?userId=0000000000&code=0000000000";
                    email = await _postalEmailHelper.SendShareEmailEmailAsync(model.Email, "Marti", callbackUrl, false);
                    break;
            }

            switch (model.SelectedViewTypeId)
            {
                case (int)EmailViewerTypeEnum.Browser:
                    return new EmailViewResult(email);
                case (int)EmailViewerTypeEnum.Email:
                    email.Send();
                    break;
            }

            this.SetAlertMessageInTempData(Models.Shared.AlertMessageTypeEnum.Success, "El correo fue enviado.");
            return RedirectToAction("EmailSender");
        }
    }
}