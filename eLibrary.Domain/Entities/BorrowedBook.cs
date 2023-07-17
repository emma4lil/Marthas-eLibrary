using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Common;
using eLibrary.Domain.Common.enums;

namespace eLibrary.Domain.Entities
{
    public class BorrowedBook: BaseEntity
    {
        public BorrowStatus Status { get; set; }
    }
}
