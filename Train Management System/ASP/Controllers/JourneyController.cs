using ASP.Models;
using Data.Journey;
using Data.Railway;
using Data.Station;
using Data.User;
using Logic.Journey;
using Logic.Railway;
using Logic.Station;
using Logic.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP.Controllers
{
    public class JourneyController : Controller
    {
        /// <summary>
        /// Gets all journeys
        /// </summary>
        /// <returns>List of journeys</returns>
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            JourneyContainer journeyC = new JourneyContainer(new JourneyDAL());
            List<JourneyModel> journeymModelList = journeyC.GetAllJourneys();
            if (journeymModelList == null)
                return View(null);
            List<JourneyViewModel> journeyViewModelList = new List<JourneyViewModel>();
            foreach (JourneyModel journeyModel in journeymModelList)
                journeyViewModelList.Add(new JourneyViewModel() { JourneyId = journeyModel.JourneyId, TrainId = journeyModel.TrainId, RailwayId = journeyModel.RailwayId, StartStationId = journeyModel.StartStationId, AdjustedArrivalTime = journeyModel.AdjustedArrivalTime, AdjustedDepartureTime = journeyModel.AdjustedDepartureTime, ArrivalTime = journeyModel.ArrivalTime, DepartureTime = journeyModel.DepartureTime, State = journeyModel.State });
            return View(journeyViewModelList);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public IActionResult Create(int startStationId, int endStationId, int trainId, TimeSpan departureTime, TimeSpan arrivalTime)
        {
            if (startStationId == 0 || endStationId == 0 || trainId == 0)
            {
                return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
            }

            try
            {
                RailwayContainer railwayC = new RailwayContainer(new RailwayDAL());
                JourneyContainer journeyC = new JourneyContainer(new JourneyDAL());
                StationContainer stationC = new StationContainer(new StationDAL());
                JourneyModel journeyModel;
                    List<RailwayModel> railwayList = journeyC.FindBestRoute(startStationId, endStationId, stationC.GetConnectingRailways());
                if (railwayList.Count == 1)
                {
                    journeyModel = new JourneyModel(railwayList[0].RailwayId, trainId, startStationId, departureTime, arrivalTime, true);
                    journeyC.InsertJourney(journeyModel);
                }
                else if (railwayList.Count > 1)
                { //insert each railway with 5 minutes overstep period in time
                    TimeSpan totalTime = arrivalTime - departureTime;

                    int waitTime = 300; //5 minutes
                    int totalSecondsPerJourney = ((int)totalTime.TotalSeconds - (waitTime * (railwayList.Count - 1))) / railwayList.Count;
                    int depTimeSeconds = (int)departureTime.TotalSeconds;
                    int arrTimeSeconds = depTimeSeconds + totalSecondsPerJourney;

                    for (int i = 0; i < railwayList.Count; i++)
                    {
                        journeyC.InsertJourney(new JourneyModel(railwayList[i].RailwayId, trainId, railwayList[i].StartStationId, new TimeSpan(0, 0, depTimeSeconds), new TimeSpan(0, 0, arrTimeSeconds), true));
                        depTimeSeconds = arrTimeSeconds + waitTime;
                        arrTimeSeconds = depTimeSeconds + totalSecondsPerJourney;
                    }
                }
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
            JourneyContainer journeyC = new JourneyContainer(new JourneyDAL());
            JourneyModel journeyModel = journeyC.FindJourneyById(id);
            JourneyViewModel journeyViewModel = new JourneyViewModel() { JourneyId = journeyModel.JourneyId, TrainId = journeyModel.TrainId, RailwayId = journeyModel.RailwayId, StartStationId = journeyModel.StartStationId, AdjustedArrivalTime = journeyModel.AdjustedArrivalTime, AdjustedDepartureTime = journeyModel.AdjustedDepartureTime, ArrivalTime = journeyModel.ArrivalTime, DepartureTime = journeyModel.DepartureTime, State = journeyModel.State };

            return PartialView("_Edit", journeyViewModel);
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public IActionResult Edit(JourneyViewModel journeyViewModel)
        {
            if (journeyViewModel.JourneyId == 0 || journeyViewModel.RailwayId == 0 || journeyViewModel.TrainId == 0 || journeyViewModel.StartStationId == 0)
            {
                return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
            }

            try
            {
                JourneyContainer journeyC = new JourneyContainer(new JourneyDAL());
                JourneyModel journeyModel = journeyC.FindJourneyById(journeyViewModel.JourneyId);

                journeyModel.State = journeyViewModel.State;
                journeyModel.ArrivalTime = journeyViewModel.ArrivalTime;
                journeyModel.DepartureTime = journeyViewModel.DepartureTime;
                journeyModel.AdjustedArrivalTime = journeyViewModel.AdjustedArrivalTime;
                journeyModel.AdjustedDepartureTime = journeyViewModel.AdjustedDepartureTime;
                journeyModel.RailwayId = journeyViewModel.RailwayId;
                journeyModel.TrainId = journeyViewModel.TrainId;
                journeyModel.StartStationId = journeyViewModel.StartStationId;

                journeyModel.UpdateJourney(new JourneyDAL());
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
                JourneyContainer journeyC = new JourneyContainer(new JourneyDAL());
                journeyC.DeleteJourney(id);
                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } } );
            }
        }


        /// <summary>
        /// allow employee to change settings from journey
        /// </summary>
        /// <returns>a view of either null or a journeyModel depending on whether employee is connected to a journey</returns>
        [Authorize(Roles = "Employee")]
        public IActionResult Settings()
        {
            UserContainer userC = new UserContainer(new UserDAL());
            UserModel currentEmployee = userC.FindUserById(Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));
            int journeyId = Convert.ToInt32(currentEmployee.JourneyId);
            if (journeyId == 0)
            {
                return View(null);
            }
            JourneyContainer journeyC = new JourneyContainer(new JourneyDAL());
            JourneyModel journeyModel = journeyC.FindJourneyById(journeyId);
            JourneyViewModel journeyViewModel = new JourneyViewModel() { JourneyId = journeyModel.JourneyId, TrainId = journeyModel.TrainId, RailwayId = journeyModel.RailwayId, StartStationId = journeyModel.StartStationId, AdjustedArrivalTime = journeyModel.AdjustedArrivalTime, AdjustedDepartureTime = journeyModel.AdjustedDepartureTime, ArrivalTime = journeyModel.ArrivalTime, DepartureTime = journeyModel.DepartureTime, State = journeyModel.State };

            return View(journeyViewModel);
        }

        /// <summary>
        /// updates the journey based on the input given by employee
        /// </summary>
        /// <param name="journeyViewModel"></param>
        /// <returns></returns>
        [HttpPost, Authorize(Roles = "Employee")]
        public IActionResult ChangeSettings(JourneyViewModel journeyViewModel)
        {
            try
            {
                JourneyDAL journeyDAL = new JourneyDAL();
                JourneyContainer journeyC = new JourneyContainer(journeyDAL);
                JourneyModel journeyModel = journeyC.FindJourneyById(journeyViewModel.JourneyId);
                journeyModel.State = journeyViewModel.State;
                journeyModel.AdjustedDepartureTime = journeyViewModel.AdjustedDepartureTime;
                journeyModel.AdjustedArrivalTime = journeyViewModel.AdjustedArrivalTime;
                journeyModel.UpdateJourney(journeyDAL);
                return RedirectToAction(nameof(Settings));
            }
            catch
            {
                return RedirectToAction(nameof(Settings));
            }
        }

        /// <summary>
        /// update the journey that a user is connected to
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        [HttpPost, Authorize(Roles = "Employee")]
        public IActionResult UpdateJourney(int journeyId)
        {
            if (1 == 0)
            {
                return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
            }
            UserContainer userC = new UserContainer(new UserDAL());
            UserModel userModel = userC.FindUserById(Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));
            userModel.JourneyId = journeyId;
            userModel.UpdateUser(new UserDAL());
            return RedirectToAction(nameof(Settings));
        }
    }
}