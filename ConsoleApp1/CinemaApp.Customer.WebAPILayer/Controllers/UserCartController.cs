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
    public class UserCartController : ApiController
    {
        private CinemaCustomerRepository cinemaRepo = new CinemaCustomerRepository();
        // GET: api/UserCart
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserCart/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserCart
        public void Post(UserCart cart)
        {
            cinemaRepo.AddCart(cart);
        }

        // PUT: api/UserCart/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserCart/5
        public void Delete(int id)
        {
            UserCart cart = cinemaRepo.GetCart(id);
            cinemaRepo.DeleteCart(cart);
        }
        [Route("api/UserCart/ReplaceUnorderedSeats/{UserDetailsId:int}")]
        [HttpGet]
        public void ReplaceUnorderedSeats(int UserDetailsId)
        {
            cinemaRepo.ReplaceUnorderedSeats(UserDetailsId);
        }
        [Route("api/UserCart/GetUnorderedSeats/{UserDetailsId:int}")]
        [HttpGet]
        public IEnumerable<UserCart> GetUnorderedSeats(int UserDetailsId)
        {
            return cinemaRepo.GetUnorderedSeats(UserDetailsId);
        }
        [Route("api/UserCart/OrderSummaryConfirmedList/{UserDetailsId:int}")]
        [HttpGet]
        public IEnumerable<UserCart> OrderSummaryConfirmedList(int UserDetailsId)
        {
            return cinemaRepo.OrderSummaryConfirmedList(UserDetailsId);
        }
    }
}
