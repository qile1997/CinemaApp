using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Model
{
    public class UserCart
    {
        public int Id { get; set; }
        [Display(Name ="Ticket Price (RM)")]
        public int TicketPrice { get; set; }
        [Display(Name = "Movie Hall Id")]
        public int MovieHallsId { get; set; }
        public int UserDetailsId { get; set; }
        [Display(Name = "Movie Time")]
        public DateTime MovieDateTime { get; set; }
        [Display(Name = "Hall")]
        public string HallNo { get; set; }
        public string MovieSeat { get; set; }
        [Display(Name = "Movie Title")]
        public string MovieTitle { get; set; }
        public bool ConfirmCart { get; set; }
    }
}
