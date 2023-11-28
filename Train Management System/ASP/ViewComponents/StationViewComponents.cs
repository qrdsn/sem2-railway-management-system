using ASP.Models;
using Data.Station;
using Logic.Station;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewsComponents
{
    public class StationSelectViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int selected)
        {
            StationContainer stationC = new StationContainer(new StationDAL());
            List<StationModel> stationModelList = stationC.GetAllStations();
            if (stationModelList == null)
            {
                return View("~/Views/Station/_StationSelect");
            }
            List<StationViewModel> stationViewModelList = new List<StationViewModel>();
            foreach (StationModel stationModel in stationModelList)
                stationViewModelList.Add(new StationViewModel() { StationId = stationModel.StationId, Location = stationModel.Location, Name = stationModel.Name });


            return View("~/Views/Station/_Select.cshtml", new Tuple<List<StationViewModel>, int>(stationViewModelList, selected));
        }
    }
}