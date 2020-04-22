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
    public class UserDetailsController : ApiController
    {
       private CinemaCustomerRepository cinemaRepo = new CinemaCustomerRepository();
        // GET: api/UserDetails
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserDetails/5
        public UserDetails Get(int id)
        {
            return cinemaRepo.GetUserDetails(id);
        }

        // POST: api/UserDetails
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserDetails/5
        public void Put(UserDetails user)
        {
            cinemaRepo.UpdateUserDetails(user);
        }

        // DELETE: api/UserDetails/5
        public void Delete(int id)
        {
        }

        [Route("api/UserDetails/LoginCheck")]
        [HttpPost]
        public UserDetails LoginCheck(UserDetails user)
        {
            return cinemaRepo.LoginCheck(user);
        }
        [Route("api/UserDetails/TicketTotal/{UserDetailsId:int}")]
        [HttpGet]
        public double TicketTotal(int UserDetailsId)
        {
            return cinemaRepo.TicketTotal(UserDetailsId);
        }
    }
}
