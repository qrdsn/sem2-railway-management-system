using Interface.Train;

namespace Logic.Train
{
    public class TrainContainer
    {
        private ITrainContainerDAL _trainDAL;

        public TrainContainer(ITrainContainerDAL trainDAL)
        {
            _trainDAL = trainDAL;
        }

        public List<TrainModel> GetAllTrains()
        {
            List<TrainDTO> trainDTOList = _trainDAL.GetTrains();
            if (trainDTOList == null)
                return null;

            List<TrainModel> trainList = new List<TrainModel>();
            
            foreach (TrainDTO trainDTO in trainDTOList)
            {
                trainList.Add(new TrainModel(trainDTO));
            }

            return trainList;
        }

        public TrainModel FindTrainById(int id)
        {
            return new TrainModel(_trainDAL.FindById(id));
        }

        public bool InsertTrain(TrainModel train)
        {
            TrainDTO trainDTO = new TrainDTO() { Type = train.Type, MaxSpeed = train.MaxSpeed, FirstClassSeats = train.FirstClassSeats, SecondClassSeats = train.SecondClassSeats };

            if (_trainDAL.InsertTrain(trainDTO) >= 1 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteTrain(int trainId)
        {
            if (_trainDAL.DeleteSeats(trainId) >= 0 && _trainDAL.DeleteTrain(trainId) == 1 && _trainDAL.DisableLinkedJourneys(trainId) >= 0 && _trainDAL.UpdateLinkedEmployees(trainId) <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}