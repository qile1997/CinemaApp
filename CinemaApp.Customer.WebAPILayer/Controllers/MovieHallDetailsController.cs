using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance.Service;
using System.Collections.Generic;
using System.Web.Http;

namespace CinemaApp.Customer.WebAPILayer.Controllers
{
    public class MovieHallDetailsController : ApiController
    {
        private CinemaCustomerService _cinemaCustomerService = new CinemaCustomerService();
        // GET: api/MovieHallDetails
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/MovieHallDetails/5
        public MovieHallDetails Get(int id)
        {
            return _cinemaCustomerService.GetMovieHallDetail(id);
        }
        public void Put(MovieHallDetails mhd)
        {
            _cinemaCustomerService.UpdateMovieHallDetailsSeat(mhd);
        }

        // POST: api/MovieHallDetails
        public void Post([FromBody] string value)
        {
        }

        [Route("MovieHallDetails/MovieSeat/{MovieHallId:int}")]
        [HttpGet]
        public void EmptyOrTaken(string MovieSeat, int MovieHallId)
        {
            _cinemaCustomerService.replaceEmptyOrTakenSeat(MovieSeat, MovieHallId);
        }

        // DELETE: api/MovieHallDetails/5
        public void Delete(int id)
        {
        }

    }
}
