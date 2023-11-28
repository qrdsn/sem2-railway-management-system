using Interface.Journey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Journey
{
    public class JourneyTicketModel
    {
        public int JourneyTicketId;
        public int JourneyId;
        public int SeatId;
        public int TicketId;
        public decimal JourneyPrice;

        public JourneyTicketModel(JourneyTicketDTO journeyTicketDTO)
        {
            JourneyTicketId = journeyTicketDTO.JourneyTicketId;
            JourneyId = journeyTicketDTO.JourneyId;
            SeatId = journeyTicketDTO.SeatId;
            TicketId = journeyTicketDTO.TicketId;
            JourneyPrice = journeyTicketDTO.JourneyPrice;
        }

        public JourneyTicketModel(int journeyTicketId, int journeyId, int seatId, int ticketId, decimal journeyPrice)
        {
            JourneyTicketId = journeyTicketId;
            JourneyId = journeyId;
            SeatId = seatId;
            TicketId = ticketId;
            JourneyPrice = journeyPrice;
        }
    }
}
