using Interface.Train;

namespace TestData
{
    public class TrainDALTest : ITrainContainerDAL, ITrainDAL
    {
        public List<TrainDTO> trains = new List<TrainDTO>();

        public TrainDALTest()
        {
            trains.Add(new TrainDTO() { });
        }

        public int DeleteSeats(int trainId)
        {
            throw new NotImplementedException();
        }

        public int DeleteTrain(int id)
        {
            throw new NotImplementedException();
        }

        public int DisableLinkedJourneys(int id)
        {
            throw new NotImplementedException();
        }

        public TrainDTO FindById(int id)
        {
            throw new NotImplementedException();
        }

        public List<TrainDTO> GetTrains()
        {
            throw new NotImplementedException();
        }

        public int InsertTrain(TrainDTO trainDTO)
        {
            throw new NotImplementedException();
        }

        public int UpdateLinkedEmployees(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateTrain(TrainDTO trainDTO)
        {
            throw new NotImplementedException();
        }
    }
}
