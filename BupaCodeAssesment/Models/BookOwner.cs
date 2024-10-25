using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BupaCodeAssesment.Models
{
    public class BookOwner
    {
        public int Age { get; set; }
        public List<Book> Books { get; set; }
    }
}