using ASP.Models;
using Data.Station;
using Logic.Station;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class StationController : Controller
    {
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            StationContainer stationC = new StationContainer(new StationDAL());
            List<StationModel> stationModelList = stationC.GetAllStations();
            if (stationModelList == null)
            {
                return View(null);
            }
            List<StationViewModel> stationViewModelList = new List<StationViewModel>();
            foreach (StationModel stationModel in stationModelList)
                stationViewModelList.Add(new StationViewModel() { StationId = stationModel.StationId, Location = stationModel.Location, Name = stationModel.Name });

            return View(stationViewModelList);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public IActionResult Create(StationViewModel stationViewModel)
        {
            if (stationViewModel.Location == null || stationViewModel.Name == null)
            {
                return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
            }
            try
            {
                StationModel station = new StationModel(stationViewModel.Location, stationViewModel.Name);
                StationContainer stationC = new StationContainer(new StationDAL());
                bool verify = stationC.InsertStation(station);
                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            StationContainer stationC = new StationContainer(new StationDAL());
            StationModel stationModel = stationC.FindStationById(id);
            StationViewModel stationViewModel = new StationViewModel() { StationId = stationModel.StationId, Location = stationModel.Location, Name = stationModel.Name };
            return PartialView("_Edit", stationViewModel);
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public IActionResult Edit(StationViewModel stationViewModel)
        {
            if (stationViewModel.StationId == 0 || stationViewModel.Location == null || stationViewModel.Name == null)
            {
                return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
            }
            try
            {
                StationContainer stationC = new StationContainer(new StationDAL());
                StationModel stationModel = stationC.FindStationById(stationViewModel.StationId);

                stationModel.Location = stationViewModel.Location;
                stationModel.Name = stationViewModel.Name;

                stationModel.UpdateStation(new StationDAL());

                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            return PartialView("_Delete", id);
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public IActionResult ConfirmDelete(int id)
        {
            if (id == 0)
            {
                return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
            }

            try
            {
                StationContainer stationC = new StationContainer(new StationDAL());
                stationC.DeleteStation(id);
                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }
    }
}
