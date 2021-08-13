using CinemaApp.DomainEntity.Interfaces;
using CinemaApp.DomainEntity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace CinemaApp.Persistance.Service
{
    public class CinemaCustomerService : iCinemaCustomerService
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

        public void AddCart(UserCart cart)
        {
            db.UserCarts.Add(cart);
            Save();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public IEnumerable<UserCart> UpdateCart(string Seat)
        {
            throw new NotImplementedException();
        }

        public UserDetails GetUserDetails(int UserDetailsId)
        {
            return db.UserDetails.Where(d => d.Id == UserDetailsId).SingleOrDefault();
        }

        public Movie GetMovieData(int MovieId)
        {
            return db.Movie.Where(d => d.MovieId == MovieId).SingleOrDefault();
        }

        public Hall GetHallData(int MovieHallId)
        {
            var HallData = (from h in db.Hall
                            join mh in db.MovieHall on h.HallId equals mh.HallId
                            where mh.MovieHallId == MovieHallId
                            select h).SingleOrDefault();
            
            return HallData;
        }

        public MovieHall GetMovieHallData(int MovieHallId)
        {
            var MovieHallData = (from h in db.Hall
                                 join mh in db.MovieHall on h.HallId equals mh.HallId
                                 where mh.MovieHallId == MovieHallId
                                 select mh).SingleOrDefault();

            return MovieHallData;
        }

        public MovieHallDetails replaceEmptyOrTakenSeat(string Seat, int MovieHallId)
        {
            return db.MovieHallDetails.Where(d => d.MovieSeat == Seat && d.MovieHallId == MovieHallId).SingleOrDefault();
        }

        public double TicketTotal(int UserDetailsId)
        {
            double TicketTotal = db.UserCarts.Where(d => d.UserDetailsId == UserDetailsId && d.ConfirmCart == false).Sum(d => (int?)d.TicketPrice) ?? 0;
            return TicketTotal;
        }

        public void UpdateMovieHallDetailsSeat(MovieHallDetails mhd)
        {
            db.Entry(mhd).State = EntityState.Modified;
            Save();
        }

        public IEnumerable<MovieHallDetails> GetMovieHallDetails()
        {
            return db.MovieHallDetails.ToList();
        }

        public MovieHallDetails GetMovieHallDetail(int Id)
        {
            return db.MovieHallDetails.Find(Id);
        }

        public void AddTransaction(Transactions trans)
        {
            db.Transactions.Add(trans);
            Save();
        }

        public void UpdateUserDetails(UserDetails user)
        {
            db.Entry(user).State = EntityState.Modified;
            Save();
        }

        public void DeleteCart(UserCart cart)
        {
            db.UserCarts.Remove(cart);
            Save();
        }

        public UserCart GetCart(int Id)
        {
            return db.UserCarts.Find(Id);
        }

        public IEnumerable<Transactions> GetTransactions()
        {
            return db.Transactions.ToList();
        }

        public void ReplaceUnorderedSeats(int UserDetailsId)
        {
            foreach (var item in db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList())
            {
                item.ConfirmCart = true;
                Save();
            }

            foreach (var item in db.MovieHallDetails.Where(d => d.UserDetailsId == UserDetailsId).ToList())
            {
                item.SeatStatus = Status.O;
                Save();
            }
        }

        public IEnumerable<UserCart> GetUnorderedSeats(int UserDetailsId)
        {
            return db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList();
        }

        public IEnumerable<Transactions> GetUserTransactionList(int UserDetailsId)
        {
            return db.Transactions.Where(d => d.UserDetailsId == UserDetailsId).ToList();
        }

        public void RemoveUnconfirmedOrders(int UserDetailsId)
        {
            var RemoveUnconfirmedOrders = (from mhd in db.MovieHallDetails
                                            join uc in db.UserCarts
                                             on mhd.MovieSeat equals uc.Seat
                                            where mhd.UserDetailsId == UserDetailsId && uc.ConfirmCart == false && uc.UserDetailsId == UserDetailsId
                                            select mhd).ToList();

            db.UserCarts.RemoveRange(db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList());
            Save();

            foreach (var item in RemoveUnconfirmedOrders)
            {
                item.SeatStatus = Status.E;
                item.UserDetailsId = null;
            }

            Save();
        }

        public IEnumerable<UserCart> OrderSummaryConfirmedList(int UserDetailsId)
        {
            return db.UserCarts.Where(d => d.ConfirmCart == true && d.UserDetailsId == UserDetailsId).ToList();
        }

    }
}
