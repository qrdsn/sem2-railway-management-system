using ASP.Models;
using Data.Train;
using Logic.Train;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
    public class TrainController : Controller
    {
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            TrainContainer trainC = new TrainContainer(new TrainDAL());
            List<TrainModel> trainModelList = trainC.GetAllTrains();
            if (trainModelList == null)
                return View(null);
            List<TrainViewModel> trainViewModelList = new List<TrainViewModel>();
            foreach (TrainModel trainModel in trainModelList)
                trainViewModelList.Add(new TrainViewModel() { TrainId = trainModel.TrainId, Type = trainModel.Type, MaxSpeed = trainModel.MaxSpeed, FirstClassSeats = trainModel.FirstClassSeats, SecondClassSeats = trainModel.SecondClassSeats });

            return View(trainViewModelList);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public IActionResult Create(TrainViewModel trainViewModel)
        {
            if (trainViewModel.MaxSpeed == 0 || trainViewModel.FirstClassSeats + trainViewModel.SecondClassSeats == 0)
            {
                return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
            }
            try
            {
                TrainModel trainModel = new TrainModel(trainViewModel.Type, trainViewModel.MaxSpeed, trainViewModel.FirstClassSeats, trainViewModel.SecondClassSeats);
                TrainContainer trainC = new TrainContainer(new TrainDAL());
                trainC.InsertTrain(trainModel);
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
            TrainContainer trainC = new TrainContainer(new TrainDAL());
            TrainModel trainModel = trainC.FindTrainById(id);
            TrainViewModel trainViewModel = new TrainViewModel() { TrainId = trainModel.TrainId, Type = trainModel.Type, MaxSpeed = trainModel.MaxSpeed };
            return PartialView("_Edit", trainViewModel);
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public IActionResult Edit(TrainViewModel trainViewModel)
        {
            if (trainViewModel.TrainId == 0 || trainViewModel.MaxSpeed == 0 || trainViewModel.FirstClassSeats + trainViewModel.SecondClassSeats == 0)
            {
                return Json(new { errors = new List<string>() { { "Values must be correctly filled" } } });
            }
            try
            {
                TrainContainer trainC = new TrainContainer(new TrainDAL());
                TrainModel trainModel = trainC.FindTrainById(trainViewModel.TrainId);

                trainModel.Type = trainViewModel.Type;
                trainModel.MaxSpeed = trainViewModel.MaxSpeed;

                trainModel.UpdateTrain(new TrainDAL());
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
                TrainContainer trainC = new TrainContainer(new TrainDAL());
                trainC.DeleteTrain(id);
                return Json(Ok());
            }
            catch
            {
                return Json(new { errors = new List<string>() { { "Something went wrong" } } });
            }
        }
    }
}
