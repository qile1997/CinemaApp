using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance;
using CinemaApp.Persistance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CinemaApp.Customer.WebAPILayer.Controllers
{
    public class MoviesController : ApiController
    {
        private CinemaCustomerRepository cinemaRepo = new CinemaCustomerRepository();
        // GET: api/Movies
        public IEnumerable<Movie> Get()
        {
            return cinemaRepo.GetMovies();
        }
        [Route("api/Movies/GetUserDetails/{UserDetailsId}")]
        [HttpGet]
        public UserDetails GetUserDetails(int UserDetailsId)
        {
            return cinemaRepo.GetUserDetails(UserDetailsId);
        }
        [Route("api/Movies/GetAvailableMovies")]
        [HttpGet]
        public IEnumerable<Movie> GetAvailableMovies()
        {
            return cinemaRepo.GetAvailableMovies();
        }
        [Route("api/Movies/GetSelectedMovies/{MovieId:int}")]
        [HttpGet]
        public IEnumerable<MovieHall> GetSelectedMovies(int MovieId)
        {
            return cinemaRepo.GetSelectedMovies(MovieId);
        }
        [Route("api/Movies/GetMovieSeats/{MovieHallId:int}")]
        [HttpGet]
        public IEnumerable<MovieHallDetails> GetMovieSeats(int MovieHallId)
        {
            return cinemaRepo.GetMovieSeats(MovieHallId);
        }

        [Route("api/Movies/GetHallData/{MovieHallId:int}")]
        [HttpGet]
        public Hall GetHallData(int MovieHallId)
        {
            return cinemaRepo.GetHallData(MovieHallId);
        }
        [Route("api/Movies/GetMovieHallData/{MovieHallId:int}")]
        [HttpGet]
        public MovieHall GetMovieHallData(int MovieHallId)
        {
            return cinemaRepo.GetMovieHallData(MovieHallId);
        }
        //[Route("api/Movies/GetMovieData/{MovieId:int}")]
        //[HttpGet]
        //public Movie GetMovieData(int MovieId)
        //{
        //    return cinemaRepo.GetMovieData(MovieId);
        //}
     
        [Route("api/Movies/UpdateSeatToCart")]
        [HttpPost]
        public void UpdateSeatToCart(MovieHallDetails Id)
        {
            cinemaRepo.UpdateMovieHallDetailsSeat(Id);
        }
        //[Route("api/Movies/GetMovieSeatDetails/{MovieHallId:int}/Seat")]
        //[HttpPost]
        //public MovieHallDetails replaceEmptyOrTakenSeat(int MovieHallId,string Seat)
        //{
        //    return cinemaRepo.replaceEmptyOrTakenSeat(Seat, MovieHallId);
        //}

        public Movie Get(int id)
        {
            Movie md = cinemaRepo.GetMovieData(id);
            return md;
        }
        public void Put(MovieHallDetails mhd)
        {
            cinemaRepo.UpdateMovieHallDetailsSeat(mhd);
        }
        //// POST: api/Movies
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Movies/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Movies/5
        //public void Delete(int id)
        //{
        //}
    }
}
