using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Common;
using eLibrary.Domain.Common.enums;

namespace eLibrary.Domain.Entities
{
    public class Review: BaseEntity
    {
        public string Test { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public StarRatings Ratings { get; set; } = StarRatings.Zero;
    }
}
