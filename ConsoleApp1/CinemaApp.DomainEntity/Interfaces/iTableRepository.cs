using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Interfaces
{
    public interface iTableRepository
    {
        void PrintUserTable();
        void PrintMoviesTable();
        void PrintHallsTable();
        void PrintMovieHallsTable();
        void PrintMovieHallDetailsTable();
        //void PrintSampleSeats();
    }
}
