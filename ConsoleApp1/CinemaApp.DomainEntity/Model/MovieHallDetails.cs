using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Model
{
    public class MovieHallDetails
    {
        public int Id { get; set; }
        public int MovieHallId { get; set; }
        public Status SeatStatus { get; set; }
        public string Seat { get; set; }
        public int? UserDetailsId { get; set; }
    }
    public enum Status
    {
        E, T, O,A
    }
}

