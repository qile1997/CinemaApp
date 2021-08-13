using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance.Service;
using System.Collections.Generic;
using System.Web.Http;

namespace CinemaApp.Customer.WebAPILayer.Controllers
{
    public class UserDetailsController : ApiController
    {
        private CinemaCustomerService _cinemaCustomerService = new CinemaCustomerService();
        // GET: api/UserDetails
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserDetails/5
        public UserDetails Get(int id)
        {
            return _cinemaCustomerService.GetUserDetails(id);
        }

        // POST: api/UserDetails
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/UserDetails/5
        public void Put(UserDetails user)
        {
            _cinemaCustomerService.UpdateUserDetails(user);
        }

        // DELETE: api/UserDetails/5
        public void Delete(int id)
        {
        }

        [Route("api/UserDetails/LoginCheck")]
        [HttpPost]
        public UserDetails LoginCheck(UserDetails user)
        {
            return _cinemaCustomerService.LoginCheck(user);
        }

        [Route("api/UserDetails/TicketTotal/{UserDetailsId:int}")]
        [HttpGet]
        public double TicketTotal(int UserDetailsId)
        {
            return _cinemaCustomerService.TicketTotal(UserDetailsId);
        }

        [Route("api/UserDetails/GetUnorderedSeats/{UserDetailsId:int}")]
        [HttpGet]
        public IEnumerable<UserCart> GetUnorderedSeats(int UserDetailsId)
        {
            return _cinemaCustomerService.GetUnorderedSeats(UserDetailsId);
        }

        [Route("api/UserDetails/RemoveUnconfirmedOrders/{UserDetailsId:int}")]
        [HttpGet]
        public void RemoveUnconfirmedOrders(int UserDetailsId)
        {
            _cinemaCustomerService.RemoveUnconfirmedOrders(UserDetailsId);
        }
    }
}
