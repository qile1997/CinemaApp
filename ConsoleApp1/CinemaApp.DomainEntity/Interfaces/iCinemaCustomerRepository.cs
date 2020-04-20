using CinemaApp.DomainEntity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Interfaces
{
    public interface iCinemaCustomerRepository
    {
        //Customer
        IEnumerable<Movie> GetMovies();
        UserDetails LoginCheck(UserDetails user);
        IEnumerable<Movie> GetAvailableMovies();
        IEnumerable<MovieHall> GetSelectedMovies(int MovieId);
        IEnumerable<MovieHallDetails> GetMovieSeats(int MovieHallId);
        IEnumerable<MovieHallDetails> GetAvailableSeats(int MovieHallId);
        //void ReplaceEmptySeats(string Seat, int MovieHallId, int user, int MovieId);
        //void ReplaceEmptySeats(string Seat, int MovieHallId);
        void AddCart(UserCart cart);
        void Save();
        IEnumerable<UserCart> UpdateCart(string Seat);

    }
}
