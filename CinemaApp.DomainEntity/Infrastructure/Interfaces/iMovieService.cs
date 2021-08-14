using CinemaApp.DomainEntity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Infrastructure.Interfaces
{
    public interface iMovieService
    {
        int EmptyUserCartId(string movieSeat, int userDetailsId, int movieHallId);
        MovieHallDetails GetMovieHallSeat(string movieSeat,int movieHallId);
        Transfer GetTransactionMode(int transferMode);
        void SaveChanges();
        void ConvertSeatStatus(MovieHallDetails movieHallSeat);
    }
}
