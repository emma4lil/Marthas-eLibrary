using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Common.enums;

namespace eLibrary.Domain.dtos.bookservice
{
    public class Books
    {
        public string Title { get; set; }
        public string CoverUrl { get; set; }
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }

    public class LookUpParam
    {
        public string key { get; set; }
    }

    public class BorrowBookRequest
    {

    }

    public class ReserveBookRequest
    {
        public string Email { get; set; }
        public Guid BookId { get; set; }
        public int TimeInHours { get; set; }
    }

    public class SubscribeBookRequest
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
    }
}
