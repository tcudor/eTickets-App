using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class Order
    {
        [Key]
        
        public int Id { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }


        public List<OrderMovie> OrderMovies { get; set; }
    }
}