using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Models.Pet;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace FindMyPet.MVC.Controllers
{
    [Authorize]
    public class PetController : BaseController
    {
        private readonly IPetDataLoader _petDataLoader;

        public PetController(IPetDataLoader petDataLoader)
        {
            if (petDataLoader == null)
                throw new ArgumentNullException(nameof(petDataLoader));

            _petDataLoader = petDataLoader;
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
            return View();
        }

        // POST: Pet/Create
        [HttpPost]
        public ActionResult Create(PetViewModel model)
        {
            try
            {
                _petDataLoader.AddPet(User.Identity.GetUserId(), model);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pet/Edit/5
        public ActionResult Edit(string id)
        {
            var model = _petDataLoader.GetPetByCode(id);

            return View(model);
        }

        // POST: Pet/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, PetViewModel model)
        {
            try
            {
                model.Code = id;
                _petDataLoader.UpdatePet(model);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
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
