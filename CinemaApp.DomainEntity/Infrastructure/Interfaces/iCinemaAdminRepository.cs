using CinemaApp.DomainEntity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Interfaces
{
    public interface iCinemaAdminRepository
    {
        void GenerateUserDetails();
        void GenerateMovies();
        void GenerateHalls();
        void GenerateMovieHalls();
        void GenerateMovieHallDetails();
        void AddUserDetails(UserDetails user);
        void AddMovie(Movie movie);
        void AddHall(Hall hall);
        void AddMovieHall(MovieHall moviehall);
        void AddMovieHallDetails(MovieHallDetails moviehalldetails);
        void ClearAllData();
        void Save();
    }
}
