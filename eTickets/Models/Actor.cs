using System.ComponentModel.DataAnnotations;

namespace eTickets.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage ="Is required")]
        public string ProfilePictureUrl { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Is required")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Is required")]
        public string LastName { get; set; }
        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Is required")]
        public string Bio { get; set; }

        //Relationships
        public List<Actor_Movie> Actors_Movies { get; set; }
    }
}
