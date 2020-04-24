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
    public class TransactionsController : ApiController
    {
        private CinemaCustomerRepository cinemaRepo = new CinemaCustomerRepository();
        // GET: api/Transactions
        public IEnumerable<Transactions> Get()
        {
            return cinemaRepo.GetTransactions();
        }

        // GET: api/Transactions/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Transactions
        public void Post(Transactions trans)
        {
            cinemaRepo.AddTransaction(trans);
        }

        // PUT: api/Transactions/5
        public void Put(int id, [FromBody]string value)
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
            return cinemaRepo.GetUserTransactionList(UserDetailsId);
        }
    }
}
