using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eTickets.Models
{
    public class OrderItem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }

        [ForeignKey("ItemId")]
        public int ItemId { get; set; }
        public Item? Item { get; set; }

        public int OrderEShopId { get; set; }
        [ForeignKey("OrderEShopId")]
        public OrderEShop OrderEShop { get; set; }
    }
}
