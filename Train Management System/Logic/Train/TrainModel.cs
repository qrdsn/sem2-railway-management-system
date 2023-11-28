using Interface.Train;

namespace Logic.Train
{
    public class TrainModel
    {
        public int TrainId;
        public TrainTypes Type;
        public int MaxSpeed;
        public int FirstClassSeats;
        public int SecondClassSeats;

        public TrainModel(TrainDTO trainDTO)
        {
            TrainId = trainDTO.TrainId;
            Type = trainDTO.Type;
            MaxSpeed = trainDTO.MaxSpeed;
            FirstClassSeats = trainDTO.FirstClassSeats;
            SecondClassSeats = trainDTO.SecondClassSeats;
        }

        public TrainModel(TrainTypes type, int maxSpeed, int firstClassSeats, int secondClassSeats)
        {
            Type = type;
            MaxSpeed = maxSpeed;
            FirstClassSeats = firstClassSeats;
            SecondClassSeats = secondClassSeats;
        }

        public bool UpdateTrain(ITrainDAL trainDAL)
        {
            TrainDTO trainDTO = new TrainDTO() { TrainId = TrainId, Type = Type, MaxSpeed = MaxSpeed };

            if (trainDAL.UpdateTrain(trainDTO) == 1)
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