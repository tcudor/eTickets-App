using System.ComponentModel.DataAnnotations;

namespace eTickets.Models
{
    public class OrderEShop
    {
        [Key]

        public int Id { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }


        public List<OrderItem> OrderItems { get; set; }
    }
}
