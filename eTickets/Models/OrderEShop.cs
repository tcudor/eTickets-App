using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eTickets.Models
{
    public class OrderEShop
    {
        [Key]

        public int Id { get; set; }

        public string? Email { get; set; }
        public string? Username { get; set; }
        [ForeignKey(nameof(Username))]
        public ApplicationUser User { get; set; }


        public List<OrderItem> OrderItems { get; set; }
    }
}
