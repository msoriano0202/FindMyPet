using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.Models.Pet;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using FindMyPet.Shared;
using System.Linq;
using FindMyPet.MVC.Models.Shared;

namespace FindMyPet.MVC.Controllers
{
    [Authorize(Roles = "Owner,Admin")]
    public class PetController : BaseController
    {
        private readonly IPetDataLoader _petDataLoader;
        private readonly IPetMapper _petMapper;
        private readonly IPetImageMapper _petImageMapper;

        public PetController(IPetDataLoader petDataLoader, IPetMapper petMapper, IPetImageMapper petImageMapper)
        {
            if (petDataLoader == null)
                throw new ArgumentNullException(nameof(petDataLoader));

            if (petMapper == null)
                throw new ArgumentNullException(nameof(petMapper));

            if (petImageMapper == null)
                throw new ArgumentNullException(nameof(petImageMapper));

            _petDataLoader = petDataLoader;
            _petMapper = petMapper;
            _petImageMapper = petImageMapper;
        }

        public ActionResult Index(int? page)
        {
            this.VerifySessionVariables();
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"].ToString());
            page = page ?? 1;
           
            var result = _petDataLoader.GetPetsPagedByOwner(this.GetSessionOwnerId(), pageSize, page.Value);
            if (page < 0) page = 1;
            else if (page > result.TotalPages) page = result.TotalPages;

            var pagedModel = new PetPagedListViewModel
            {
                Records = result.Result,
                Pagination = this.SetPaginationViewModel("/Pet/?page=", result.TotalRecords, result.TotalPages, page.Value, pageSize)
            };

            this.SetAlertMessageInViewBag();
            return View(pagedModel);
        }

