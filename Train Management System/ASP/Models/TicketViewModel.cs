namespace ASP.Models
{
    public class TicketViewModel
    {
        public List<JourneySearchViewModel> JourneySearchViewModelList { get; set; }
        public TimeSpan TimeMinutes { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Code { get; set; }
        public int UserId { get; set; }
    }
}
