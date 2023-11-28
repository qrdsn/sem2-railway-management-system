using ASP.Models;
using Data.Journey;
using Data.Railway;
using Data.Station;
using Data.Ticket;
using Data.User;
using Logic;
using Logic.Journey;
using Logic.Railway;
using Logic.Station;
using Logic.Ticket;
using Logic.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class TicketController : Controller
    {
        public IActionResult Buy()
        {
            return View();
        }

        /// <summary>
        /// searches for the fastest possible route with fastest consecutive journeys after indicated departure time, and returns a potential ticket
        /// </summary>
        /// <param name="startStationId"></param>
        /// <param name="endStationId"></param>
        /// <param name="departureTime"></param>
        /// <param name="seatType"></param>
        /// <returns>searchViewModel</returns>
        [HttpPost]
        public IActionResult Search(int startStationId, int endStationId, DateTime departureTime, int seatType)
        {
            List<string> errors = new List<string>();

            if (startStationId == 0 || endStationId == 0 || departureTime == DateTime.MinValue || seatType == 0)
            {
                errors.Add("All values must be filled");
                return Json(new
                {
                    errors
                });
            }
            else if (startStationId == endStationId)
            {
                errors.Add("Start and ending station can't be the same");
                return Json(new
                {
                    errors
                });
            }
            else
            {
                try
                {
                    TicketContainer ticketC = new TicketContainer(new TicketDAL());
                    JourneyContainer journeyC = new JourneyContainer(new JourneyDAL());
                    RailwayContainer railwayC = new RailwayContainer(new RailwayDAL());
                    StationContainer stationC = new StationContainer(new StationDAL());
                    Dictionary<int, List<RailwayModel>> connectedStations = stationC.GetConnectingRailways();

                    List<RailwayModel> railwayModelList = journeyC.FindBestRoute(startStationId, endStationId, connectedStations);
                    int length = 0;
                    foreach (RailwayModel railwayModel in railwayModelList)
                    {
                        length += railwayModel.Length;
                    }
                    List<JourneyModel> journeyModelList = journeyC.FindBestJourney(railwayModelList, departureTime, startStationId);
                    if (journeyModelList == null)
                    {
                        errors.Add("No possible ticket found");
                        return Json(new
                        {
                            errors
                        });
                    }

                    TicketModel ticketModel = ticketC.GetTicket(journeyModelList, railwayModelList, Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value), seatType);

                    TicketViewModel searchViewModel = new TicketViewModel() { JourneySearchViewModelList = new List<JourneySearchViewModel>() };
                    if ((journeyModelList[journeyModelList.Count - 1].ArrivalTime - journeyModelList[0].DepartureTime).TotalMinutes < 0)
                        searchViewModel.TimeMinutes = journeyModelList[journeyModelList.Count - 1].ArrivalTime - journeyModelList[0].DepartureTime +  new TimeSpan(24, 0, 0);
                    else
                        searchViewModel.TimeMinutes = journeyModelList[journeyModelList.Count - 1].ArrivalTime - journeyModelList[0].DepartureTime;
                    searchViewModel.PurchasePrice = ticketModel.PurchasePrice;
                    for (int i = 0; i < journeyModelList.Count; i++)
                    {
                        string startStationName;
                        string endStationName;
                        RailwayModel railwayModel = railwayC.FindRailwayById(journeyModelList[i].RailwayId);
                        if (railwayModel.StartStationId == journeyModelList[i].StartStationId)
                        {
                            startStationName = railwayModel.StartStationName;
                            endStationName = railwayModel.EndStationName;
                        }
                        else
                        {
                            startStationName = railwayModel.EndStationName;
                            endStationName = railwayModel.StartStationName;
                        }
                        searchViewModel.JourneySearchViewModelList.Add(new JourneySearchViewModel() { StartStationName = startStationName, EndStationName = endStationName, JourneyPrice = ticketModel.JourneyTicketList[i].JourneyPrice, StartTime = journeyModelList[i].DepartureTime, EndTime = journeyModelList[i].ArrivalTime });
                    }

                    BuyTicketViewModel ticketViewModel = new BuyTicketViewModel() { PurchaseDate = ticketModel.PurchaseDate, PurchasePrice = ticketModel.PurchasePrice, TicketId = ticketModel.TicketId, UserId = ticketModel.UserId, JourneyTicketList = new List<JourneyTicketViewModel>(), SeatType = ticketModel.SeatType };
                    foreach(JourneyTicketModel journeyTicketModel in ticketModel.JourneyTicketList)
                    {
                        ticketViewModel.JourneyTicketList.Add(new JourneyTicketViewModel() { JourneyId = journeyTicketModel.JourneyId, JourneyPrice = journeyTicketModel.JourneyPrice, JourneyTicketId = journeyTicketModel.JourneyTicketId, SeatId = journeyTicketModel.SeatId, TicketId = journeyTicketModel.SeatId });
                    }

                    //return JsonConvert.SerializeObject(ticketViewModel);
                    return Json(new
                    {
                        ticketViewModel = ticketViewModel, searchViewModel = searchViewModel
                    });

                }
                catch
                {
                    return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
                }
            }
        }

        /// <summary>
        /// actually insert the ticket into database when confirmed
        /// </summary>
        /// <param name="ticketViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Buy(BuyTicketViewModel ticketViewModel)
        {
            try
            {
                //Convert to normal ticketmodel
                TicketModel ticketModel = new TicketModel(ticketViewModel.TicketId, ticketViewModel.UserId, DateTime.Now, ticketViewModel.PurchasePrice, new List<JourneyTicketModel>(), ticketViewModel.SeatType, Functions.GetUniqueKey(10));
                foreach(JourneyTicketViewModel journeyTicketViewModel in ticketViewModel.JourneyTicketList)
                {
                    ticketModel.JourneyTicketList.Add(new JourneyTicketModel(journeyTicketViewModel.JourneyTicketId, journeyTicketViewModel.JourneyId, journeyTicketViewModel.SeatId, journeyTicketViewModel.TicketId, journeyTicketViewModel.JourneyPrice));
                }

                TicketContainer ticketC = new TicketContainer(new TicketDAL());
                ticketC.BuyTicket(ticketModel);

                //what the fuck
                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }

        [HttpPost]
        public IActionResult SearchResult(TicketViewModel ticket)
        {
            return PartialView("_SearchResult", ticket);
        }

        /// <summary>
        /// returns a list of tickets either from specific user, or if user is administrator from all users
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Passenger")]
        public IActionResult Index()
        {
            int uid = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            TicketContainer ticketC = new TicketContainer(new TicketDAL());
            JourneyContainer journeyC = new JourneyContainer(new JourneyDAL());
            RailwayContainer railwayC = new RailwayContainer(new RailwayDAL());
            List<TicketViewModel> ticketViewModelList = new List<TicketViewModel>();
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == "Administrator") //if administrator
            {
                List<TicketModel> ticketModelList = ticketC.GetAllTickets();
                if (ticketModelList == null)
                    return View(null);
                foreach (TicketModel ticketModel in ticketModelList)
                {
                    ticketViewModelList.Add(new TicketViewModel() { PurchasePrice = ticketModel.PurchasePrice, JourneySearchViewModelList = new List<JourneySearchViewModel>(), Code = ticketModel.Code, UserId = ticketModel.UserId }); //items
                    if (ticketModel.JourneyTicketList != null)
                    {
                        for (int i = 0; i < ticketModel.JourneyTicketList.Count; i++)
                        {
                            JourneyModel journeyModel = journeyC.FindJourneyById(ticketModel.JourneyTicketList[i].JourneyId);
                            string startStationName;
                            string endStationName;
                            RailwayModel railwayModel = railwayC.FindRailwayById(journeyModel.RailwayId);
                            if (railwayModel.StartStationId == journeyModel.StartStationId)
                            {
                                startStationName = railwayModel.StartStationName;
                                endStationName = railwayModel.EndStationName;
                            }
                            else
                            {
                                startStationName = railwayModel.EndStationName;
                                endStationName = railwayModel.StartStationName;
                            }



                            //latest ticketViewModel in ticketViewModelList
                            ticketViewModelList[ticketViewModelList.Count - 1].JourneySearchViewModelList.Add(new JourneySearchViewModel() { StartStationName = startStationName, EndStationName = endStationName, StartTime = journeyModel.DepartureTime, EndTime = journeyModel.ArrivalTime, JourneyPrice = ticketModel.JourneyTicketList[i].JourneyPrice });
                        }
                    }
                }
            } else
            { //if specific user
                List<TicketModel> ticketModelList = ticketC.GetTicketsFromUser(uid);
                if (ticketModelList == null)
                    return View(null);
                foreach (TicketModel ticketModel in ticketModelList)
                {
                    ticketViewModelList.Add(new TicketViewModel() { PurchasePrice = ticketModel.PurchasePrice, JourneySearchViewModelList = new List<JourneySearchViewModel>(), Code = ticketModel.Code }); //items
                    if (ticketModel.JourneyTicketList != null)
                    {
                        for (int i = 0; i < ticketModel.JourneyTicketList.Count; i++)
                        {
                            JourneyModel journeyModel = journeyC.FindJourneyById(ticketModel.JourneyTicketList[i].JourneyId);
                            string startStationName;
                            string endStationName;
                            RailwayModel railwayModel = railwayC.FindRailwayById(journeyModel.RailwayId);
                            if (railwayModel.StartStationId == journeyModel.StartStationId)
                            {
                                startStationName = railwayModel.StartStationName;
                                endStationName = railwayModel.EndStationName;
                            }
                            else
                            {
                                startStationName = railwayModel.EndStationName;
                                endStationName = railwayModel.StartStationName;
                            }

                            //latest ticketViewModel in ticketViewModelList
                            ticketViewModelList[ticketViewModelList.Count - 1].JourneySearchViewModelList.Add(new JourneySearchViewModel() { StartStationName = startStationName, EndStationName = endStationName, StartTime = journeyModel.DepartureTime, EndTime = journeyModel.ArrivalTime, JourneyPrice = ticketModel.JourneyTicketList[i].JourneyPrice });
                        }
                    }
                }
            }
            return View(ticketViewModelList);
        }

        public IActionResult Edit(int id)
        {
            TicketContainer ticketC = new TicketContainer(new TicketDAL());
            TicketModel ticketModel = ticketC.FindTicketById(id);
            TicketViewModel ticketViewModel = new TicketViewModel() { }; // stuff
            return View(ticketViewModel);
        }

        [HttpPost]
        public IActionResult Edit(TicketModel ticketModel1)
        {
            try
            {
                TicketContainer ticketC = new TicketContainer(new TicketDAL());
                TicketModel ticketModel = ticketC.FindTicketById(ticketModel1.TicketId);

                ticketModel.TicketId = ticketModel1.TicketId;
                ticketModel.UserId = ticketModel1.UserId;
                //the rest

                //ticketModel.UpdateTicket(new TicketDAL());
                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(TicketViewModel ticket)
        {
            return View();
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            try
            {
                TicketContainer ticketC = new TicketContainer(new TicketDAL());
                ticketC.DeleteTicket(id);
                return Json(Ok());
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Validate()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = "Employee")]
        public IActionResult Validate(string code)
        {
            if (code == null)
            {
                return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
            }
            try
            {
                TicketContainer ticketC = new TicketContainer(new TicketDAL());
                UserContainer userC = new UserContainer(new UserDAL());
                UserModel currentEmployee = userC.FindUserById(Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));
                int journeyId = Convert.ToInt32(currentEmployee.JourneyId);
                if (journeyId == 0)
                    return View(null);
                int currentJourney = ticketC.Validate(code);

                if (currentJourney == journeyId)
                {
                    return Json(new
                    {
                        result = true
                    });
                } else
                {
                    return Json(new
                    {
                        result = false
                    });
                }
                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }

        [HttpPost]
        public IActionResult ValidateResult(bool result)
        {
            return PartialView("_ValidateResult", result);
        }
    }
}