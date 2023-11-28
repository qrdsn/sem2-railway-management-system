using Interface.Journey;
using Interface.Ticket;
using Logic.Journey;

namespace Logic.Ticket
{
    public class TicketModel
    {
        public int TicketId;
        public int UserId;
        public DateTime PurchaseDate;
        public decimal PurchasePrice;
        public List<JourneyTicketModel> JourneyTicketList;
        public int SeatType;
        public string Code;

        public TicketModel(TicketDTO ticketDTO)
        {
            TicketId = ticketDTO.TicketId;
            UserId = ticketDTO.UserId;
            PurchaseDate = ticketDTO.PurchaseDate;
            PurchasePrice = ticketDTO.PurchasePrice;
            Code = ticketDTO.Code;
            if (ticketDTO.JourneyTicketDTOList != null)
            {
                JourneyTicketList = new List<JourneyTicketModel>();
                foreach (JourneyTicketDTO journeyTicketDTO in ticketDTO.JourneyTicketDTOList)
                {
                    JourneyTicketList.Add(new JourneyTicketModel(journeyTicketDTO));
                }
            }
        }

        public TicketModel(int ticketId, int userId, DateTime purchaseDate, decimal purchasePrice, List<JourneyTicketModel> journeyTicketList, int seatType, string code)
        {
            TicketId = ticketId;
            UserId = userId;
            PurchaseDate = purchaseDate;
            PurchasePrice = purchasePrice;
            JourneyTicketList = journeyTicketList.ToList();
            SeatType = seatType;
            Code = code;
        }
    }
}