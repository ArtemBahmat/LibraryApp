using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public class BookRental
    {
        public int Id { get; set; }
        public string ClientName { get; set; }        
        [Column(TypeName = "DateTime2")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime EndDate { get; set; }
        public virtual List<Book> Books { get; } = new List<Book>();
    }
}
