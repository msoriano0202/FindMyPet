using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FindMyPet.MVC.Models.Home;
using FindMyPet.MVC.Models.Shared;
using FindMyPet.MVC.ServiceClients;
using FindMyPet.Shared;
using Microsoft.AspNet.Identity;

namespace FindMyPet.MVC.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            this.VerifySessionVariables();

            this.SetAlertMessageInViewBag();
            return View();
        }

        private List<SelectListItem> GetAlertTypes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Seleccionar ->", Value = null },
                new SelectListItem { Text = "Perdido", Value = ((int)AlertTypeEnum.Lost).ToString() },
                new SelectListItem { Text = "Abandonado", Value = ((int)AlertTypeEnum.Abandom).ToString() },
                new SelectListItem { Text = "Herido", Value = ((int)AlertTypeEnum.Injured).ToString() },
                new SelectListItem { Text = "Encontrado", Value = ((int)AlertTypeEnum.Found).ToString() },
                new SelectListItem { Text = "En Adopcion", Value = ((int)AlertTypeEnum.Adoption).ToString() }
            };
        }

        public ActionResult Alert()
        {
            this.VerifySessionVariables();

            var model = new PetPublicAlertViewModel
            {
                AlertTypes = GetAlertTypes()
            };

            if (Request.IsAuthenticated)
                model.OwnerId = this.GetSessionOwnerId();

            this.SetAlertMessageInViewBag();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alert(PetPublicAlertViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!model.Latitude.HasValue && !model.Longitude.HasValue)
            {
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, "Su ubicacion no puede ser determinada.");
                return RedirectToAction("Alert");
            }

            this.VerifySessionVariables();

            if (User.Identity.IsAuthenticated)
                model.OwnerId = this.GetSessionOwnerId();

            var files = model.Images.ToList();
            if (files.Count > 0)
            {
                if (!ValidFileExtensions(files))
                {
                    this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, "Solo estan permitidos archivos .jpg y .png");
                    return RedirectToAction("Alert");
                }
                else
                {
                    var urlImages = new List<string>();
                    model.StaticMapUrl = SaveStaticGoogleMap(model.StaticMapUrl);

                    foreach (var file in files)
                    {
                        var newImageFilePath = this.ResizeAndSaveImage(file);
                        urlImages.Add(newImageFilePath);
                    }

                    try
                    {
                        var petAlert = _ownerDataLoader.AddPetPublicAlert(model, urlImages);
                        this.SetAlertMessageInTempData(AlertMessageTypeEnum.Success, "La alerta ha sido guardada.");
                    }
                    catch (Exception ex)
                    {
                        this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, ex.Message);
                    }
                }
            }

            return RedirectToAction("Alert");
        }

        private bool ValidFileExtensions(List<HttpPostedFileBase> files)
        {
            var result = true;
            foreach (var file in files)
            {
                if (!ValidImageExtension(file.FileName))
                {
                    result = false;
                    break;
                }
                
            }

            return result;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}