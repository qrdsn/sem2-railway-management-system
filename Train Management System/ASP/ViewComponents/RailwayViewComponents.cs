using ASP.Models;
using Data.Railway;
using Data.Station;
using Logic.Railway;
using Logic.Station;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewsComponents
{
    public class RailwaySelectViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int selected)
        {
            RailwayContainer railwayC = new RailwayContainer(new RailwayDAL());
            List<RailwayModel> railwayModelList = railwayC.GetAllRailways();
            if (railwayModelList == null)
                return View(null);
            List<RailwayViewModel> railwayViewModelList = new List<RailwayViewModel>();
            foreach (RailwayModel railwayModel in railwayModelList)
                railwayViewModelList.Add(new RailwayViewModel() { RailwayId = railwayModel.RailwayId, StartStationId = railwayModel.StartStationId, StartStationName = railwayModel.StartStationName, EndStationId = railwayModel.EndStationId, EndStationName = railwayModel.EndStationName, State = railwayModel.State, Length = railwayModel.Length });

            return View("~/Views/Railway/_Select.cshtml", new Tuple<List<RailwayViewModel>, int>(railwayViewModelList, selected));
        }
    }

    public class RailwayNameViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(RailwayViewModel railway)
        {
            StationContainer stationC = new StationContainer(new StationDAL());
            StationModel startStationModel = stationC.FindStationById(railway.StartStationId);
            StationViewModel startStationViewModel = new StationViewModel() { StationId = startStationModel.StationId, Location = startStationModel.Location, Name = startStationModel.Name };
            StationModel endStationModel = stationC.FindStationById(railway.EndStationId);
            StationViewModel endStationViewModel = new StationViewModel() { StationId = endStationModel.StationId, Location = endStationModel.Location, Name = endStationModel.Name };

            return View("~/Views/Railway/_Name.cshtml", new Tuple<StationViewModel, StationViewModel>(startStationViewModel, endStationViewModel));
        }
    }

    public class RailwayNameIndexViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int railwayId, int startStationId)
        {
            RailwayContainer railwayC = new RailwayContainer(new RailwayDAL());
            RailwayModel railway = railwayC.FindRailwayById(railwayId);
            if (railway == null)
                return null;
            StationContainer stationC = new StationContainer(new StationDAL());
            StationModel startStationModel = stationC.FindStationById(railway.StartStationId);
            StationViewModel startStationViewModel = new StationViewModel() { StationId = startStationModel.StationId, Location = startStationModel.Location, Name = startStationModel.Name };
            StationModel endStationModel = stationC.FindStationById(railway.EndStationId);
            StationViewModel endStationViewModel = new StationViewModel() { StationId = endStationModel.StationId, Location = endStationModel.Location, Name = endStationModel.Name };
            if (startStationViewModel.StationId == startStationId)
            {
                return View("~/Views/Railway/_Name.cshtml", new Tuple<StationViewModel, StationViewModel>(startStationViewModel, endStationViewModel));
            } else
            {
                return View("~/Views/Railway/_Name.cshtml", new Tuple<StationViewModel, StationViewModel>(endStationViewModel, startStationViewModel));
            }
        }
    }
}