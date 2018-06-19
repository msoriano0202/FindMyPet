using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Models.PetSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindMyPet.MVC.Controllers
{
    public class PetSearchController : BaseController
    {
        private readonly IPetSearchDataLoader _petSearchDataLoader;

        public PetSearchController(IPetSearchDataLoader petSearchDataLoader)
        {
            if (petSearchDataLoader == null)
                throw new ArgumentNullException(nameof(petSearchDataLoader));

            _petSearchDataLoader = petSearchDataLoader;
        }

        public ActionResult Index()
        {
            this.VerifySessionVariables();

            var today = DateTime.Now;
            var fromDT = today.AddDays(-7).Date;
            var from = new DateTime(fromDT.Year, fromDT.Month, fromDT.Day, 0, 0, 0);
            var to = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59);

            var result = _petSearchDataLoader.SearchLostPets(from, to);
            var model = new PetSearchViewModel
            {
                FromDate = from, 
                ToDate = to,
                Points = result.ConvertAll(x => new PointAlertViewModel
                {
                    PetId = x.PetId.Value,
                    PetCode = x.PetCode.ToString(),
                    PetName = x.PetName,
                    PetProfileImageUrl = this.GetPetImageProfile(x.PetProfileImageUrl),
                    LostDateTime = x.LostDateTime.ToString("dd / MMM / yyyy hh:mm:ss tt"),
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                })
            };

            return View(model);
        }
    }
}