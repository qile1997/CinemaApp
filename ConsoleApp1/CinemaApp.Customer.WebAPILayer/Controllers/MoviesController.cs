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
        [Route("api/Movies/LoginCheck")]
        [HttpPost]
        public UserDetails LoginCheck(UserDetails user)
        {
            return cinemaRepo.LoginCheck(user);
        }
        [Route("api/Movies/GetMovies")]
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
        //[Route("api/Movies/GetAvailableSeats/{MovieHallId:int}")]
        //[HttpGet]
        //public IEnumerable<MovieHallDetails> GetAvailableSeats(int MovieHallId)
        //{
        //    return cinemaRepo.GetAvailableSeats(MovieHallId);
        //}
        //[Route("api/Movies/ReplaceEmptySeat/Seat/{MovieHallId:int}/{user:int}/{MovieId:int}")]
        //[HttpPost]
        //public void ReplaceEmptySeat(string Seat, int MovieHallId, int user, int MovieId)
        //{
        //    cinemaRepo.ReplaceEmptySeats(Seat, MovieHallId, user, MovieId);
        //}
        //[Route("api/Movies/ReplaceEmptySeat/EmptySeats/{MovieHallId:int}")]
        //[HttpGet]
        //public void ReplaceEmptySeat(string EmptySeats, int MovieHallId)
        //{
        //    //cinemaRepo.ReplaceEmptySeats(EmptySeats, MovieHallId);
        //}

        //// GET: api/Movies/5
        //public Movie Get(int id)
        //{
        //    Movie movie = db.Movie.Find(id);
        //    return movie;
        //}

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
