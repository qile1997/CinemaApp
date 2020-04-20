using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Model
{
    public class MovieHall
    {
        public int Id { get; set; }
        public int MovieHallId { get; set; }
        public int MovieId { get; set; }
        public int HallId { get; set; }
        public DateTime MovieDateTime { get; set; }
    }
}