        public ActionResult Create()
        {
            var now = System.DateTime.Now;
            var model = new PetProfileViewModel()
            {
                DateOfBirth = now.Date
            };

            this.SetAlertMessageInViewBag();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PetProfileViewModel model)
        {
            try
            {
                var pet = _petDataLoader.AddPet(User.Identity.GetUserId(), model);

                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Success, "Su Mascota ha sido creada.");
                return RedirectToAction("PetProfile", new { id = pet.Code});
            }
            catch(Exception ex)
            {
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, ex.Message);
                return RedirectToAction("Create");
            }
        }

        public ActionResult PetProfile(string id)
        {
            this.VerifySessionVariables();

            var pet = _petDataLoader.GetPetByCode(id);
            var model = _petMapper.PetToProfileViewModel(pet);
            SetPetProfileNavBarInfo(pet, "PetProfile");

            this.SetAlertMessageInViewBag();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PetProfile(PetProfileViewModel model)
        {
            try
            {
                _petDataLoader.UpdatePet(model);
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Success, "Su Mascota ha sido actualizada.");
            }
            catch (Exception ex)
            {
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, ex.Message);
            }

            return RedirectToAction("PetProfile", new { id = model.Code });
        }

        public ActionResult PetAlbum(string id)
        {
            this.VerifySessionVariables();

            var pet = _petDataLoader.GetPetByCode(id);
            var model = pet.Images.ConvertAll(x => _petImageMapper.PetImageToPetImageViewModel(x));
            SetPetProfileNavBarInfo(pet, "PetAlbum");

            this.SetAlertMessageInViewBag();
            return View(model);
        }

        [HttpPost]
        public ActionResult UploadPetProfileImage(string id, string an)
        {
            if (Request.Files.Count > 0)
            {
                var message = string.Empty;

                var file = Request.Files[0];
                if (file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName))
                    message = "NoFile";
                else if (!ValidImageExtension(file.FileName))
                    message = "FileNoValid";
                else
                {
                    var newImageFilePath = this.ResizeAndSaveImage(file);
                    _petDataLoader.AddPetImage(id, newImageFilePath, true);
                }
            }

            //Micky: Show messahe when there is no profile image selected or bad extension
            //else { }

            return RedirectToAction(an, new { id = id });
        }

        [HttpPost]
        public ActionResult UploadPetImage(string id)
        {
            if (Request.Files.Count > 0)
            {
                var message = string.Empty;

                var file = Request.Files[0];
                if (file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName))
                    message = "NoFile";
                else if (!ValidImageExtension(file.FileName))
                    message = "FileNoValid";
                else
                {
                    var newImageFilePath = this.ResizeAndSaveImage(file);
                    _petDataLoader.AddPetImage(id, newImageFilePath, false);
                }
            }

            //Micky: Show messahe when there is no profile image selected or bad extension
            //else { }

            return RedirectToAction("PetAlbum", new { id = id });
        }

        public ActionResult Delete(string id)
        {
            try
            {
                // Micky delete en cascada
                _petDataLoader.DeletePet(id);
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Success, "Su Mascota ha sido borrada.");
            }
            catch (Exception ex)
            {
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, ex.Message);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteImage(string code, string id)
        {
            try
            {
                var result = _petDataLoader.DeletePetImage(id);
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Success, "La imagen ha sido borrada.");
            }
            catch (Exception ex)
            {
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, ex.Message);
            }
            
            return RedirectToAction("PetAlbum", new { id = code });
        }

        public ActionResult SetImageProfile(string code, string id)
        {
            var result = _petDataLoader.SetPetImageAsDefault(id);

            return RedirectToAction("PetAlbum", new { id = code });
        }

        public ActionResult PetShare(string id)
        {
            this.VerifySessionVariables();

            var pet = _petDataLoader.GetPetByCode(id);
            var model = _petMapper.PetToPetShareViewModel(pet);
            SetPetProfileNavBarInfo(pet, "PetShare");

            this.SetAlertMessageInViewBag();
            return View(model);
        }

        [HttpPost]
        public ActionResult PetShare(string id, string Email)
        {
            try
            {
                var ownerId = this.GetSessionOwnerId();
                var pet = _petDataLoader.GetPetByCode(id);
                var token = _petDataLoader.CreateSharePetToken(ownerId, id, Email);

                var callbackUrl = Url.Action("ConfirmPetShare", "Pet", new { code = token }, protocol: Request.Url.Scheme);
                var body = $"Por favor, haga click en este link para compartir ser propietario de la mascota: {callbackUrl}";
                _emailHelper.SendEmailSharePet(Email, pet.Name, body);

                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Success, "Se ha enviado un correo a la direccion ingresada .");
            }
            catch (Exception ex)
            {
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, ex.Message);
            }

            return RedirectToAction("PetShare", new { id = id });
        }

        public ActionResult ConfirmPetShare(string code)
        {
            try
            {
                var result = _petDataLoader.ConfirmSharePet(code);
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Success, "La Mascota ha sido compartida.");
            }
            catch (Exception ex)
            {
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, ex.Message);
            }

            return RedirectToAction("Index");
        }

        public ActionResult PetAlert(string id)
        {
            this.VerifySessionVariables();

            var pet = _petDataLoader.GetPetByCode(id);
            var activeAlert = pet.Alerts.SingleOrDefault(a => a.Status == (int)AlertStatusEnum.Active);

            SetPetProfileNavBarInfo(pet, "PetAlert");
            var model = new PetAlertViewModel() { PetCode = id };

            this.SetAlertMessageInViewBag();
            if (activeAlert == null)   
                return View(model);
            else
                return View("PetFound", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PetAlert(PetAlertViewModel model)
        {
            if (!model.Latitude.HasValue && !model.Longitude.HasValue)
            {
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, "Su ubicacion no puede ser determinada.");
                return RedirectToAction("PetAlert", new { id = model.PetCode });
            }

            this.VerifySessionVariables();

            model.OwnerId = this.GetSessionOwnerId();
            model.Type = (int)AlertTypeEnum.Lost;
            model.StaticMapUrl = SaveStaticGoogleMap(model.StaticMapUrl);

            try
            {
                var petAlert = _ownerDataLoader.AddPetAlert(model);
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Success, "La alerta ha sido guardada.");
            }
            catch (Exception ex)
            {
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, ex.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PetFound(string id, PetAlertViewModel model)
        {
            this.VerifySessionVariables();

            model.OwnerId = this.GetSessionOwnerId();
            model.PetCode = id;

            try
            {
                var petAlert = _ownerDataLoader.FoundPet(model);
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Success, "Nos alegra que haya encontrado a su Mascota!!!");
            }
            catch (Exception ex)
            {
                this.SetAlertMessageInTempData(AlertMessageTypeEnum.Error, ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
