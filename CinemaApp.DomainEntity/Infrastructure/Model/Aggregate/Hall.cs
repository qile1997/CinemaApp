using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Model
{
    public class Hall
    {
        public int Id { get; set; }
        public int HallId { get; set; }
        public string HallNo { get; set; }
        public int TotalRow { get; set; }
        public int TotalColumn { get; set; }
        public int TotalSeats => TotalRow * TotalColumn;
    }
}
