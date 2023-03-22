using eTickets.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace eTickets.Models
{
    public class Item : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public string? PictureUrl { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

    }
}
