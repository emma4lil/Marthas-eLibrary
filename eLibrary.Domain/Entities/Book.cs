using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Common;
using eLibrary.Domain.Common.enums;

namespace eLibrary.Domain.Entities
{
    public class Book : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Description { get; set; }

        public string ISBN { get; set; }
        public string CoverUrl { get; set; }
        public string Author { get; set; }
        public BookStatus Status { get; set; }

        public string StatusDescription;
    }
}
