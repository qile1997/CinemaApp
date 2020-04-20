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
        public int UserDetailsId { get; set; }
        public string Seat { get; set; }
        public int MovieId { get; set; }
        public IEnumerable<MovieHallDetails> MovieHallDetails { get; set; }
    }
}