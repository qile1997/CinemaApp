using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Data
    {
        public ICollection<UserDetails> UserDetail { get; set; }
        public ICollection<Movie> MovieList { get; set; }
        public ICollection<MovieStartTime> MovieTime { get; set; }
        public ICollection<MovieSeats> MovieSeats { get; set; }

        public Data()
        {
            UserDetail = new List<UserDetails>();
            MovieList = new List<Movie>();
            MovieTime = new List<MovieStartTime>();
            MovieSeats = new List<MovieSeats>();
        }

        public void GenerateUserData()
        {
            UserDetail.Add(new UserDetails() { Name = "John", Username = "123", Password = "123" });
            UserDetail.Add(new UserDetails() { Name = "Mike", Username = "321", Password = "321" });
        }
        public void GenerateMovieData()
        {
            MovieList.Add(new Movie() { MovieId = 101, MovieTitle = "Justice League", ReleaseDate = new DateTime(2012, 4, 12), MovieAvailability = true });
            MovieList.Add(new Movie() { MovieId = 102, MovieTitle = "The Matrix", ReleaseDate = new DateTime(2012, 5, 14), MovieAvailability = false });
            MovieList.Add(new Movie() { MovieId = 103, MovieTitle = "The Avengers", ReleaseDate = new DateTime(2012, 5, 14), MovieAvailability = true });
        }
        public void GenerateMovieTime()
        {
            MovieTime.Add(new MovieStartTime() { MovieId = 101, Id = 201, MovieTime = new DateTime(2012, 5, 15, 06, 30, 00) });
            MovieTime.Add(new MovieStartTime() { MovieId = 101, Id = 202, MovieTime = new DateTime(2012, 5, 15, 10, 30, 00) });
            MovieTime.Add(new MovieStartTime() { MovieId = 101, Id = 203, MovieTime = new DateTime(2012, 5, 15, 12, 30, 00) });

            //MovieTime.Add(new MovieStartTime() { MovieId = 102, Id = 204, MovieTime = new DateTime(2012, 5, 15, 06, 30, 00) });
            //MovieTime.Add(new MovieStartTime() { MovieId = 102, Id = 205, MovieTime = new DateTime(2012, 5, 15, 10, 30, 00) });
            //MovieTime.Add(new MovieStartTime() { MovieId = 102, Id = 206, MovieTime = new DateTime(2012, 5, 15, 12, 30, 00) });

            MovieTime.Add(new MovieStartTime() { MovieId = 103, Id = 207, MovieTime = new DateTime(2012, 5, 15, 06, 30, 00) });
            MovieTime.Add(new MovieStartTime() { MovieId = 103, Id = 208, MovieTime = new DateTime(2012, 5, 15, 10, 30, 00) });
            MovieTime.Add(new MovieStartTime() { MovieId = 103, Id = 209, MovieTime = new DateTime(2012, 5, 15, 12, 30, 00) });
        }
        public void GenerateMovieSeats()
        {
            Status seatstatus;

            foreach (var item in MovieTime)
            {
                for (int i = 1; i <= 3; i++)
                {
                    for (int x = 1; x <= 10; x++)
                    {
                        Random rng = new Random();
                        Thread.Sleep(1);
                        int k = rng.Next(0, 10);

                        if (k > 3)
                        {
                            seatstatus = Status.E;
                        }
                        else
                        {
                            seatstatus = Status.T;
                        }

                        Thread.Sleep(1);
                        MovieSeats.Add(new MovieSeats() { MovieId = item.Id, SeatRow = i, SeatColumn = x, Seat = i + "," + x, SeatStatus = seatstatus });
                    }
                }
            }
        }
    }
}
