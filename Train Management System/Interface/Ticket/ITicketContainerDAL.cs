using Interface.Journey;
using Interface.Railway;

namespace Interface.Ticket
{
    public interface ITicketContainerDAL
    {
        public List<TicketDTO> GetTicketsFromUser(int userId);
        public List<TicketDTO> GetAllTickets();
        public TicketDTO FindTicketById(int id);
        public List<JourneyTicketDTO> GetJourneysFromTicket(int ticketId);
        public int InsertTicket(TicketDTO ticketDTO);
        public int InsertJourneyTicket(JourneyTicketDTO journeyTicketDTO);
        public int GetEmptySeat(int journeyId, int seatType);
        public int DeleteTicket(int id);
        public int ValidateTicket(string code);
    }
}