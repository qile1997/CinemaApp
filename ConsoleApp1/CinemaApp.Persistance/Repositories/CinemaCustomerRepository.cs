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
    public class CinemaCustomerRepository : iCinemaCustomerRepository
    {
        private AppDbContext db = new AppDbContext();
        public IEnumerable<Movie> GetAvailableMovies()
        {
            return db.Movie.Where(d => d.MovieAvailability == true).ToList();
        }

        public IEnumerable<MovieHallDetails> GetAvailableSeats(int MovieHallId)
        {
            return db.MovieHallDetails.Where(d => d.SeatStatus == Status.E && d.MovieHallId == MovieHallId).ToList();
        }

        public IEnumerable<Movie> GetMovies()
        {
            return db.Movie.ToList();
        }

        public IEnumerable<MovieHallDetails> GetMovieSeats(int MovieHallId)
        {
            return db.MovieHallDetails.Where(d => d.MovieHallId == MovieHallId).ToList();
        }

        public IEnumerable<MovieHall> GetSelectedMovies(int MovieId)
        {
            return db.MovieHall.Where(d => d.MovieId == MovieId).ToList();
        }

        public UserDetails LoginCheck(UserDetails user)
        {
            return db.UserDetails.Where(d => d.Username == user.Username && d.Password == user.Password).SingleOrDefault();
        }

        //public void ReplaceEmptySeats(string Seat, int MovieHallId, int user, int MovieId)
        //{
        //    var replaceEmptySeat = db.MovieHallDetails.Where(d => d.Seat == Seat && d.MovieHallId == MovieHallId).SingleOrDefault();
        //    replaceEmptySeat.SeatStatus = Status.T;
        //    Save();
        //    var filterPrice = (from m in db.Movie
        //                       where m.MovieId == MovieId
        //                       select m.TicketPrice).SingleOrDefault();

        //    UserCart cart = new UserCart();
        //    cart.MovieHallsId = MovieHallId;
        //    cart.MovieId = MovieId;
        //    cart.UserDetailsId = user;
        //    cart.TicketPrice = filterPrice;
        //    cart.Seat = Seat;
        //    AddCart(cart);
        //}

        public void AddCart(UserCart cart)
        {
            db.UserCarts.Add(cart);
            Save();
        }

        //public IEnumerable<UserCart> UpdateCart(string Seat)
        //{
        //    var cart = db.UserCarts.Where(d => d.Seat == Seat).ToList();
        //    return cart;
        //}
        public void Save()
        {
            db.SaveChanges();
        }

        public IEnumerable<UserCart> UpdateCart(string Seat)
        {
            throw new NotImplementedException();
        }
    }
}
