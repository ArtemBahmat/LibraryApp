using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }             
        public int? Rental_Id { get; set; }

        [ForeignKey("Rental_Id")]
        public virtual BookRental  Rental { get; set; }        
    }
}
