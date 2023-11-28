using ASP.Models;
using Data.Railway;
using Data.Station;
using Logic.Railway;
using Logic.Station;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
    public class RailwayController : Controller
    {
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            RailwayContainer railwayC = new RailwayContainer(new RailwayDAL());
            List<RailwayModel> railwayModelList = railwayC.GetAllRailways();
            if (railwayModelList == null)
                return View(null);
            List<RailwayViewModel> railwayViewModelList = new List<RailwayViewModel>();
            foreach (RailwayModel railwayModel in railwayModelList)
                railwayViewModelList.Add(new RailwayViewModel() { RailwayId = railwayModel.RailwayId, StartStationId = railwayModel.StartStationId, StartStationName = railwayModel.StartStationName, EndStationId = railwayModel.EndStationId, EndStationName = railwayModel.EndStationName, State = railwayModel.State, Length = railwayModel.Length });

            return View(railwayViewModelList);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public IActionResult Create(RailwayViewModel railwayViewModel)
        {
            if (railwayViewModel.StartStationId == 0 || railwayViewModel.EndStationId == 0 || railwayViewModel.Length == 0)
            {
                return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
            }
            try
            {
                RailwayModel railway = new RailwayModel(railwayViewModel.StartStationId, railwayViewModel.EndStationId, true, railwayViewModel.Length);
                RailwayContainer railwayC = new RailwayContainer(new RailwayDAL());
                railwayC.InsertRailway(railway);
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
            RailwayContainer railwayC = new RailwayContainer(new RailwayDAL());
            RailwayModel railwayModel = railwayC.FindRailwayById(id);
            RailwayViewModel railwayViewModel = new RailwayViewModel() { RailwayId = railwayModel.RailwayId, StartStationId = railwayModel.StartStationId, StartStationName = railwayModel.StartStationName, EndStationId = railwayModel.EndStationId, EndStationName = railwayModel.EndStationName, State = railwayModel.State, Length = railwayModel.Length };
            return PartialView("_Edit", railwayViewModel);
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public IActionResult Edit(RailwayViewModel railwayViewModel)
        {
            if (railwayViewModel.RailwayId == 0 || railwayViewModel.StartStationId == 0 || railwayViewModel.EndStationId == 0 || railwayViewModel.Length == 0)
            {
                return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
            }
            try
            {
                RailwayContainer railwayC = new RailwayContainer(new RailwayDAL());
                RailwayModel railwayModel = railwayC.FindRailwayById(railwayViewModel.RailwayId);

                railwayModel.StartStationId = railwayViewModel.StartStationId;
                railwayModel.EndStationId = railwayViewModel.EndStationId;
                railwayModel.State = railwayViewModel.State;
                railwayModel.Length = railwayViewModel.Length;

                railwayModel.UpdateRailway(new RailwayDAL());

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
                RailwayContainer stationC = new RailwayContainer(new RailwayDAL());
                stationC.DeleteRailway(id);
                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }
    }
}
