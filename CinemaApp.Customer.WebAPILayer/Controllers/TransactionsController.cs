using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance.Service;
using System.Collections.Generic;
using System.Web.Http;

namespace CinemaApp.Customer.WebAPILayer.Controllers
{
    public class TransactionsController : ApiController
    {
        private CinemaCustomerService _cinemaCustomerService = new CinemaCustomerService();
        // GET: api/Transactions
        public IEnumerable<Transactions> Get()
        {
            return _cinemaCustomerService.GetTransactions();
        }

        // GET: api/Transactions/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Transactions
        public void Post(Transactions trans)
        {
            _cinemaCustomerService.AddTransaction(trans);
        }

        // PUT: api/Transactions/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Transactions/5
        public void Delete(int id)
        {
        }
        [Route("api/Transactions/GetUserTransactionList/{UserDetailsId:int}")]
        [HttpGet]
        public IEnumerable<Transactions> GetUserTransactionList(int UserDetailsId)
        {
            return _cinemaCustomerService.GetUserTransactionList(UserDetailsId);
        }
    }
}
