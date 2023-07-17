using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Common;
using eLibrary.Domain.Common.enums;

namespace eLibrary.Domain.Entities
{
    public class Reservation: BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public User User { get; set; }
        public int TimeInHours { get; set; }
        public ReservationStatus State { get; set; }
    }
}
