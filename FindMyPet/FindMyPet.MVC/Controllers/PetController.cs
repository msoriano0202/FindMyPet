using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.Models.Pet;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

namespace FindMyPet.MVC.Controllers
{
    [Authorize]
    public class PetController : BaseController
    {
        private readonly IPetDataLoader _petDataLoader;
        private readonly IPetMapper _petMapper;

        public PetController(IPetDataLoader petDataLoader, IPetMapper petMapper)
        {
            if (petDataLoader == null)
                throw new ArgumentNullException(nameof(petDataLoader));

            if (petMapper == null)
                throw new ArgumentNullException(nameof(petMapper));

            _petDataLoader = petDataLoader;
            _petMapper = petMapper;
        }

        // GET: Pet
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

            return View(pagedModel);
        }

        // GET: Pet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Pet/Create
        public ActionResult Create()
        {
            var now = System.DateTime.Now;
            var model = new PetProfileViewModel()
            {
                DateOfBirth = now.Date
            };

            return View(model);
        }

        // POST: Pet/Create
        [HttpPost]
        public ActionResult Create(PetProfileViewModel model)
        {
            try
            {
                var pet = _petDataLoader.AddPet(User.Identity.GetUserId(), model);

                return RedirectToAction("PetProfile", new { id = pet.Code});
            }
            catch
            {
                return View();
            }
        }

        public ActionResult PetProfile(string id)
        {
            this.VerifySessionVariables();

            var pet = _petDataLoader.GetPetByCode(id);
            var model = _petMapper.PetToProfileViewModel(pet);
            SetPetProfileNavBarInfo(pet, "PetProfile");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PetProfile(string id, PetProfileViewModel model)
        {
            try
            {
                model.Code = id;
                _petDataLoader.UpdatePet(model);

                return RedirectToAction("PetProfile", new { id = id});
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult UploadPetProfileImage(string id)
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
                    var uploadsFolder = Server.MapPath(ConfigurationManager.AppSettings["UploadsFolder"].ToString());
                    var tempFileName = string.Format("{0}.{1}", Guid.NewGuid().ToString(), GetFileExtension(file.FileName));
                    var newFileName = string.Format("{0}.{1}", Guid.NewGuid().ToString(), this.defaultImageExtension);

                    var tempImageFilePath = Path.Combine(uploadsFolder, tempFileName);
                    var newImageFilePath = Path.Combine(uploadsFolder, newFileName);

                    file.SaveAs(tempImageFilePath);
                    this.PerformImageResizeAndPutOnCanvas(uploadsFolder, tempFileName, this.defaultImageWidthSize, this.defaultImageHeightSize, newFileName);
                    _petDataLoader.AddPetImage(id, newImageFilePath, true);

                    System.IO.File.Delete(tempImageFilePath);
                }
            }

            //Micky: Show messahe when there is no profile image selected or bad extension
            //else { }

            return RedirectToAction("PetProfile", new { id = id });
        }

        // GET: Pet/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                    _petDataLoader.DeletePet(id);
            }
            catch (Exception ex) { }

            return RedirectToAction("Index");
        }

        // POST: Pet/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
