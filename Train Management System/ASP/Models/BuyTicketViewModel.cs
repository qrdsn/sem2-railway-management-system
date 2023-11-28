namespace ASP.Models
{
    public class BuyTicketViewModel
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public List<JourneyTicketViewModel> JourneyTicketList { get; set; }
        public int SeatType { get; set; }
    }
}
