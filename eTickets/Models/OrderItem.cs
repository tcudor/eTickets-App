using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTickets.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }

        [ForeignKey("MovieId")]
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
