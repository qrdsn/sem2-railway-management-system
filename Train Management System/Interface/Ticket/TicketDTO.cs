using Interface.Journey;

namespace Interface.Ticket
{
    public class TicketDTO
    {
        public int TicketId;
        public int UserId;
        public DateTime PurchaseDate;
        public decimal PurchasePrice;
        public List<JourneyTicketDTO> JourneyTicketDTOList;
        public string Code;
    }
}
