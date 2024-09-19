using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBookingApp
{
    internal class ReservationInfo
    {
        public int ReservationId { get; set; }
        public string MovieName { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public decimal TicketPrice { get; set; }
        public int TicketAmount { get; set; }
        public int UserId { get; set; }
    }
}
