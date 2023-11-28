namespace Interface.Train
{
    public interface ITrainContainerDAL
    {
        public List<TrainDTO> GetTrains();
        public TrainDTO FindById(int id);
        public int DeleteTrain(int id);
        public int InsertTrain(TrainDTO trainDTO);
        public int DisableLinkedJourneys(int id);
        public int UpdateLinkedEmployees(int id);
        public int DeleteSeats(int trainId);
    }
}