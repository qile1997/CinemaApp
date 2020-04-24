using CinemaApp.DomainEntity.Interfaces;
using CinemaApp.DomainEntity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaApp.Persistance.Repository
{
    public class CinemaAdminRepository : iCinemaAdminRepository
    {
        private AppDbContext db = new AppDbContext();
        public void AddHall(Hall hall)
        {
            db.Hall.Add(hall);
            Save();
        }

        public void AddMovie(Movie movie)
        {
            db.Movie.Add(movie);
            Save();
        }

        public void AddMovieHall(MovieHall moviehall)
        {
            db.MovieHall.Add(moviehall);
            Save();
        }

        public void AddMovieHallDetails(MovieHallDetails moviehalldetails)
        {
            db.MovieHallDetails.Add(moviehalldetails);
            Save();
        }

        public void AddUserDetails(UserDetails user)
        {
            db.UserDetails.Add(user);
            Save();
        }

        public void ClearAllData()
        {
            var removeuser = db.UserDetails.ToList();
            db.UserDetails.RemoveRange(removeuser);
            var removemovie = db.Movie.ToList();
            db.Movie.RemoveRange(removemovie);
            var removemoviehalls = db.MovieHall.ToList();
            db.MovieHall.RemoveRange(removemoviehalls);
            var removemoviehalldetails = db.MovieHallDetails.ToList();
            db.MovieHallDetails.RemoveRange(removemoviehalldetails);
            var removehall = db.Hall.ToList();
            db.Hall.RemoveRange(removehall);
            var removetransactions = db.Transactions.ToList();
            db.Transactions.RemoveRange(removetransactions);
            Save();
        }

        public void GenerateHalls()
        {
            Hall hall = new Hall();
            hall.HallId = 11;
            hall.HallNo = "One";
            hall.TotalRow = 4;
            hall.TotalColumn = 10;
            AddHall(hall);
            hall.HallId = 12;
            hall.HallNo = "Two";
            hall.TotalRow = 3;
            hall.TotalColumn = 10;
            AddHall(hall);
            hall.HallId = 13;
            hall.HallNo = "Three";
            hall.TotalRow = 4;
            hall.TotalColumn = 10;
            AddHall(hall);
        }

        public void GenerateMovieHallDetails()
        {
            Status seatstatus;

            var MovieHallData = from h in db.Hall
                                join mh in db.MovieHall on h.HallId equals mh.HallId
                                join m in db.Movie on mh.MovieId equals m.MovieId
                                where m.MovieAvailability == true
                                select new
                                {
                                    MovieHallId = mh.MovieHallId,
                                    TotalRow = h.TotalRow,
                                    TotalColumn = h.TotalColumn,
                                    HallId = h.HallId,
                                };

            foreach (var item in MovieHallData)
            {
                for (int i = 1; i <= item.TotalRow; i++)
                {
                    for (int x = 1; x <= item.TotalColumn; x++)
                    {
                        Random rng = new Random();
                        int k = rng.Next(0, 10);
                        Thread.Sleep(5);
                        if (k > 3)
                        {
                            seatstatus = Status.E;
                        }
                        else
                        {
                            seatstatus = Status.T;
                        }
                        Thread.Sleep(5);
                        MovieHallDetails moviehalldetails = new MovieHallDetails();
                        moviehalldetails.MovieHallId = item.MovieHallId;
                        moviehalldetails.Seat = i + "," + x;
                        moviehalldetails.SeatStatus = seatstatus;
                        moviehalldetails.UserDetailsId = null;
                        db.MovieHallDetails.Add(moviehalldetails);
                    }
                }
            }
            Save();
        }

        public void GenerateMovieHalls()
        {
            MovieHall moviehall = new MovieHall();
            moviehall.MovieHallId = 201;
            moviehall.HallId = 11;
            moviehall.MovieId = 101;
            moviehall.MovieDateTime = new DateTime(2012, 5, 16, 06, 30, 00);
            AddMovieHall(moviehall);
            moviehall.MovieHallId = 202;
            moviehall.HallId = 12;
            moviehall.MovieId = 101;
            moviehall.MovieDateTime = new DateTime(2012, 5, 16, 08, 30, 00);
            AddMovieHall(moviehall);
            moviehall.MovieHallId = 203;
            moviehall.HallId = 13;
            moviehall.MovieId = 101;
            moviehall.MovieDateTime = new DateTime(2012, 5, 16, 12, 30, 00);
            AddMovieHall(moviehall);
            moviehall.MovieHallId = 204;
            moviehall.HallId = 11;
            moviehall.MovieId = 103;
            moviehall.MovieDateTime = new DateTime(2012, 5, 17, 06, 50, 00);
            AddMovieHall(moviehall);
            moviehall.MovieHallId = 205;
            moviehall.HallId = 12;
            moviehall.MovieId = 103;
            moviehall.MovieDateTime = new DateTime(2012, 5, 17, 08, 30, 00);
            AddMovieHall(moviehall);
            moviehall.MovieHallId = 206;
            moviehall.HallId = 13;
            moviehall.MovieId = 103;
            moviehall.MovieDateTime = new DateTime(2012, 5, 17, 12, 30, 00);
            AddMovieHall(moviehall);
            moviehall.MovieHallId = 207;
            moviehall.HallId = 11;
            moviehall.MovieId = 104;
            moviehall.MovieDateTime = new DateTime(2012, 5, 16, 08, 30, 00);
            AddMovieHall(moviehall);
            moviehall.MovieHallId = 208;
            moviehall.HallId = 12;
            moviehall.MovieId = 104;
            moviehall.MovieDateTime = new DateTime(2012, 5, 18, 10, 30, 00);
            AddMovieHall(moviehall);
            moviehall.MovieHallId = 209;
            moviehall.HallId = 13;
            moviehall.MovieId = 104;
            moviehall.MovieDateTime = new DateTime(2012, 5, 17, 12, 30, 00);
            AddMovieHall(moviehall);
        }

        public void GenerateMovies()
        {
            Movie movie = new Movie();
            movie.MovieId = 101;
            movie.MovieTitle = "Justice League";
            movie.ReleaseDate = new DateTime(2012, 5, 12);
            movie.MovieAvailability = true;
            movie.TicketPrice = 12;
            AddMovie(movie);
            movie.MovieId = 102;
            movie.MovieTitle = "The Matrix";
            movie.ReleaseDate = new DateTime(2012, 5, 24);
            movie.MovieAvailability = false;
            movie.TicketPrice = 13;
            AddMovie(movie);
            movie.MovieId = 103;
            movie.MovieTitle = "The Avengers";
            movie.ReleaseDate = new DateTime(2012, 4, 13);
            movie.MovieAvailability = true;
            movie.TicketPrice = 15;
            AddMovie(movie);
            movie.MovieId = 104;
            movie.MovieTitle = "Spiderman";
            movie.ReleaseDate = new DateTime(2012, 4, 14);
            movie.MovieAvailability = true;
            movie.TicketPrice = 15;
            AddMovie(movie);
            movie.MovieId = 105;
            movie.MovieTitle = "Green Lantern";
            movie.ReleaseDate = new DateTime(2012, 5, 16);
            movie.MovieAvailability = false;
            movie.TicketPrice = 14;
            AddMovie(movie);
        }

        public void GenerateUserDetails()
        {
            UserDetails user = new UserDetails();
            user.Name = "John";
            user.Username = "123";
            user.Password = "123";
            user.Balance = 100;
            AddUserDetails(user);
            user.Name = "Mike";
            user.Username = "321";
            user.Password = "321";
            user.Balance = 150;
            AddUserDetails(user);
            user.Name = "Test";
            user.Username = "111";
            user.Password = "111";
            user.Balance = 12;
        }
        public void ClearUserCart()
        {
            foreach (var item in db.MovieHallDetails)
            {
                item.UserDetailsId = null;
                if (item.SeatStatus == Status.O || item.SeatStatus == Status.A)
                {
                    Random rng = new Random();
                    int k = rng.Next(0, 10);
                    Thread.Sleep(5);
                    if (k > 3)
                    {
                        item.SeatStatus = Status.E;
                    }
                    else
                    {
                        item.SeatStatus = Status.T;
                    }
                }
            }
            var cart = db.UserCarts.ToList();
            db.UserCarts.RemoveRange(cart);
            Save();
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}
