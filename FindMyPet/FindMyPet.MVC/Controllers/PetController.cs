using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.Models.Pet;
using FindMyPet.MVC.ServiceClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using FindMyPet.MVC.DataLoaders;

namespace FindMyPet.MVC.Controllers
{
    [Authorize]
    public class PetController : Controller
    {
        private readonly IPetDataLoader _petDataLoader;

        public PetController(IPetDataLoader petDataLoader)
        {
            if (petDataLoader == null)
                throw new ArgumentNullException(nameof(petDataLoader));

            _petDataLoader = petDataLoader;
        }

        // GET: Pet
        public ActionResult Index()
        {
            var model = _petDataLoader.GetPetsByOwner(User.Identity.GetUserId());

            return View(model);
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
        public ActionResult Delete(int id)
        {
            return View();
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
