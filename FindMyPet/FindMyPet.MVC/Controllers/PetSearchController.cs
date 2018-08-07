using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.Models.PetSearch;
using FindMyPet.Shared;
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

        private List<RadioButtonModel> GetMapRabioButtonList()
        {
            return new List<RadioButtonModel>
            {
                new RadioButtonModel { Value = (int)SearchOnMapOptionEnum.All , ElementId ="LastAlertsOpt0", DisplayText = "Todas", Selected = false },
                new RadioButtonModel { Value = (int)SearchOnMapOptionEnum.LastWeek , ElementId ="LastAlertsOpt1", DisplayText = "Última Semana", Selected = false },
                new RadioButtonModel { Value = (int)SearchOnMapOptionEnum.LastMonth , ElementId ="LastAlertsOpt2", DisplayText = "Última Mes", Selected = false },
                new RadioButtonModel { Value = (int)SearchOnMapOptionEnum.Custom , ElementId ="LastAlertsOpt3", DisplayText = "Buscar por Fechas", Selected = false }
            };
        }

        public ActionResult Index(int? op)
        {
            this.VerifySessionVariables();
            op = op ?? 0;
            if (op < 0 || op > 3)
                op = 0;

            DateTime? from; DateTime? to;
            var list = GetMapRabioButtonList();
            SetSelectedItem(op.Value, list);
            this.SetFromToBaseOnMapOption(op.Value, out from, out to);

            var model = new PetSearchViewModel
            {
                Options = list,
                From = from,
                To = to,
                Points = new List<PointAlertViewModel>()
            };

            if ((from.HasValue && to.HasValue) && (from.Value > to.Value))
                this.SetAlertMessageInTempData(Models.Shared.AlertMessageTypeEnum.Error, "Seleccione un rango valido de fechas.");
            else
            {
                var result = _petSearchDataLoader.SearchLostPets(from, to);
                model.Points = result.ConvertAll(x => new PointAlertViewModel
                {
                    AlertCode = x.AlertCode.ToString(),
                    PetId = x.PetId,
                    PetCode = x.PetCode?.ToString(),
                    PetName = x.PetName,
                    PetProfileImageUrl = this.GetPetImageProfile(x.PetProfileImageUrl),
                    LostDateTime = x.LostDateTime.ToString("dd / MMM / yyyy hh:mm:ss tt"),
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                });
            }

            this.SetAlertMessageInViewBag();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(PetSearchViewModel model)
        {
            DateTime fromTemp;
            DateTime toTemp;

            try
            {
                if (
                DateTime.TryParse(model.From.Value.ToString(), out fromTemp) &&
                DateTime.TryParse(model.To.Value.ToString(), out toTemp)
                )
                {
                    TempData["FromDate"] = fromTemp;
                    TempData["ToDate"] = toTemp;
                }
                else
                {
                    this.SetAlertMessageInTempData(Models.Shared.AlertMessageTypeEnum.Error, "El rango de fechas no es válido.");
                }
            }
            catch (Exception ex)
            {
                if (model.OptionSelected == 3)
                {
                    TempData["FromDate"] = null;
                    TempData["ToDate"] = null;
                    this.SetAlertMessageInTempData(Models.Shared.AlertMessageTypeEnum.Error, "Seleccione algun rango de fechas.");
                }
            }

            return RedirectToAction("Index", new { op = model.OptionSelected });
        }

        public ActionResult PublicProfile(string id)
        {
            this.VerifySessionVariables();

            var data = _petSearchDataLoader.GetPetAlertDetails(Guid.Parse(id));
            if (data == null)
                return View("AlertNotExist");

            var model = _petSearchMapper.GetPetPublicProfileViewModel(data);
            model.AlertCode = id;

            model.PetInfo.ProfileImageUrl = this.GetPetImageProfile(model.PetInfo.ProfileImageUrl);
            foreach (var owner in model.OwnersInfo)
            {
                owner.ProfileImageUrl = this.GetOwnerImageProfile(owner.ProfileImageUrl);
            }

            return View(model);
        }

        public ActionResult ReportAlert(string code)
        {
            var response = _petSearchDataLoader.ManageReportedPetAlert(code, (int)AlertStatusEnum.Reported);

            this.SetAlertMessageInTempData(Models.Shared.AlertMessageTypeEnum.Success, "La Alerta ha sido Reportada.");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SuccessStories(int? page)
        {
            this.VerifySessionVariables();

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

        private List<RadioButtonModel> GetRabioButtonList()
        {
            return new List<RadioButtonModel>
            {
                new RadioButtonModel { Value = (int)LastAlertOptionEnum.LastWeek , ElementId ="LastAlertsOpt1", DisplayText = "Última Semana", Selected = false },
                new RadioButtonModel { Value = (int)LastAlertOptionEnum.LastMonth , ElementId ="LastAlertsOpt2", DisplayText = "Última Mes", Selected = false },
                new RadioButtonModel { Value = (int)LastAlertOptionEnum.Custom , ElementId ="LastAlertsOpt3", DisplayText = "Buscar por Fechas", Selected = false }
            };
        }

        private void SetSelectedItem(int option, List<RadioButtonModel> list)
        {
            var itemFound = list.SingleOrDefault(e => e.Value == option);
            if (itemFound != null)
                itemFound.Selected = true;
            else
                list[0].Selected = true;
        }

        public ActionResult LastAlerts(int? op, int? page)
        {
            this.VerifySessionVariables();
            op = op ?? 0;
            if (op < 0 || op > 2)
                op = 0;

            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"].ToString());
            page = page ?? 1;

            DateTime from; DateTime to;
            var list = GetRabioButtonList();
            SetSelectedItem(op.Value, list);
            this.SetFromToBaseOnLastAlertsOption(op.Value, out from, out to);

            var model = new PetLastAlertsPagedListViewModel
            {
                Options = list,
                From = from,
                To = to,
                Records = new List<PetLastAlertDetailViewModel>()
            };

            if (from > to)
                this.SetAlertMessageInTempData(Models.Shared.AlertMessageTypeEnum.Error, "Seleccione un rango valido de fechas.");
            else
            {
                var result = _petSearchDataLoader.GetPetLastAlerts(from, to, pageSize, page.Value);
                if (page < 0) page = 1;
                else if (page > result.TotalPages) page = result.TotalPages;

                model.Records = result.Result;
                model.Pagination = this.SetPaginationViewModel($"/PetSearch/LastAlerts?op={op}&page=", result.TotalRecords, result.TotalPages, page.Value, pageSize);
            }

            this.SetAlertMessageInViewBag();
            return View(model);
        }

        [HttpPost]
        public ActionResult LastAlerts(PetLastAlertsPagedListViewModel model)
        {
            DateTime fromTemp;
            DateTime toTemp;
            if (
                DateTime.TryParse(model.From.Value.ToString(), out fromTemp) &&
                DateTime.TryParse(model.To.Value.ToString(), out toTemp)
                )
            {
                TempData["FromDate"] = fromTemp;
                TempData["ToDate"] = toTemp;
            }
            else
            {
                this.SetAlertMessageInTempData(Models.Shared.AlertMessageTypeEnum.Error, "El rango de fechas no es válido.");
            }

            return RedirectToAction("LastAlerts", new { op = model.OptionSelected });
        }

    }
}