using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Model
{
    public class Movie
    {
        public int Id { get; set; }
        [Display(Name = "Movie Id")]
        public int MovieId { get; set; }
        [Display(Name = "Movie Title")]
        public string MovieTitle { get; set; }
        [Display(Name ="Release Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}")]
        public DateTime ReleaseDate { get; set; }
        [Display(Name = "Movie Availability")]
        public bool MovieAvailability { get; set; }
        public int TicketPrice { get; set; }
    }
}
