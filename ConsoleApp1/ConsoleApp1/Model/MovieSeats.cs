using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class MovieSeats
    {
        public Status SeatStatus { get; set; }
        public int SeatRow { get; set; }
        public int SeatColumn { get; set; }
        //public int TotalSeats => SeatRow * SeatColumn;
        public string Seat { get; set; }
    }
    public enum Status
    {
        E, T
    }
}
