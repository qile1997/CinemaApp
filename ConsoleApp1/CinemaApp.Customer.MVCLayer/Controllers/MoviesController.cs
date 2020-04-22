using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using CinemaApp.Customer.MVCLayer.ViewModel;
using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance;

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
            int UserDetailsId = Convert.ToInt32(Session["UserId"]);
            var RemoveUnconfirmedOrders = db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList();
            db.UserCarts.RemoveRange(RemoveUnconfirmedOrders);

            var _RemoveUnconfirmedOrders = (from mhd in db.MovieHallDetails
                                            join uc in db.UserCarts
                                             on mhd.Seat equals uc.Seat
                                            where mhd.UserDetailsId == UserDetailsId && uc.ConfirmCart == false && uc.UserDetailsId == UserDetailsId
                                            select mhd).ToList();

            foreach (var item in _RemoveUnconfirmedOrders)
            {
                item.SeatStatus = Status.E;
                item.UserDetailsId = null;
            }

            db.SaveChanges();
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

            Session["PaymentId"] = UserDetailsId;
            Session.Timeout = 5;

            IEnumerable<MovieHallDetails> movieSeats;
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetMovieSeats/{MovieHallId}").Result;
            movieSeats = response.Content.ReadAsAsync<IEnumerable<MovieHallDetails>>().Result;

            var HallData = (from h in db.Hall
                            join mh in db.MovieHall on h.HallId equals mh.HallId
                            where mh.MovieHallId == MovieHallId
                            select h).SingleOrDefault();

            var MovieData = (from m in db.Movie
                             join mh in db.MovieHall on m.MovieId equals mh.MovieId
                             where mh.HallId == HallData.HallId && mh.MovieHallId == MovieHallId
                             select m).SingleOrDefault();

            var MovieHallData = (from m in db.Movie
                                 join mh in db.MovieHall on m.MovieId equals mh.MovieId
                                 where mh.HallId == HallData.HallId && mh.MovieHallId == MovieHallId
                                 select mh).SingleOrDefault();

            var totalPrice = db.UserCarts.Where(d => d.UserDetailsId == UserDetailsId && d.MovieHallsId == MovieHallId && d.ConfirmCart == false).ToList();

            var UserDetails = db.UserDetails.Where(d => d.Id == UserDetailsId).SingleOrDefault();

            double BalanceCount = db.UserCarts.Where(d => d.UserDetailsId == UserDetailsId && d.ConfirmCart == false).Sum(d => (int?)d.TicketPrice) ?? 0;

            ViewBag.CurrentBalance = UserDetails.Balance;
            ViewBag.RemainingBalance = UserDetails.Balance - BalanceCount;
            Session["BalanceCount"] = UserDetails.Balance - BalanceCount;
            ViewBag.Username = UserDetails.Name;
            ViewBag.HallNo = HallData.HallNo;
            ViewBag.MovieTitle = MovieData.MovieTitle;
            ViewBag.MovieDateTime = MovieHallData.MovieDateTime.ToString("h:mm tt");
            ViewBag.TicketPrice = MovieData.TicketPrice;
            ViewBag.UserDetailsId = UserDetailsId;
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

            var MovieData = db.Movie.Where(d => d.MovieId == MovieId).SingleOrDefault();

            var replaceEmptyOrTakenSeat = db.MovieHallDetails.Where(d => d.Seat == Seat && d.MovieHallId == MovieHallId).SingleOrDefault();

            var HallData = (from h in db.Hall
                            join mh in db.MovieHall on h.HallId equals mh.HallId
                            where mh.MovieHallId == MovieHallId
                            select h).SingleOrDefault();

            var MovieHallData = (from h in db.Hall
                                 join mh in db.MovieHall on h.HallId equals mh.HallId
                                 where mh.MovieHallId == MovieHallId
                                 select mh).SingleOrDefault();

            var UserDetails = db.UserDetails.Where(d => d.Id == UserDetailsId).SingleOrDefault();

            double TicketTotal = db.UserCarts.Where(d => d.UserDetailsId == UserDetailsId && d.ConfirmCart == false).Sum(d => (int?)d.TicketPrice) ?? 0;

            if (TicketTotal  + MovieData.TicketPrice > UserDetails.Balance && replaceEmptyOrTakenSeat.SeatStatus == Status.E)
            {
                return Json(new { Button = true, Msg = UserDetails.Name + " , you need minimum RM" + (MovieData.TicketPrice - Convert.ToDouble(Session["BalanceCount"]) ) + " to buy seat " + "(" + Seat + ")" }, JsonRequestBehavior.AllowGet);
            }

            if (replaceEmptyOrTakenSeat.UserDetailsId == null)
            {

                replaceEmptyOrTakenSeat.SeatStatus = Status.A;
                replaceEmptyOrTakenSeat.UserDetailsId = UserDetailsId;
                db.SaveChanges();

                UserCart cart = new UserCart();
                cart.UserDetailsId = UserDetailsId;
                cart.MovieHallsId = MovieHallId;
                cart.MovieTitle = MovieData.MovieTitle;
                cart.MovieDateTime = MovieHallData.MovieDateTime;
                cart.HallNo = HallData.HallNo;
                cart.TicketPrice = MovieData.TicketPrice;
                cart.Seat = Seat;
                cart.ConfirmCart = false;
                db.UserCarts.Add(cart);
                db.SaveChanges();
                return Json(JsonRequestBehavior.AllowGet);
            }
            else
            {
                var removeCart = db.UserCarts.Where(d => d.Seat == Seat && d.UserDetailsId == UserDetailsId && d.MovieHallsId == MovieHallId).SingleOrDefault();
                db.UserCarts.Remove(removeCart);
                replaceEmptyOrTakenSeat.SeatStatus = Status.E;
                replaceEmptyOrTakenSeat.UserDetailsId = null;
                db.SaveChanges();
                return Json(JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ClearHistory()
        {
            int UserDetailsId = Convert.ToInt32(Session["UserId"]);
            var userHistory = db.Transactions.Where(d => d.UserDetailsId == UserDetailsId).ToList();
            db.Transactions.RemoveRange(userHistory);
            db.SaveChanges();
            return RedirectToAction("TransactionView");
        }
   
        public ActionResult RemoveOrderSummaryConfirmed(int value, int? Id)
        {
            Session["RemoveCartItemId"] = Id;
            ViewBag._TicketPrice = value;
            ViewBag.TicketPrice = value + value * 0.05;
            return View();
        }
        //Ajax
        [HttpPost]
        public ActionResult RemoveOrderSummaryConfirmed(string TotalAmount, int TransferMode,string Remarks)
        {
            AppDbContext db = new AppDbContext();

            int UserDetailsId = Convert.ToInt32(Session["UserId"]);
            int Id = Convert.ToInt32(Session["RemoveCartItemId"]);
            var cart = db.UserCarts.Where(d => d.Id == Id).SingleOrDefault();
            db.UserCarts.Remove(cart);
            db.SaveChanges();

            //Convert Seat from T to E
            var MovieHallDetails = db.MovieHallDetails.Where(d => d.MovieHallId == cart.MovieHallsId && d.UserDetailsId == cart.UserDetailsId && d.SeatStatus == Status.O && d.Seat == cart.Seat).SingleOrDefault();
            MovieHallDetails.SeatStatus = Status.E;
            MovieHallDetails.UserDetailsId = null;
            db.SaveChanges();

            //Add Transaction
            Transactions trans = new Transactions();
            trans.Transaction = TransactionType.Refund;
            trans.TransactionDate = DateTime.Now;
            trans.TransferAmount = "+" + TotalAmount;
            trans.Remarks = Remarks;

            if (TransferMode == 1)
            {
                trans.TransferMode = Transfer.IBGT;
            }
            else
            {
                trans.TransferMode = Transfer.IBG;
            }
            trans.UserDetailsId = UserDetailsId;
            db.Transactions.Add(trans);
            db.SaveChanges();
            //Return Balance
            var UserBalance = db.UserDetails.Where(d => d.Id == UserDetailsId).SingleOrDefault();
            UserBalance.Balance += Convert.ToDouble(TotalAmount);
            db.SaveChanges();
            return RedirectToAction("OrderSummaryConfirmed");
        }
        public ActionResult OrderSummaryConfirmed()
        {
            //User Balance
            int UserDetailsId = Convert.ToInt32(Session["UserId"]);
            var CheckBalance = db.UserDetails.Where(d => d.Id == UserDetailsId).SingleOrDefault();
            ViewBag.Balance = CheckBalance.Balance;

            var OrderSummaryConfirmed = db.UserCarts.Where(d => d.ConfirmCart == true && d.UserDetailsId == UserDetailsId).ToList();
            var UserDetails = db.UserDetails.Where(d => d.Id == UserDetailsId).SingleOrDefault();
            ViewBag.Username = UserDetails.Name;
            return View(OrderSummaryConfirmed);
        }

        public ActionResult OrderSummary()
        {
            //User Balance
            int UserDetailsId = Convert.ToInt32(Session["UserId"]);
            var UserDetails = db.UserDetails.Where(d => d.Id == UserDetailsId).SingleOrDefault();
            var OrderSummary = db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList();

            ViewBag.Count = OrderSummary.Count();
            ViewBag.Balance = UserDetails.Balance;
            ViewBag.Username = UserDetails.Name;
            return View(OrderSummary);
        }
        public ActionResult MoviePayment()
        {
            int UserDetailsId = Convert.ToInt32(Session["UserId"]);
            var OrderSummary = db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList();
            var _TransferAmount = OrderSummary.Sum(d => d.TicketPrice);
            MoviePaymentDetails mpd = new MoviePaymentDetails
            {
                TransferAmount = _TransferAmount
            };
         
            return View(mpd);
        }
        public ActionResult Payment()
        {
            int UserDetailsId = Convert.ToInt32(Session["UserId"]);
            var OrderSummary = db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList();
            ViewBag.TotalAmount = OrderSummary.Sum(d => d.TicketPrice);
            return View();
        }

        [HttpPost]
        public ActionResult Payment(int TransferMode, double TotalAmount)
        {
            int UserDetailsId = Convert.ToInt32(Session["UserId"]);

            var OrderSummary = db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList();
            var _OrderSummary = db.MovieHallDetails.Where(d => d.UserDetailsId == UserDetailsId).ToList();

            foreach (var item in _OrderSummary)
            {
                item.SeatStatus = Status.O;
                db.SaveChanges();
            }

            foreach (var item in OrderSummary)
            {
                item.ConfirmCart = true;
                db.SaveChanges();
            }

            Transactions trans = new Transactions();
            trans.Transaction = TransactionType.Purchase;
            trans.TransactionDate = DateTime.Now;
            trans.TransferAmount = "-" + TotalAmount;
            trans.UserDetailsId = UserDetailsId;
            if (TransferMode == 1)
            {
                trans.TransferMode = Transfer.IBGT;
            }
            else
            {
                trans.TransferMode = Transfer.IBG;
            }
            db.Transactions.Add(trans);

            var UserBalance = db.UserDetails.Where(d => d.Id == UserDetailsId).SingleOrDefault();
            UserBalance.Balance -= TotalAmount;

            db.SaveChanges();
            return RedirectToAction("OrderSummaryConfirmed");

        }
        public ActionResult TransactionView()
        {
            //User Balance
            int UserDetailsId = Convert.ToInt32(Session["UserId"]);
            var UserDetails = db.UserDetails.Where(d => d.Id == UserDetailsId).SingleOrDefault();
            ViewBag.Balance = UserDetails.Balance;
            ViewBag.Username = UserDetails.Name;
            var UserTransaction = db.Transactions.Where(d => d.UserDetailsId == UserDetailsId).ToList();
            return View(UserTransaction);
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
