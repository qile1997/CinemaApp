using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CinemaApp.Customer.WebAPILayer.Controllers
{
    public class MovieHallDetailsController : ApiController
    {
        private CinemaCustomerRepository cinemaRepo = new CinemaCustomerRepository();
        // GET: api/MovieHallDetails
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/MovieHallDetails/5
        public MovieHallDetails Get(int id)
        {
            MovieHallDetails mhd = cinemaRepo.GetMovieHallDetail(id);
            return mhd;
        }
        public void Put(MovieHallDetails mhd)
        {
            cinemaRepo.UpdateMovieHallDetailsSeat(mhd);
        }
        // POST: api/MovieHallDetails
        public void Post([FromBody]string value)
        {
        }
        [Route("MovieHallDetails/Seat/{MovieHallId:int}")]
        [HttpGet]
        public void EmptyOrTaken(string Seat,int MovieHallId)
        {
            cinemaRepo.replaceEmptyOrTakenSeat(Seat, MovieHallId);
        }

        // DELETE: api/MovieHallDetails/5
        public void Delete(int id)
        {
        }
 
    }
}
