using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance.Service;
using System.Collections.Generic;
using System.Web.Http;

namespace CinemaApp.Customer.WebAPILayer.Controllers
{
    public class UserCartController : ApiController
    {
        private CinemaCustomerService _cinemaCustomerService = new CinemaCustomerService();
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
            _cinemaCustomerService.AddCart(cart);
        }

        // PUT: api/UserCart/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/UserCart/5
        public void Delete(int id)
        {
            _cinemaCustomerService.RemoveUserCart(_cinemaCustomerService.GetUserCartById(id));
        }

        [Route("api/UserCart/ReplaceUnorderedSeats/{UserDetailsId:int}")]
        [HttpGet]
        public void ReplaceUnorderedSeats(int UserDetailsId)
        {
            _cinemaCustomerService.ReplaceUnorderedSeats(UserDetailsId);
        }

        [Route("api/UserCart/GetUnorderedSeats/{UserDetailsId:int}")]
        [HttpGet]
        public IEnumerable<UserCart> GetUnorderedSeats(int UserDetailsId)
        {
            return _cinemaCustomerService.GetUnorderedSeats(UserDetailsId);
        }

        [Route("api/UserCart/OrderSummaryConfirmedList/{UserDetailsId:int}")]
        [HttpGet]
        public IEnumerable<UserCart> OrderSummaryConfirmedList(int UserDetailsId)
        {
            return _cinemaCustomerService.OrderSummaryConfirmedList(UserDetailsId);
        }

        [Route("api/UserCart/GetTransferMode/{TransferMode:int}")]
        [HttpGet]
        public Transfer TransferMode(int TransferMode)
        {
            return _cinemaCustomerService.GetTransactionMode(TransferMode);
        }
    }
}
