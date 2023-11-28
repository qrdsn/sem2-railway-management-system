using Data.Station;
using Data.Train;
using Data.Railway;
using Data.Journey;
using Data.User;
using Logic.Railway;
using Logic.Station;
using Logic.Train;
using Logic.Journey;
using Logic.User;
using Interface.User;
using Logic.Ticket;
using Data.Ticket;

/// station ///


// findstation by id //and// update station
/*var stationC = new StationContainer(new StationDAL());
var xyz = stationC.FindStationById(10);

xyz.Location = "Eindhoven";
xyz.Name = "something else";
var upd = xyz.UpdateStation(new StationDAL());*/


// get all stations
/*var stationC = new StationContainer(new StationDAL());
var xyz = stationC.GetAllStations();*/


// insert station
/*var station = new Station("eindhoven", "eindhoven centraal");
var stationC = new StationContainer(new StationDAL());
var y = stationC.InsertStation(station);*/


// Delete station
/*var stationC = new StationContainer(new StationDAL());
var abcd = stationC.DeleteStation(7);*/





/// railway ///



// get railway by id
/*var railwayC = new RailwayContainer(new RailwayDAL());
var singleRailway = railwayC.FindRailwayById(3);*/
// update railway
/*singleRailway.Length = 300;
singleRailway.StartStationId = 2;
var upd = singleRailway.UpdateRailway(new RailwayDAL());*/



// get all railways
/*var railwayC = new RailwayContainer(new RailwayDAL());
var xyza = railwayC.GetAllRailways();*/


// insert railway
/*var railwayC = new RailwayContainer(new RailwayDAL());
var insertedrailway = new Railway(1, false, 900);
var y = railwayC.InsertRailway(insertedrailway);*/





/// train ///


// get train by id
/*var trainC = new TrainContainer(new TrainDAL());
var thePropertyValuesOfASingleTrain = trainC.FindTrainById(2);*/


// get all trains
/*var trainC = new TrainContainer(new TrainDAL());
var abcdefghijklmnopqrstuvwxyz = trainC.GetAllTrains();
*/

// insert train
/*var train = new Train(TrainTypes.ThomasDeTrein, 69420, 0);
var trainC = new TrainContainer(new TrainDAL());
var y = trainC.InsertTrain(train);*/





/// journey ///

// get journey by id
/*var journeyC = new JourneyContainer(new JourneyDAL());
var singleJourney = journeyC.FindJourneyById(2);
// update gotten journey
singleJourney.AdjustedDepartureTime = DateTime.Now;
singleJourney.UpdateJourney(new JourneyDAL());*/


// get all journeys

// insert journey
/*var journey = new Journey(2, 3, new DateTime(2022, 04, 10, 16, 43, 08), new DateTime(2022, 04, 10, 16, 48, 45), true, 0);
var journeyC = new JourneyContainer(new JourneyDAL());
var abc = journeyC.InsertJourney(journey);*/




/// user ///

// test
/*List<string> errors;


*//*var userC = new UserContainer(new UserDAL());
var test = userC.Register("test@something123.com", "aBc123$", "aBc123$", out errors);*/



/*var userC = new UserContainer(new UserDAL());
var logintest = userC.Login("test@something123.com", "aBc123$", out errors);*//*


var userC = new UserContainer(new UserDAL());
var newUser = new UserModel("admin@mail.com", "Quinn", "Richardson", UserRoles.Administrator, null);
var addUserTest = userC.CreateUser(newUser, out errors);
*/





/// ticket ///

var ticketC = new TicketContainer(new TicketDAL());
var tikcet = ticketC.GetTicketsFromUser(1);







Console.ReadKey();
Environment.Exit(0);