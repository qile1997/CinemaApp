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

        private const string HallController = "Hall";

        private const string UserDetailsController = "UserDetails";

        private const string TransactionsController = "Transactions";

        private const string MovieHallDetailsController = "MovieHallDetails";

        private const string MovieHallController = "MovieHall";

        private const string UserCartController = "UserCart";

        // GET: Movies
        public int UserSessionId()
        {
            return Convert.ToInt32(Session["UserId"]);
        }
        public ActionResult Index()
        {

            response = GlobalVariables.WebApiClient.GetAsync(Controller).Result;
            IEnumerable<Movie> movieList = response.Content.ReadAsAsync<IEnumerable<Movie>>().Result;

            return View(movieList);
        }

        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage(UserDetails user)
        {
            response = GlobalVariables.WebApiClient.PostAsJsonAsync($"{UserDetailsController}/LoginCheck", user).Result;
            UserDetails _user = response.Content.ReadAsAsync<UserDetails>().Result;

            if (_user == null)
            {
                ViewBag.FailMessage = "Username / Password invalid";
                return View();
            }

            Session["UserId"] = _user.Id;
            return RedirectToAction("MovieList");
        }
        public ActionResult MovieList()
        {
            IEnumerable<Movie> movieList;
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetAvailableMovies").Result;
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

            //Get Current Hall Data
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetHallData/{MovieHallId}").Result;
            Hall HallData = response.Content.ReadAsAsync<Hall>().Result;

            //Get Movie Data
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/{MovieId}").Result;
            Movie MovieData = response.Content.ReadAsAsync<Movie>().Result;

            //Get Current Movie Hall Data
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetMovieHallData/{MovieHallId}").Result;
            MovieHall MovieHallData = response.Content.ReadAsAsync<MovieHall>().Result;

            var totalPrice = db.UserCarts.Where(d => d.UserDetailsId == UserDetailsId && d.MovieHallsId == MovieHallId && d.ConfirmCart == false).ToList();

            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/{UserDetailsId}").Result;
            UserDetails UserDetails = response.Content.ReadAsAsync<UserDetails>().Result;

            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/TicketTotal/{UserDetailsId}").Result;
            double TicketTotal = response.Content.ReadAsAsync<double>().Result;

            ViewBag.CurrentBalance = UserDetails.Balance;
            ViewBag.RemainingBalance = UserDetails.Balance - TicketTotal;
            Session["BalanceCount"] = UserDetails.Balance - TicketTotal;
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

            int UserDetailsId = UserSessionId();
            int MovieId = Convert.ToInt32(Session["MovieId"]);
            int MovieHallId = Convert.ToInt32(Session["MovieSeats"]);

            //Replacing Empty / Taken Seat
            var replaceEmptyOrTakenSeat = db.MovieHallDetails.Where(d => d.Seat == Seat && d.MovieHallId == MovieHallId).SingleOrDefault();

            //response = GlobalVariables.WebApiClient.GetAsync($"{MovieHallDetailsController}/{Seat}/{MovieHallId}").Result;

            //Get Movie Data
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/{MovieId}").Result;
            Movie MovieData = response.Content.ReadAsAsync<Movie>().Result;

            //Get Current Hall Data
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetHallData/{MovieHallId}").Result;
            Hall HallData = response.Content.ReadAsAsync<Hall>().Result;

            //Get Current Movie Hall Data
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetMovieHallData/{MovieHallId}").Result;
            MovieHall MovieHallData = response.Content.ReadAsAsync<MovieHall>().Result;

            //Get Current User Data
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/{UserDetailsId}").Result;
            UserDetails UserDetails = response.Content.ReadAsAsync<UserDetails>().Result;

            //Ticket Total Price
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/TicketTotal/{UserDetailsId}").Result;
            double TicketTotal = response.Content.ReadAsAsync<double>().Result;

            if (TicketTotal + MovieData.TicketPrice > UserDetails.Balance && replaceEmptyOrTakenSeat.SeatStatus == Status.E)
            {
                return Json(new { Button = true, Msg = UserDetails.Name + " , you need minimum RM" + (MovieData.TicketPrice - Convert.ToDouble(Session["BalanceCount"])) + " to buy seat " + "(" + Seat + ")" }, JsonRequestBehavior.AllowGet);
            }
            int mhdId = replaceEmptyOrTakenSeat.Id;

            if (replaceEmptyOrTakenSeat.UserDetailsId == null)
            {
                //Current Seat Id

                MovieHallDetails mhd = new MovieHallDetails
                {
                    Id = mhdId,
                    MovieHallId = MovieHallId,
                    Seat = Seat,
                    SeatStatus = Status.A,
                    UserDetailsId = UserDetailsId
                };

                response = GlobalVariables.WebApiClient.PutAsJsonAsync($"{MovieHallDetailsController}/{mhdId}", mhd).Result;

                UserCart cart = new UserCart()
                {
                    UserDetailsId = UserDetailsId,
                    MovieHallsId = MovieHallId,
                    MovieTitle = MovieData.MovieTitle,
                    MovieDateTime = MovieHallData.MovieDateTime,
                    HallNo = HallData.HallNo,
                    TicketPrice = MovieData.TicketPrice,
                    Seat = Seat,
                    ConfirmCart = false
                };

                response = GlobalVariables.WebApiClient.PostAsJsonAsync($"{UserCartController}", cart).Result;
                return Json(JsonRequestBehavior.AllowGet);
            }
            else
            {
                var removeCart = db.UserCarts.Where(d => d.Seat == Seat && d.UserDetailsId == UserDetailsId && d.MovieHallsId == MovieHallId).SingleOrDefault();
                response = GlobalVariables.WebApiClient.DeleteAsync($"{UserCartController}/{removeCart.Id}").Result;
                //db.UserCarts.Remove(removeCart);
                //db.SaveChanges();
                MovieHallDetails mhd = new MovieHallDetails
                {
                    Id = mhdId,
                    MovieHallId = MovieHallId,
                    Seat = Seat,
                    SeatStatus = Status.E,
                    UserDetailsId = null
                };
                response = GlobalVariables.WebApiClient.PutAsJsonAsync($"{MovieHallDetailsController}/{mhdId}", mhd).Result;
                return Json(JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ClearHistory()
        {
            int UserDetailsId = UserSessionId();
            var userHistory = db.Transactions.Where(d => d.UserDetailsId == UserDetailsId).ToList();
            db.Transactions.RemoveRange(userHistory);
            db.SaveChanges();
            return RedirectToAction("TransactionView");
        }

        public ActionResult RemoveOrderSummaryConfirmed(int value, int? Id)
        {
            Session["RemoveCartItemId"] = Id;
            ViewBag._TicketPrice = value;
            ViewBag.TicketPrice = value - value * 0.05;
            return View();
        }
        //Ajax
        [HttpPost]
        public ActionResult RemoveOrderSummaryConfirmed(string TotalAmount, int TransferMode, string Remarks)
        {
            AppDbContext db = new AppDbContext();

            int UserDetailsId = UserSessionId();
            int Id = Convert.ToInt32(Session["RemoveCartItemId"]);
            var cart = db.UserCarts.Where(d => d.Id == Id).SingleOrDefault();
            db.UserCarts.Remove(cart);
            db.SaveChanges();

            //Convert Seat from T to E
            var MovieHallDetails = db.MovieHallDetails.Where(d => d.MovieHallId == cart.MovieHallsId && d.UserDetailsId == cart.UserDetailsId && d.SeatStatus == Status.O && d.Seat == cart.Seat).SingleOrDefault();
            MovieHallDetails.SeatStatus = Status.E;
            MovieHallDetails.UserDetailsId = null;
            db.SaveChanges();

            Transactions _trans = new Transactions();
            if (TransferMode == 1)
            {
                _trans.TransferMode = Transfer.IBGT;
            }
            else
            {
                _trans.TransferMode = Transfer.IBG;
            }
            //Add Transaction
            Transactions trans = new Transactions()
            {
                Transaction = TransactionType.Refund,
                TransactionDate = DateTime.Now,
                TransferAmount = "+" + TotalAmount,
                Remarks = Remarks,
                UserDetailsId = UserDetailsId,
                TransferMode = _trans.TransferMode
            };

            response = GlobalVariables.WebApiClient.PostAsJsonAsync($"{TransactionsController}", trans).Result;

            //Return Balance
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/{UserDetailsId}").Result;
            UserDetails UserDetails = response.Content.ReadAsAsync<UserDetails>().Result;

            UserDetails user = new UserDetails()
            {
                Id = UserDetailsId,
                Name = UserDetails.Name,
                Username = UserDetails.Username,
                Password = UserDetails.Password,
                Balance = UserDetails.Balance + Convert.ToDouble(TotalAmount)
            };

            response = GlobalVariables.WebApiClient.PutAsJsonAsync($"{UserDetailsController}", user).Result;
            return RedirectToAction("OrderSummaryConfirmed");
        }
        public ActionResult OrderSummary()
        {
            //User Balance
            int UserDetailsId = UserSessionId();

            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/{UserDetailsId}").Result;
            UserDetails UserDetails = response.Content.ReadAsAsync<UserDetails>().Result;

            var OrderSummary = db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList();

            ViewBag.Count = OrderSummary.Count();
            ViewBag.Balance = UserDetails.Balance;
            ViewBag.Username = UserDetails.Name;
            return View(OrderSummary);
        }
        public ActionResult OrderSummaryConfirmed()
        {
            //User Balance
            int UserDetailsId = UserSessionId();

            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/{UserDetailsId}").Result;
            UserDetails UserDetails = response.Content.ReadAsAsync<UserDetails>().Result;

            var OrderSummaryConfirmed = db.UserCarts.Where(d => d.ConfirmCart == true && d.UserDetailsId == UserDetailsId).ToList();
            ViewBag.Balance = UserDetails.Balance;
            ViewBag.Username = UserDetails.Name;
            return View(OrderSummaryConfirmed);
        }
        public ActionResult Payment()
        {
            int UserDetailsId = UserSessionId();

            var OrderSummary = db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList();

            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/{UserDetailsId}").Result;
            UserDetails UserDetails = response.Content.ReadAsAsync<UserDetails>().Result;

            ViewBag.TotalAmount = OrderSummary.Sum(d => d.TicketPrice);
            ViewBag.Balance = UserDetails.Balance;
            return View();
        }

        [HttpPost]
        public ActionResult Payment(int TransferMode, double TotalAmount)
        {
            int UserDetailsId = UserSessionId();

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

            Transactions _trans = new Transactions();
            if (TransferMode == 1)
            {
                _trans.TransferMode = Transfer.IBGT;
            }
            else
            {
                _trans.TransferMode = Transfer.IBG;
            }

            Transactions trans = new Transactions()
            {
                Transaction = TransactionType.Purchase,
                TransactionDate = DateTime.Now,
                TransferAmount = "-" + TotalAmount,
                UserDetailsId = UserDetailsId,
                TransferMode = _trans.TransferMode
            };
            db.Transactions.Add(trans);

            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/{UserDetailsId}").Result;
            UserDetails UserDetails = response.Content.ReadAsAsync<UserDetails>().Result;

            UserDetails user = new UserDetails()
            {
                Id = UserDetailsId,
                Name = UserDetails.Name,
                Username = UserDetails.Username,
                Password = UserDetails.Password,
                Balance = UserDetails.Balance - TotalAmount
            };

            response = GlobalVariables.WebApiClient.PutAsJsonAsync($"{UserDetailsController}", user).Result;
            return RedirectToAction("OrderSummaryConfirmed");

        }

        public ActionResult TransactionView()
        {
            //User Balance
            int UserDetailsId = UserSessionId();

            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/{UserDetailsId}").Result;
            UserDetails UserDetails = response.Content.ReadAsAsync<UserDetails>().Result;

            ViewBag.Balance = UserDetails.Balance;
            ViewBag.Username = UserDetails.Name;

            response = GlobalVariables.WebApiClient.GetAsync($"{TransactionsController}/{UserDetailsId}").Result;
            IEnumerable<Transactions> transactionList = response.Content.ReadAsAsync<IEnumerable<Transactions>>().Result;

            //var UserTransaction = db.Transactions.Where(d => d.UserDetailsId == UserDetailsId).ToList();
            return View(transactionList);
        }
        public ActionResult Logout()
        {
            int UserDetailsId = UserSessionId();
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
