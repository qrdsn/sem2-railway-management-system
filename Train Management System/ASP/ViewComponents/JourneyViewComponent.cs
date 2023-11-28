using ASP.Models;
using Data.Journey;
using Logic.Journey;
using Microsoft.AspNetCore.Mvc;

namespace ASP.ViewComponents
{
    public class JourneySelectViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            JourneyContainer journeyC = new JourneyContainer(new JourneyDAL());
            List<JourneyModel> journeymModelList = journeyC.GetAllJourneys();
            if (journeymModelList == null)
                return View(null);
            List<JourneyViewModel> journeyViewModelList = new List<JourneyViewModel>();
            foreach (JourneyModel journeyModel in journeymModelList)
                journeyViewModelList.Add(new JourneyViewModel() { JourneyId = journeyModel.JourneyId, TrainId = journeyModel.TrainId, RailwayId = journeyModel.RailwayId, StartStationId = journeyModel.StartStationId, AdjustedArrivalTime = journeyModel.AdjustedArrivalTime, AdjustedDepartureTime = journeyModel.AdjustedDepartureTime, ArrivalTime = journeyModel.ArrivalTime, DepartureTime = journeyModel.DepartureTime, State = journeyModel.State });
            return View("~/Views/Journey/_Select.cshtml", journeyViewModelList);
        }
    }
}
