using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Common;

namespace eLibrary.Domain.Entities
{
    public class BookSubscription: BaseEntity
    {
        public Guid BookId { get; set; }
        public Guid SubscriberId { get; set; }
        public User Subscriber { get; set; }
        public bool HasNotified { get; set; }
    }
}
