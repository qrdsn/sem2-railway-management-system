using ASP.Models;
using Data.Train;
using Logic.Train;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewsComponents
{
    public class TrainSelectViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int selected)
        {
            TrainContainer trainC = new TrainContainer(new TrainDAL());
            List<TrainModel> trainModelList = trainC.GetAllTrains();
            if (trainModelList == null)
                return View(null);
            List<TrainViewModel> trainViewModelList = new List<TrainViewModel>();
            foreach (TrainModel trainModel in trainModelList)
                trainViewModelList.Add(new TrainViewModel() { TrainId = trainModel.TrainId, Type = trainModel.Type, MaxSpeed = trainModel.MaxSpeed, FirstClassSeats = trainModel.FirstClassSeats, SecondClassSeats = trainModel.SecondClassSeats });

            return View("~/Views/Train/_Select.cshtml", new Tuple<List<TrainViewModel>, int>(trainViewModelList, selected));
        }
    }

    public class TrainNameIndexViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int trainId)
        {
            TrainContainer trainC = new TrainContainer(new TrainDAL());
            TrainModel trainModel = trainC.FindTrainById(trainId);
            TrainViewModel trainViewModel = new TrainViewModel() { TrainId = trainModel.TrainId, Type = trainModel.Type, MaxSpeed = trainModel.MaxSpeed, FirstClassSeats = trainModel.FirstClassSeats, SecondClassSeats = trainModel.SecondClassSeats };

            return View("~/Views/Train/_Name.cshtml", trainViewModel);
        }
    }
}