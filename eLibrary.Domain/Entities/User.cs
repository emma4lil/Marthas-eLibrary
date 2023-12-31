﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Common;

namespace eLibrary.Domain.Entities
{
    public class User: BaseEntity
    {
        public string Email { get; set; }
        public string Lastname { get; set; }
        public string FirstName { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
