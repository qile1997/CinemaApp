using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CinemaApp.Customer.WebAPILayer.Controllers
{
    public class MovieHallController : ApiController
    {
        // GET: api/MovieHall
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/MovieHall/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MovieHall
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/MovieHall/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/MovieHall/5
        public void Delete(int id)
        {
        }
    }
}
