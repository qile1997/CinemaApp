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
            SaveChanges();
        }

        public void SaveChanges()
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

        public MovieHallDetails GetMovieHallSeat(string MovieSeat, int MovieHallId)
        {
            return db.MovieHallDetails.Where(d => d.MovieSeat == MovieSeat && d.MovieHallId == MovieHallId).SingleOrDefault();
        }

        public double TicketTotal(int UserDetailsId)
        {
            return db.UserCarts.Where(d => d.UserDetailsId == UserDetailsId && d.ConfirmCart == false).Sum(d => (int?)d.TicketPrice) ?? 0;
        }

        public void UpdateMovieHallDetailsSeat(MovieHallDetails mhd)
        {
            db.Entry(mhd).State = EntityState.Modified;
            SaveChanges();
        }

        public IEnumerable<MovieHallDetails> GetMovieHallDetails()
        {
            return db.MovieHallDetails.ToList();
        }

        public MovieHallDetails GetMovieHallDetailById(int Id)
        {
            return db.MovieHallDetails.Find(Id);
        }

        public void AddTransaction(Transactions trans)
        {
            db.Transactions.Add(trans);
            SaveChanges();
        }

        public void UpdateUserDetails(UserDetails user)
        {
            db.Entry(user).State = EntityState.Modified;
            SaveChanges();
        }

        public void RemoveUserCart(UserCart cart)
        {
            db.UserCarts.Remove(cart);
            SaveChanges();
        }

        public UserCart GetUserCartById(int userCartId)
        {
            return db.UserCarts.Where(d => d.Id == userCartId).SingleOrDefault();
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
                SaveChanges();
            }

            foreach (var item in db.MovieHallDetails.Where(d => d.UserDetailsId == UserDetailsId).ToList())
            {
                item.SeatStatus = Status.O;
                SaveChanges();
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
                                             on mhd.MovieSeat equals uc.MovieSeat
                                            where mhd.UserDetailsId == UserDetailsId && uc.ConfirmCart == false && uc.UserDetailsId == UserDetailsId
                                            select mhd).ToList();

            db.UserCarts.RemoveRange(db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList());
            SaveChanges();

            foreach (var item in RemoveUnconfirmedOrders)
            {
                item.SeatStatus = Status.E;
                item.UserDetailsId = null;
            }

            SaveChanges();
        }

        public IEnumerable<UserCart> OrderSummaryConfirmedList(int UserDetailsId)
        {
            return db.UserCarts.Where(d => d.ConfirmCart == true && d.UserDetailsId == UserDetailsId).ToList();
        }

        public void RemoveAllTransaction(IEnumerable<Transactions> transactionList)
        {
            db.Transactions.Attach(transactionList.First());
            db.Transactions.RemoveRange(transactionList);
        }

        public Transfer GetTransactionMode(int TransferMode)
        {
            switch (TransferMode)
            {
                case 0:
                    return Transfer.IBGT;
                case 1:
                    return Transfer.IBG;
                default:
                    return 0;
            }
        }
    }
}
