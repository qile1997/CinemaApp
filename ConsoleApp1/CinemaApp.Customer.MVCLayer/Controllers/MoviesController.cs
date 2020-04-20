using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using CinemaApp.Customer.MVCLayer.ViewModel;
using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance;
using CinemaApp.Persistance.Repository;

namespace CinemaApp.Customer.MVCLayer.Controllers
{
    public class MoviesController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private HttpResponseMessage response;
        private const string Controller = "Movies";

        // GET: Movies
        public ActionResult Index()
        {
            IEnumerable<Movie> movieList;
            response = GlobalVariables.WebApiClient.GetAsync(Controller).Result;
            movieList = response.Content.ReadAsAsync<IEnumerable<Movie>>().Result;

            return View(movieList);
        }
        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage(UserDetails user)
        {
            response = GlobalVariables.WebApiClient.PostAsJsonAsync($"{Controller}/LoginCheck", user).Result;
            UserDetails _user = response.Content.ReadAsAsync<UserDetails>().Result;

            if (_user == null)
            {
                ViewBag.FailMessage = "Username / Password invalid";
                return View();
            }

            Session["UserId"] = _user.Id;
            return RedirectToAction("MovieList");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
        public ActionResult MovieList()
        {
            IEnumerable<Movie> movieList;
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetMovies").Result;
            movieList = response.Content.ReadAsAsync<IEnumerable<Movie>>().Result;

            ViewBag.MovieId = new SelectList(movieList, "MovieId", "MovieId");
            return View(movieList);
        }
        [HttpPost]
        public ActionResult MovieList(int MovieId)
        {
            Session["MovieId"] = MovieId;
            return RedirectToAction("MovieHalls");
        }

        public ActionResult MovieHalls()
        {

            int MovieId = Convert.ToInt32(Session["MovieId"]);

            IEnumerable<MovieHall> movieHalls;
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetSelectedMovies/{MovieId}").Result;
            movieHalls = response.Content.ReadAsAsync<IEnumerable<MovieHall>>().Result;
            ViewBag.MovieHallId = new SelectList(movieHalls, "MovieHallId", "MovieHallId");

            return View(movieHalls);
        }

        [HttpPost]
        public ActionResult MovieHalls(int MovieHallId)
        {
            Session["MovieSeats"] = MovieHallId;
            return RedirectToAction("MovieSeats");
        }
        public ActionResult MovieSeats()
        {
            int UserDetailsId = Convert.ToInt32(Session["UserId"]);
            int MovieHallId = Convert.ToInt32(Session["MovieSeats"]);
            int MovieId = Convert.ToInt32(Session["MovieId"]);

            IEnumerable<MovieHallDetails> movieSeats;
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetMovieSeats/{MovieHallId}").Result;
            movieSeats = response.Content.ReadAsAsync<IEnumerable<MovieHallDetails>>().Result;

            var filterHallNo = (from h in db.Hall
                                join mh in db.MovieHall on h.HallId equals mh.HallId
                                where mh.MovieHallId == MovieHallId
                                select h).SingleOrDefault();

            var filterMovieTitle = (from m in db.Movie
                                    join mh in db.MovieHall on m.MovieId equals mh.MovieId
                                    where mh.HallId == filterHallNo.HallId && mh.MovieHallId == MovieHallId
                                    select m).SingleOrDefault();

            var filterMovieTime = (from m in db.Movie
                                   join mh in db.MovieHall on m.MovieId equals mh.MovieId
                                   where mh.HallId == filterHallNo.HallId && mh.MovieHallId == MovieHallId
                                   select mh).SingleOrDefault();

            var totalPrice = db.UserCarts.Where(d => d.UserDetailsId == UserDetailsId && d.MovieHallsId==MovieHallId).ToList();

            ViewBag.HallNo = filterHallNo.HallNo;
            ViewBag.MovieTitle = filterMovieTitle.MovieTitle;
            ViewBag.MovieDateTime = filterMovieTime.MovieDateTime.ToString("h:mm tt");
            ViewBag.TicketPrice = filterMovieTitle.TicketPrice;
            ViewBag.TotalPrice = filterMovieTitle.TicketPrice * totalPrice.Count();
            return View(movieSeats);
        }

        //Ajax
        [HttpPost]
        public ActionResult UpdateCart(string Seat)
        {
            AppDbContext db = new AppDbContext();
            int UserDetailsId = Convert.ToInt32(Session["UserId"]);
            int MovieId = Convert.ToInt32(Session["MovieId"]);
            int MovieHallId = Convert.ToInt32(Session["MovieSeats"]);

            var filterPrice = db.Movie.Where(d => d.MovieId == MovieId).SingleOrDefault();
            var replaceEmptyOrTakenSeat = db.MovieHallDetails.Where(d => d.Seat == Seat && d.MovieHallId == MovieHallId).SingleOrDefault();

            if (replaceEmptyOrTakenSeat.UserDetailsId == null)
            {
                replaceEmptyOrTakenSeat.SeatStatus = Status.T;
                replaceEmptyOrTakenSeat.UserDetailsId = UserDetailsId;
                db.SaveChanges();
                UserCart cart = new UserCart();
                cart.UserDetailsId = UserDetailsId;
                cart.MovieHallsId = MovieHallId;
                cart.MovieId = MovieId;
                cart.TicketPrice = filterPrice.TicketPrice;
                cart.Seat = Seat;
                db.UserCarts.Add(cart);
                db.SaveChanges();
                return Json(JsonRequestBehavior.AllowGet);
            }
            else
            {
                var removeCart = db.UserCarts.Where(d => d.Seat == Seat && d.UserDetailsId == UserDetailsId && d.MovieHallsId == MovieHallId && d.MovieId == MovieId).SingleOrDefault();
                db.UserCarts.Remove(removeCart);
                replaceEmptyOrTakenSeat.SeatStatus = Status.E;
                replaceEmptyOrTakenSeat.UserDetailsId = null;
                db.SaveChanges();
                return Json(JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult OrderSummary()
        {
            return View(db.UserCarts.ToList());
        }
        public ActionResult ViewCart()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
