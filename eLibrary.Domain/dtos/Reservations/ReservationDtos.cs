using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.Domain.dtos.Reservations
{
    public class ReservationDetail
    {
        public string Email { get; set; }
        public string BookTitle { get; set; }
        public string Status { get; set; }
        public Guid Id { get; set; }
        public int TimeInHours { get; set; }
    }
}
