using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance.Service;
using System.Collections.Generic;
using System.Web.Http;

namespace CinemaApp.Customer.WebAPILayer.Controllers
{
    public class AdminController : ApiController
    {
        private CinemaAdminService _cinemaAdminService = new CinemaAdminService();
        [Route("api/Admin/GenerateUserDetails")]
        [HttpGet]
        public void GenerateUserDetails()
        {
            _cinemaAdminService.GenerateUserDetails();
        }
        [Route("api/Admin/GenerateMovies")]
        [HttpGet]
        public void GenerateMovies()
        {
            _cinemaAdminService.GenerateMovies();
        }
        [Route("api/Admin/GenerateHalls")]
        [HttpGet]
        public void GenerateHalls()
        {
            _cinemaAdminService.GenerateHalls();
        }
        [Route("api/Admin/GenerateMovieHalls")]
        [HttpGet]
        public void GenerateMovieHalls()
        {
            _cinemaAdminService.GenerateMovieHalls();
        }
        [Route("api/Admin/GenerateMovieHallDetails")]
        [HttpGet]
        public void GenerateMovieHallDetails()
        {
            _cinemaAdminService.GenerateMovieHallDetails();
        }
        [Route("api/Admin/ClearUserCart")]
        [HttpGet]
        public void ClearUserCart()
        {
            _cinemaAdminService.ClearUserCart();
        }
        [Route("api/Admin/ClearAllData")]
        [HttpGet]
        public void ClearAllData()
        {
            _cinemaAdminService.ClearAllData();
        }
        // GET: api/Admin
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Admin/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Admin
        public void Post(UserDetails user)
        {
        }

        // PUT: api/Admin/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Admin/5
        public void Delete(int id)
        {
        }
    }
}
