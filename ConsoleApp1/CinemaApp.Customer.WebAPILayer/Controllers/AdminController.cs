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
    public class AdminController : ApiController
    {
        private CinemaAdminRepository adminRepo = new CinemaAdminRepository();
        [Route("api/Admin/GenerateUserDetails")]
        [HttpGet]
        public void GenerateUserDetails()
        {
           adminRepo.GenerateUserDetails();
        }
        [Route("api/Admin/GenerateMovies")]
        [HttpGet]
        public void GenerateMovies()
        {
            adminRepo.GenerateMovies();
        }
        [Route("api/Admin/GenerateHalls")]
        [HttpGet]
        public void GenerateHalls()
        {
            adminRepo.GenerateHalls();
        }
        [Route("api/Admin/GenerateMovieHalls")]
        [HttpGet]
        public void GenerateMovieHalls()
        {
            adminRepo.GenerateMovieHalls();
        }
        [Route("api/Admin/GenerateMovieHallDetails")]
        [HttpGet]
        public void GenerateMovieHallDetails()
        {
            adminRepo.GenerateMovieHallDetails();
        }
        [Route("api/Admin/ClearUserCart")]
        [HttpGet]
        public void ClearUserCart()
        {
            adminRepo.ClearUserCart();
        }
        [Route("api/Admin/ClearAllData")]
        [HttpGet]
        public void ClearAllData()
        {
            adminRepo.ClearAllData();
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
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Admin/5
        public void Delete(int id)
        {
        }
    }
}
