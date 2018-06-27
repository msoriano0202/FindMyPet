using FindMyPet.MVC.Helpers;
using FindMyPet.MVC.Models.EmailSender;
using FindMyPet.Shared;
using Postal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FindMyPet.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmailSenderController : BaseController
    {
        private readonly IPostalEmailHelper _postalEmailHelper;

        public EmailSenderController(IPostalEmailHelper postalEmailHelper)
        {
            if (postalEmailHelper == null)
                throw new ArgumentNullException(nameof(postalEmailHelper));

            _postalEmailHelper = postalEmailHelper;
        }

        private List<SelectListItem> GetEmailTypes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Register Welcome", Value = ((int)EmailTypeEnum.Welcome).ToString() },
                new SelectListItem { Text = "ReSend Confirmation", Value = ((int)EmailTypeEnum.ReSendConfirmation).ToString() },
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

        public ActionResult Index()
        {
            this.VerifySessionVariables();

            var model = new EmailSenderViewModel
            {
                EmailTypes = new SelectList(GetEmailTypes(), "Value", "Text"),
                ViewTypes = new SelectList(GetViewTypes(), "Value", "Text")
            };

            this.SetAlertMessageInViewBag();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(EmailSenderViewModel model)
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
                case (int)EmailTypeEnum.PetShare:
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
            return RedirectToAction("Index");
        }
    }
}