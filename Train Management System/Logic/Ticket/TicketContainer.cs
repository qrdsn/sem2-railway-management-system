using Interface.Journey;
using Interface.Railway;
using Interface.Ticket;
using Logic.Journey;
using Logic.Railway;

namespace Logic.Ticket
{
    public class TicketContainer
    {
        private ITicketContainerDAL _ticketDAL;

        public TicketContainer(ITicketContainerDAL ticketDAL)
        {
            _ticketDAL = ticketDAL;
        }

        public List<TicketModel> GetTicketsFromUser(int userId)
        {
            List<TicketDTO> ticketDTOList = _ticketDAL.GetTicketsFromUser(userId);

            if (ticketDTOList == null)
            {
                return null;
            }

            List<TicketModel> ticketList = new List<TicketModel>();

            foreach (TicketDTO ticketDTO in ticketDTOList)
            {
                ticketList.Add(new TicketModel(ticketDTO));
            }

            return ticketList;

        }

        public TicketModel FindTicketById(int id)
        {
            TicketDTO ticketDTO = _ticketDAL.FindTicketById(id);
            return new TicketModel(ticketDTO);
        }

        public bool BuyTicket(TicketModel ticket)
        {
            TicketDTO ticketDTO = new TicketDTO();

            ticketDTO.UserId = ticket.UserId;
            ticketDTO.PurchaseDate = DateTime.Now;
            ticketDTO.PurchasePrice = ticket.PurchasePrice;
            ticketDTO.Code = ticket.Code;

            ticketDTO.TicketId = _ticketDAL.InsertTicket(ticketDTO);

            foreach (JourneyTicketModel journeyTicketModel in ticket.JourneyTicketList)
            {
                _ticketDAL.InsertJourneyTicket(new JourneyTicketDTO() { JourneyTicketId = journeyTicketModel.JourneyTicketId, JourneyId = journeyTicketModel.JourneyId, SeatId = _ticketDAL.GetEmptySeat(journeyTicketModel.JourneyId, ticket.SeatType), TicketId = ticketDTO.TicketId, JourneyPrice = journeyTicketModel.JourneyPrice });
            }

            return true;
        }

        public bool DeleteTicket(int ticketId)
        {
            if (_ticketDAL.DeleteTicket(ticketId) == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public List<TicketModel> GetAllTickets()
        {
            List<TicketDTO> ticketDTOList = _ticketDAL.GetAllTickets();
            if (ticketDTOList == null)
                return null;

            List<TicketModel> ticketList = new List<TicketModel>();

            foreach (TicketDTO ticketDTO in ticketDTOList)
            {
                ticketList.Add(new TicketModel(ticketDTO));
            }

            return ticketList;
        }

        public TicketModel GetTicket(List<JourneyModel> journeyModelList, List<RailwayModel> railwayModelList, int userId, int seatType)
        {
            List<JourneyTicketModel> journeyTicketModelList = new List<JourneyTicketModel>();

            decimal totalPrice = 0;

            int i = 0;
            foreach(JourneyModel journeyModel in journeyModelList)
            {
                decimal price = 5 + railwayModelList[i].Length/5 - journeyModelList.Count + (journeyModelList.Count * 3 * (1/seatType));
                totalPrice += price;
                i++;
                journeyTicketModelList.Add(new JourneyTicketModel(0, journeyModel.JourneyId, 0, 0, price));
            }
            
            TicketModel ticketModel = new TicketModel(0, userId, DateTime.Now, totalPrice, journeyTicketModelList, seatType, Functions.GetUniqueKey(30));

            return ticketModel;
        }

        public int Validate(string code)
        {
            int journey_id = _ticketDAL.ValidateTicket(code);
            if (journey_id != 0)
            {
                return journey_id;
            } else
            {
                return 0;
            }
        }
    }
}