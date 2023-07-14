using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Common;

namespace eLibrary.Domain.Entities
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }
    }
}
