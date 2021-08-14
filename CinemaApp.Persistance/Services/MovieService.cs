using CinemaApp.DomainEntity.Infrastructure.Interfaces;
using CinemaApp.DomainEntity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Persistance.Services
{
    public class MovieService : iMovieService
    {
        private AppDbContext db = new AppDbContext();

        public void ConvertSeatStatus(MovieHallDetails movieHallSeat)
        {
            movieHallSeat.SeatStatus = Status.E;
            movieHallSeat.UserDetailsId = null;
        }

        public int EmptyUserCartId(string movieSeat, int userDetailsId, int movieHallId)
        {
            return db.UserCarts.Where(d => d.MovieSeat == movieSeat &&
                                        d.UserDetailsId == userDetailsId &&
                                        d.MovieHallsId == movieHallId).SingleOrDefault().
                                        Id;
        }

        public MovieHallDetails GetMovieHallSeat(string movieSeat, int movieHallId)
        {
            return db.MovieHallDetails.Where(d => d.MovieSeat == movieSeat && d.MovieHallId == movieHallId).SingleOrDefault();
        }

        public Transfer GetTransactionMode(int transferMode)
        {
            switch (transferMode)
            {
                case 0:
                    return Transfer.IBGT;
                case 1:
                    return Transfer.IBG;
                default:
                    return 0;
            }
     
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
