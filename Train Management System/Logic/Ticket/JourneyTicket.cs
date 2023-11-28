using Interface.Journey;
using Interface.Ticket;

namespace Logic.Ticket
{
    public class JourneyTicket
    {
        public int JourneyTicketId;
        public int JourneyId;
        public int TicketId;
        public int SeatId;
        public decimal JourneyPrice;

        public JourneyTicket(JourneyTicketDTO journeyTicketDTO)
        {
            JourneyTicketId = journeyTicketDTO.JourneyTicketId;
            TicketId = journeyTicketDTO.TicketId;
            TicketId = journeyTicketDTO.TicketId;
            SeatId = journeyTicketDTO.SeatId;
            JourneyPrice = journeyTicketDTO.JourneyPrice;
        }
    }
}