using System.ComponentModel.DataAnnotations;

namespace eTickets.Models
{
    public class Producer
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Profile Picture")]
        public string ProfilePictureUrl { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Biography")]
        public string Bio { get; set; }

        //Relationships
        public List<Movie> Movies { get; set; }
    }
}
