using System.ComponentModel.DataAnnotations;
using eTickets.Data.Base;
using System;
namespace eTickets.Models
{
    public class Actor : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        [Required()]
        public string? ProfilePictureUrl { get; set; }

        [Display(Name = "First Name")]
        [Required()]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required()]
        public string? LastName { get; set; }

        [Display(Name = "Biography")]
        [Required()]
        public string? Bio { get; set; }

        public string? FullName { get { return this.FirstName + " " + this.LastName; } }

        // Relationships
        public List<Actor_Movie>? Actors_Movies { get; set; }
    }
}
