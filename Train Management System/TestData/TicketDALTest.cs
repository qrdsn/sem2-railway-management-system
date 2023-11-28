using Interface.Journey;
using Interface.Ticket;

namespace TestData
{
    public class TicketDALTest : ITicketContainerDAL, ITicketDAL
    {
        public List<TicketDTO> Tickets = new List<TicketDTO>();

        public TicketDALTest()
        {
            Tickets.Add(new TicketDTO() { });
        }

        public int DeleteTicket(int id)
        {
            throw new NotImplementedException();
        }

        public TicketDTO FindTicketById(int id)
        {
            throw new NotImplementedException();
        }

        public List<TicketDTO> GetAllTickets()
        {
            throw new NotImplementedException();
        }

        public int GetEmptySeat(int journeyId, int seatType)
        {
            throw new NotImplementedException();
        }

        public List<JourneyDTO> GetJourneysFromTicket(int ticketId)
        {
            throw new NotImplementedException();
        }

        public List<List<JourneyDTO>> GetJourneysFromTickets(int userId)
        {
            throw new NotImplementedException();
        }

        public List<TicketDTO> GetTicketsFromUser(int userId)
        {
            throw new NotImplementedException();
        }

        public int InsertJourneyTicket(JourneyDTO journeyTicketDTO)
        {
            throw new NotImplementedException();
        }

        public int InsertJourneyTicket(JourneyTicketDTO journeyTicketDTO)
        {
            throw new NotImplementedException();
        }

        public int InsertTicket(TicketDTO ticketDTO)
        {
            throw new NotImplementedException();
        }

        public int ValidateTicket(string code)
        {
            throw new NotImplementedException();
        }

        List<JourneyTicketDTO> ITicketContainerDAL.GetJourneysFromTicket(int ticketId)
        {
            throw new NotImplementedException();
        }
    }
}
