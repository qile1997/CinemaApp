using CinemaApp.DomainEntity.Interfaces;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Persistance.Repository
{
    public class TableRepository : iTableRepository
    {
        private AppDbContext db = new AppDbContext();
        public void PrintHallsTable()
        {
            Console.WriteLine("Halls");
            var HallTable = new ConsoleTable("Hall Id", "Hall No", "Total Rows", "Total Columns", "Total Seats");

            foreach (var item in db.Hall)
            {
                HallTable.AddRow(item.HallId, item.HallNo, item.TotalRow, item.TotalColumn, item.TotalSeats);
            }

            HallTable.Write();
        }

        public void PrintMovieHallDetailsTable()
        {
            Console.WriteLine("Movie Hall Details");

            var MovieHallData = from h in db.Hall
                                join mh in db.MovieHall on h.HallId equals mh.HallId
                                join m in db.Movie on mh.MovieId equals m.MovieId
                                select new
                                {
                                    MovieHallId = mh.MovieHallId,
                                    MovieTitle = m.MovieTitle,
                                    MovieDateTime = mh.MovieDateTime,
                                    HallNo = h.HallNo,
                                    TotalRow = h.TotalRow,
                                    TotalColumn = h.TotalColumn,
                                    HallId = h.HallId,
                                };
                
            foreach (var _item in MovieHallData)
            {
                Console.WriteLine("Hall No : " + _item.HallNo);
                //Console.WriteLine(_item.MovieHallId);
                Console.WriteLine("Movie Title : " + _item.MovieTitle);
                Console.WriteLine("Movie Start Time : " + _item.MovieDateTime);

                var PrintSeats = db.MovieHallDetails.Where(mhd => mhd.MovieHallId == _item.MovieHallId);

                foreach (var item in PrintSeats)
                {
                    Console.Write(" {0} {1} ", item.Seat, item.SeatStatus);

                    if (item.Seat.Substring(item.Seat.Length - 2) == "10")
                    {
                        Console.WriteLine("");
                    }
                }
                Console.WriteLine("");
            }
        }
        //public void PrintSampleSeats()
        //{
        //    Console.WriteLine("Sample Seats in each hall");

        //    var PrintSampleSeats = new ConsoleTable("");

        //    var SampleSeat = from h in db.Hall
        //                     join mh in db.MovieHall on h.HallId equals mh.HallId
        //                     join mhd in db.MovieHallDetails on mh.MovieHallId equals mhd.MovieHallId
        //                     join m in db.Movie on mh.MovieId equals m.MovieId
        //                     select new
        //                     {
        //                         MovieHallId = mhd.MovieHallId,
        //                         HallNo = h.HallNo,
        //                         MovieTitle = m.MovieTitle,
        //                         MovieDateTime = mh.MovieDateTime,
        //                         HallId = h.HallId,
        //                     };

        //    foreach (var item in SampleSeat)
        //    {
        //        var _SampleSeats = db.MovieHallDetails.Where(mhd => mhd.MovieHallId == item.MovieHallId);

        //        foreach (var _item in _SampleSeats)
        //        {
        //            Console.Write(" {0} {1} ", _item.Seat, _item.SeatStatus);

        //            if (_item.Seat.Substring(_item.Seat.Length - 2) == "10")
        //            {
        //                Console.WriteLine("");
        //            }
        //        }
        //    }
        //}
        public void PrintMovieHallsTable()
        {
            Console.WriteLine("Movie Halls");
            var MovieHallsTable = new ConsoleTable("Movie Hall Id", "Movie Id", "Hall Id", "Movie Start Time");

            foreach (var item in db.MovieHall)
            {
                MovieHallsTable.AddRow(item.MovieHallId, item.MovieId, item.HallId, item.MovieDateTime);
            }

            MovieHallsTable.Write();
        }

        public void PrintMoviesTable()
        {
            Console.WriteLine("Movies");
            var MovieTable = new ConsoleTable("Id", "Movie Title", "Ticket Price", "Release Date", "Movie Available");

            foreach (var item in db.Movie)
            {
                string MovieInput;

                if (item.MovieAvailability)
                {
                    MovieInput = "Now Showing";
                }
                else
                {
                    MovieInput = "Coming Soon";
                }

                MovieTable.AddRow(item.MovieId, item.MovieTitle, item.TicketPrice, item.ReleaseDate, MovieInput);
            }
            MovieTable.Write();
        }

        public void PrintUserTable()
        {
            Console.WriteLine("User Details");
            var UserTable = new ConsoleTable("Id", "Name", "Username", "Password", "Balance");

            foreach (var item in db.UserDetails)
            {
                UserTable.AddRow(item.Id, item.Name, item.Username, item.Password, item.Balance);
            }

            UserTable.Write();
        }
    }
}
