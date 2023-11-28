using Interface.Train;

namespace ASP.Models
{
    public class TrainViewModel
    {
        public int TrainId { get; set; }
        public TrainTypes Type { get; set; }
        public int MaxSpeed { get; set; }
        public int FirstClassSeats { get; set; }
        public int SecondClassSeats { get; set; }
    }
}
