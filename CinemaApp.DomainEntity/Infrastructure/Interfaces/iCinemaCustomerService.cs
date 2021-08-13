using CinemaApp.DomainEntity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Interfaces
{
    public interface iCinemaCustomerService
    {
        //UserDetails
        UserDetails LoginCheck(UserDetails user);
        UserDetails GetUserDetails(int UserDetailsId);
        void UpdateUserDetails(UserDetails user);
        double TicketTotal(int UserDetailsId);
        void RemoveUnconfirmedOrders(int UserDetailsId);
        //Movie
        IEnumerable<Movie> GetMovies();
        IEnumerable<Movie> GetAvailableMovies();
        Movie GetMovieData(int MovieId);
        //Hall
        Hall GetHallData(int MovieHallId);
        //MovieHall
        IEnumerable<MovieHall> GetSelectedMovies(int MovieId);
        MovieHall GetMovieHallData(int MovieHallId);
        //MovieHallDetails
        IEnumerable<MovieHallDetails> GetMovieHallDetails();
        MovieHallDetails GetMovieHallDetail(int Id);
        IEnumerable<MovieHallDetails> GetMovieSeats(int MovieHallId);
        IEnumerable<MovieHallDetails> GetAvailableSeats(int MovieHallId);
        void UpdateMovieHallDetailsSeat(MovieHallDetails mhd);
        MovieHallDetails replaceEmptyOrTakenSeat(string Seat, int MovieHallId);
        //UserCart
        UserCart GetCart(int Id);
        void AddCart(UserCart cart);
        void DeleteCart(UserCart cart);
        void ReplaceUnorderedSeats(int UserDetailsId);
        IEnumerable<UserCart> GetUnorderedSeats(int UserDetailsId);
        IEnumerable<UserCart> OrderSummaryConfirmedList(int UserDetailsId);
        //Transactions
        IEnumerable<Transactions> GetTransactions();
        IEnumerable<Transactions> GetUserTransactionList(int UserDetailsId);
        void AddTransaction(Transactions trans);
        void Save();

    }
}
