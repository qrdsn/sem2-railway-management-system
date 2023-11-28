using Interface;
using Interface.Journey;
using Interface.Railway;
using Interface.Ticket;
using System.Data.SqlClient;

namespace Data.Ticket
{
    public class TicketDAL : ITicketContainerDAL, ITicketDAL
    {
        public TicketDTO FindTicketById(int id)
        {
            string q = @"SELECT * FROM [ticket] WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            // how to format the data for usage
            Func<SqlDataReader, TicketDTO> mapping = rdr =>
            {
                return new TicketDTO() { TicketId = (int)rdr["id"], UserId = (int)rdr["user_id"], PurchaseDate = (DateTime)rdr["purchase_date"], PurchasePrice = (decimal)rdr["purchase_price"], JourneyTicketDTOList = GetJourneysFromTicket((int)rdr["id"]), Code = (string)rdr["code"] };
            };

            // executing the retrieval 
            return DatabaseContext.GetSingle(q, parms, mapping);
        }

        /// <summary>
        /// gets all journeys from given ticketid
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public List<JourneyTicketDTO> GetJourneysFromTicket(int ticketId)
        {
            string q = @"SELECT * FROM [journey_ticket] WHERE ticket_id = @tid";

            var parms = new Dictionary<string, object> {
                {"@tid", ticketId}
            };

            Func<SqlDataReader, List<JourneyTicketDTO>> mapping = rdr =>
            {
                List<JourneyTicketDTO> output = new List<JourneyTicketDTO>();

                while (rdr.Read())
                {
                    output.Add(new JourneyTicketDTO() { JourneyTicketId= (int)rdr["id"], JourneyId = (int)rdr["journey_id"], SeatId = (int)rdr["seat_id"], JourneyPrice = (decimal)rdr["price"] });
                }

                return output;
            };

            // executing the retrieval 
            return DatabaseContext.GetList(q, parms, mapping);
        }

        /// <summary>
        /// gets tickets from a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TicketDTO> GetTicketsFromUser(int userId)
        {
            // what data to get
            string q = @"SELECT * FROM [ticket] WHERE user_id = @uid";

            var parms = new Dictionary<string, object> {
                {"@uid", userId}
            };

            // how to format the data for usage
            Func<SqlDataReader, List<TicketDTO>> mapping = rdr =>
            {
                List<TicketDTO> output = new List<TicketDTO>();

                while (rdr.Read())
                {
                    output.Add(new TicketDTO() { TicketId = (int)rdr["id"], JourneyTicketDTOList = GetJourneysFromTicket((int)rdr["id"]), PurchaseDate = (DateTime)rdr["purchase_date"], PurchasePrice = (decimal)rdr["purchase_price"], UserId = (int)rdr["user_id"], Code = (string)rdr["code"] });
                }

                return output;
            };

            // executing the retrieval 
            return DatabaseContext.GetList(q, parms, mapping);
        }

        public List<TicketDTO> GetAllTickets()
        {
            string q = @"SELECT * FROM [ticket]";

            var parms = new Dictionary<string, object> {
            };

            // how to format the data for usage
            Func<SqlDataReader, List<TicketDTO>> mapping = rdr =>
            {
                List<TicketDTO> output = new List<TicketDTO>();

                while (rdr.Read())
                {
                    output.Add(new TicketDTO() { TicketId = (int)rdr["id"], JourneyTicketDTOList = GetJourneysFromTicket((int)rdr["id"]), PurchaseDate = (DateTime)rdr["purchase_date"], PurchasePrice = (decimal)rdr["purchase_price"], UserId = (int)rdr["user_id"], Code = (string)rdr["code"] });
                }

                return output;
            };

            // executing the retrieval 
            return DatabaseContext.GetList(q, parms, mapping);
        }

        public int InsertTicket(TicketDTO ticketDTO)
        {
            string q = @"INSERT INTO [ticket] (user_id, purchase_date, purchase_price, code) output INSERTED.ID VALUES (@uid, @pdate, @pprice, @code);";

            var parms = new Dictionary<string, object>
            {
                {"@uid", ticketDTO.UserId},
                {"@pdate", ticketDTO.PurchaseDate},
                {"@pprice", ticketDTO.PurchasePrice},
                {"@code", ticketDTO.Code}
            };

            return Convert.ToInt32(DatabaseContext.ExecuteScalar(q, parms));
        }

        /// <summary>
        /// inserts in link table the journey for specific ticket
        /// </summary>
        /// <param name="journeyTicketDTO"></param>
        /// <returns></returns>
        public int InsertJourneyTicket(JourneyTicketDTO journeyTicketDTO)
        {
            string q = @"INSERT INTO [journey_ticket] (journey_id, seat_id, ticket_id, price) VALUES (@jid, @sid, @tid, @price);";

            var parms = new Dictionary<string, object>
            {
                {"@jid", journeyTicketDTO.JourneyId},
                {"@sid", journeyTicketDTO.SeatId},
                {"@tid", journeyTicketDTO.TicketId},
                {"@price", journeyTicketDTO.JourneyPrice}
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        /// <summary>
        /// gets an seat that isn't yet claimed by another user, with specific seat type
        /// </summary>
        /// <param name="journeyId"></param>
        /// <param name="seatType"></param>
        /// <returns></returns>
        public int GetEmptySeat(int journeyId, int seatType)
        {
            string q = @"BEGIN TRANSACTION
                        DECLARE @tid INT;
                        SELECT @tid = train_id FROM [journey] where id = @jid;
                        SELECT id
                        FROM [seat]
                        WHERE id NOT IN
                            (SELECT seat_id 
                             FROM [journey_ticket] WHERE journey_id = @jid) AND train_id = @tid AND type = @seatType
                        COMMIT;";

            var parms = new Dictionary<string, object>
            {
                {"@jid", journeyId},
                {"@seatType", seatType},
            };

            return Convert.ToInt32(DatabaseContext.ExecuteScalar(q, parms));
        }

        public int DeleteTicket(int id)
        {
            string q = @"BEGIN TRANSACTION
                        DELETE [station] WHERE id = @id;
                        DELETE [journey_ticket] WHERE ticket_id = @id;
                        COMMIT;";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int ValidateTicket(string code)
        {
            string q = @"BEGIN TRANSACTION 
                        DECLARE @table TABLE(journey_id int, departure_time Time(0), arrival_time Time(0), adjusted_arrival_time Time(0), adjusted_departure_time Time(0));

                        INSERT INTO @table SELECT jt.journey_id, j.departure_time, j.arrival_time, j.adjusted_arrival_time, j.adjusted_departure_time FROM journey_ticket as jt LEFT JOIN journey AS j ON j.id = jt.journey_id WHERE ticket_id = (
                        SELECT id FROM [ticket] WHERE code = @code);

                        SELECT journey_id FROM @table WHERE DATEPART(minute, departure_time) < DATEPART(minute, GETDATE()) AND DATEPART(minute, arrival_time) > DATEPART(minute, GETDATE());
                        COMMIT;";

            var parms = new Dictionary<string, object>
            {
                {"@code", code},
            };

            return Convert.ToInt32(DatabaseContext.ExecuteScalar(q, parms));
        }
    }
}
