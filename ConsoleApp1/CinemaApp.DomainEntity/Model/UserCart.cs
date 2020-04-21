using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Model
{
    public class UserCart
    {
        public int Id { get; set; }
        public int TicketPrice { get; set; }
        public int MovieHallsId { get; set; }
        public int UserDetailsId { get; set; }
        public DateTime MovieDateTime { get; set; }
        public string HallNo { get; set; }
        public string Seat { get; set; }
        public string MovieTitle { get; set; }
        public bool ConfirmCart { get; set; }
    }
}
