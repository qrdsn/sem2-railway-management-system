namespace Interface.Journey
{
    public interface IJourneyContainerDAL
    {
        public List<JourneyDTO> GetJourneys();
        public JourneyDTO FindById(int id);
        public int DeleteJourney(int id);
        public int InsertJourney(JourneyDTO journeyDTO);
        public int DisableLinkedTickets(int journeyId);
    }
}