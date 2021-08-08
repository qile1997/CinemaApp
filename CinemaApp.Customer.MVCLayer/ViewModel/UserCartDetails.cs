using CinemaApp.DomainEntity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaApp.Customer.MVCLayer.ViewModel
{
    public class UserCartDetails
    {
        public int Id { get; set; }
        public int TicketPrice { get; set; }
        public int MovieHallsId { get; set; }
        public string MovieTitle { get; set; }
        public string Seat { get; set; }
        public string HallNo { get; set; }
        public IEnumerable<UserCart> _UserCart { get; set; }
    }
}