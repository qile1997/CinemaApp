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
        MovieHallDetails GetMovieHallDetailById(int Id);
        IEnumerable<MovieHallDetails> GetMovieSeats(int MovieHallId);
        IEnumerable<MovieHallDetails> GetAvailableSeats(int MovieHallId);
        void UpdateMovieHallDetailsSeat(MovieHallDetails mhd);
        MovieHallDetails GetMovieHallSeat(string MovieSeat, int MovieHallId);

        //UserCart
        UserCart GetUserCartById(int Id);
        void AddCart(UserCart userCart);
        void RemoveUserCart(UserCart userCart);
        void ReplaceUnorderedSeats(int UserDetailsId);
        IEnumerable<UserCart> GetUnorderedSeats(int UserDetailsId);
        IEnumerable<UserCart> OrderSummaryConfirmedList(int UserDetailsId);

        //Transactions
        IEnumerable<Transactions> GetTransactions();
        IEnumerable<Transactions> GetUserTransactionList(int UserDetailsId);
        Transfer GetTransactionMode(int TransferMode);
        void AddTransaction(Transactions transaction);
        void RemoveAllTransaction(IEnumerable<Transactions> transactions);
        void SaveChanges();

    }
}
