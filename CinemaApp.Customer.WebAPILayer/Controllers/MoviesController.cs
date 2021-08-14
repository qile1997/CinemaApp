using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance.Service;
using System.Collections.Generic;
using System.Web.Http;

namespace CinemaApp.Customer.WebAPILayer.Controllers
{
    public class MoviesController : ApiController
    {
        private CinemaCustomerService _cinemaCustomerService = new CinemaCustomerService();

        // GET: api/Movies
        public IEnumerable<Movie> Get()
        {
            return _cinemaCustomerService.GetMovies();
        }

        [Route("api/Movies/GetUserDetails/{UserDetailsId}")]
        [HttpGet]
        public UserDetails GetUserDetails(int UserDetailsId)
        {
            return _cinemaCustomerService.GetUserDetails(UserDetailsId);
        }

        [Route("api/Movies/GetAvailableMovies")]
        [HttpGet]
        public IEnumerable<Movie> GetAvailableMovies()
        {
            return _cinemaCustomerService.GetAvailableMovies();
        }

        [Route("api/Movies/GetSelectedMovies/{MovieId:int}")]
        [HttpGet]
        public IEnumerable<MovieHall> GetSelectedMovies(int MovieId)
        {
            return _cinemaCustomerService.GetSelectedMovies(MovieId);
        }

        [Route("api/Movies/GetMovieSeats/{MovieHallId:int}")]
        [HttpGet]
        public IEnumerable<MovieHallDetails> GetMovieSeats(int MovieHallId)
        {
            return _cinemaCustomerService.GetMovieSeats(MovieHallId);
        }

        [Route("api/Movies/GetHallData/{MovieHallId:int}")]
        [HttpGet]
        public Hall GetHallData(int MovieHallId)
        {
            return _cinemaCustomerService.GetHallData(MovieHallId);
        }

        [Route("api/Movies/GetMovieHallData/{MovieHallId:int}")]
        [HttpGet]
        public MovieHall GetMovieHallData(int MovieHallId)
        {
            return _cinemaCustomerService.GetMovieHallData(MovieHallId);
        }

        [Route("api/Movies/UpdateSeatToCart")]
        [HttpPost]
        public void UpdateSeatToCart(MovieHallDetails Id)
        {
            _cinemaCustomerService.UpdateMovieHallDetailsSeat(Id);
        }

        public Movie Get(int id)
        {
            return _cinemaCustomerService.GetMovieData(id);
        }

        public void Put(MovieHallDetails mhd)
        {
            _cinemaCustomerService.UpdateMovieHallDetailsSeat(mhd);
        }
    }
}
