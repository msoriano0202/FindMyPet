using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.Models.PetSearch;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindMyPet.MVC.Controllers
{
    public class PetSearchController : BaseController
    {
        private readonly IPetSearchDataLoader _petSearchDataLoader;
        private readonly IPetSearchMapper _petSearchMapper;

        public PetSearchController(IPetSearchDataLoader petSearchDataLoader, IPetSearchMapper petSearchMapper)
        {
            if (petSearchDataLoader == null)
                throw new ArgumentNullException(nameof(petSearchDataLoader));

            if (petSearchMapper == null)
                throw new ArgumentNullException(nameof(petSearchMapper));

            _petSearchDataLoader = petSearchDataLoader;
            _petSearchMapper = petSearchMapper;
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

        public ActionResult PublicProfile(string id)
        {
            var data = _petSearchDataLoader.GetPetLostDetails(Guid.Parse(id));
            var model = _petSearchMapper.GetPetPublicProfileViewModel(data);

            model.PetInfo.ProfileImageUrl = this.GetPetImageProfile(model.PetInfo.ProfileImageUrl);
            foreach (var owner in model.OwnersInfo)
            {
                owner.ProfileImageUrl = this.GetOwnerImageProfile(owner.ProfileImageUrl);
            }

            return View(model);
        }

        public ActionResult SuccessStories(int? page)
        {
            this.VerifySessionVariables();
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"].ToString());
            page = page ?? 1;

            var result = _petSearchDataLoader.GetPetSuccessStories(pageSize, page.Value);
            if (page < 0) page = 1;
            else if (page > result.TotalPages) page = result.TotalPages;

            var pagedModel = new PetSuccessStoryPagedListViewModel
            {
                Records = result.Result,
                Pagination = this.SetPaginationViewModel("/PetSearch/SuccessStories?page=", result.TotalRecords, result.TotalPages, page.Value, pageSize)
            };

            return View(pagedModel);
        }
    }
}