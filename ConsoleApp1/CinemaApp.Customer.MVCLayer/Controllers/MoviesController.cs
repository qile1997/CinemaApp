using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
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


        public int UserIdSession()
        {
            return Convert.ToInt32(Session["UserId"]);
        }
        public int MovieSessionId()
        {
            return Convert.ToInt32(Session["MovieId"]);
        }
        public int MovieHallIdSession()
        {
            return Convert.ToInt32(Session["MovieHallId"]);
        }
        //Get User Data
        public UserDetails GetUserDetails()
        {
            int UserDetailsId = UserIdSession();
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/{UserDetailsId}").Result;
            UserDetails UserDetails = response.Content.ReadAsAsync<UserDetails>().Result;
            return UserDetails;
        }
        //Get Movie data
        public Movie GetMovieData()
        {
            int MovieId = MovieSessionId();
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/{MovieId}").Result;
            Movie MovieData = response.Content.ReadAsAsync<Movie>().Result;
            return MovieData;
        }
        //Get Hall data
        public Hall GetHallData()
        {
            int MovieHallId = MovieHallIdSession();
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetHallData/{MovieHallId}").Result;
            Hall HallData = response.Content.ReadAsAsync<Hall>().Result;
            return HallData;
        }
        //Get MovieHall Data
        public MovieHall GetMovieHallData()
        {
            int MovieHallId = MovieHallIdSession();
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetMovieHallData/{MovieHallId}").Result;
            MovieHall MovieHallData = response.Content.ReadAsAsync<MovieHall>().Result;
            return MovieHallData;
        }
        //Get Ticket Price From Cart
        public double GetTicketPrice()
        {
            int UserDetailsId = UserIdSession();
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/TicketTotal/{UserDetailsId}").Result;
            double TicketTotal = response.Content.ReadAsAsync<double>().Result;
            return TicketTotal;
        }
        //Update User
        public void UpdateUserDetails(UserDetails user)
        {
            response = GlobalVariables.WebApiClient.PutAsJsonAsync($"{UserDetailsController}", user).Result;
        }
        //Update Transaction
        public void UpdateTransaction(Transactions trans)
        {
            response = GlobalVariables.WebApiClient.PostAsJsonAsync($"{TransactionsController}", trans).Result;
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
            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetAvailableMovies").Result;
            IEnumerable<Movie> movieList = response.Content.ReadAsAsync<IEnumerable<Movie>>().Result;

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
            int MovieId = MovieSessionId();

            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetSelectedMovies/{MovieId}").Result;
            IEnumerable<MovieHall> movieHalls = response.Content.ReadAsAsync<IEnumerable<MovieHall>>().Result;
            ViewBag.MovieHallId = new SelectList(movieHalls, "MovieHallId", "MovieHallId");

            return View(movieHalls);
        }

        [HttpPost]
        public ActionResult MovieHalls(int MovieHallId)
        {
            Session["MovieHallId"] = MovieHallId;
            return RedirectToAction("MovieSeats");
        }
        public ActionResult MovieSeats()
        {
            int UserDetailsId = UserIdSession();
            int MovieHallId = MovieHallIdSession();

            //5 minutes for payment
            Session["PaymentId"] = UserDetailsId;
            Session.Timeout = 5;

            response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GetMovieSeats/{MovieHallId}").Result;
            IEnumerable<MovieHallDetails> movieSeats = response.Content.ReadAsAsync<IEnumerable<MovieHallDetails>>().Result;

            //Get Current Hall Data
            Hall HallData = GetHallData();

            //Get Movie Data
            Movie MovieData = GetMovieData();

            //Get Current Movie Hall Data
            MovieHall MovieHallData = GetMovieHallData();

            //Get Current User Data
            UserDetails UserDetails = GetUserDetails();

            //Ticket Total Price
            double TicketTotal = GetTicketPrice();

            ViewBag.CurrentBalance = UserDetails.Balance;
            ViewBag.RemainingBalance = UserDetails.Balance - TicketTotal;
            Session["BalanceCount"] = UserDetails.Balance - TicketTotal;
            ViewBag.Username = UserDetails.Name;
            ViewBag.HallNo = HallData.HallNo;
            ViewBag.MovieId = MovieData.MovieId;
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

            int UserDetailsId = UserIdSession();
            int MovieId = MovieSessionId();
            int MovieHallId = MovieHallIdSession();

            //Replacing Empty / Taken Seat
            var replaceEmptyOrTakenSeat = db.MovieHallDetails.Where(d => d.Seat == Seat && d.MovieHallId == MovieHallId).SingleOrDefault();

            //Get Movie Data
            Movie MovieData = GetMovieData();

            //Get Current Hall Data
            Hall HallData = GetHallData();

            //Get Current Movie Hall Data
            MovieHall MovieHallData = GetMovieHallData();

            //Get Current User Data
            UserDetails UserDetails = GetUserDetails();

            //Ticket Total Price
            double TicketTotal = GetTicketPrice();

            //Calculate minimum balance needed for ticket
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
            int UserDetailsId = UserIdSession();
            response = GlobalVariables.WebApiClient.GetAsync($"{TransactionsController}/GetUserTransactionList/{UserDetailsId}").Result;
            IEnumerable<Transactions> transactionList = response.Content.ReadAsAsync<IEnumerable<Transactions>>().Result;

            //Error
            db.Transactions.RemoveRange(transactionList);
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

            int UserDetailsId = UserIdSession();
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
            if (Remarks == "")
            {
                _trans.Remarks = "-";
            }
            else
            {
                _trans.Remarks = Remarks;
            }
            //Add Transaction
            Transactions trans = new Transactions()
            {
                Transaction = TransactionType.Refund,
                TransactionDate = DateTime.Now,
                TransferAmount = "+" + TotalAmount,
                Remarks = _trans.Remarks,
                UserDetailsId = UserDetailsId,
                TransferMode = _trans.TransferMode
            };

            UpdateTransaction(trans);
            //Return Balance
            //Get Current User Data
            UserDetails UserDetails = GetUserDetails();

            UserDetails user = new UserDetails()
            {
                Id = UserDetailsId,
                Name = UserDetails.Name,
                Username = UserDetails.Username,
                Password = UserDetails.Password,
                Balance = UserDetails.Balance + Convert.ToDouble(TotalAmount)
            };

            UpdateUserDetails(user);
            return RedirectToAction("OrderSummaryConfirmed");
        }
        public ActionResult OrderSummary()
        {
            //Get Current User Data
            UserDetails UserDetails = GetUserDetails();

            //Ticket Total Price
            double TicketTotal = GetTicketPrice();

            var OrderSummary = GetOrderSummary();

            ViewBag.Count = OrderSummary.Count();
            ViewBag.RemainingBalance = UserDetails.Balance - TicketTotal;
            ViewBag.Balance = UserDetails.Balance;
            ViewBag.Username = UserDetails.Name;
            return View(OrderSummary);
        }
        public IEnumerable<UserCart> GetOrderSummary()
        {
            int UserDetailsId = UserIdSession();

            response = GlobalVariables.WebApiClient.GetAsync($"{UserCartController}/GetUnorderedSeats/{UserDetailsId}").Result;
            IEnumerable<UserCart> OrderSummary = response.Content.ReadAsAsync<IEnumerable<UserCart>>().Result;
            return OrderSummary;
        }
        public ActionResult OrderSummaryConfirmed()
        {
            //User Balance
            int UserDetailsId = UserIdSession();

            //Get Current User Data
            UserDetails UserDetails = GetUserDetails();

            response = GlobalVariables.WebApiClient.GetAsync($"{UserCartController}/OrderSummaryConfirmedList/{UserDetailsId}").Result;
            IEnumerable<UserCart> OrderSummaryConfirmedList = response.Content.ReadAsAsync<IEnumerable<UserCart>>().Result;

            ViewBag.Balance = UserDetails.Balance;
            ViewBag.Username = UserDetails.Name;
            return View(OrderSummaryConfirmedList);
        }
        public ActionResult Payment()
        {
            //int UserDetailsId = UserIdSession();

            //Sum of cart seats ticket price
            var OrderSummary = GetOrderSummary();

            //Get Current User Data
            UserDetails UserDetails = GetUserDetails();

            ViewBag.TotalAmount = OrderSummary.Sum(d => d.TicketPrice);
            ViewBag.Balance = UserDetails.Balance;
            return View();
        }

        [HttpPost]
        public ActionResult Payment(int TransferMode, double TotalAmount)
        {
            int UserDetailsId = UserIdSession();

            response = GlobalVariables.WebApiClient.GetAsync($"{UserCartController}/ReplaceUnorderedSeats/{UserDetailsId}").Result;

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
                TransferMode = _trans.TransferMode,
                Remarks = "-"
            };
            UpdateTransaction(trans);

            //Get Current User Data
            UserDetails UserDetails = GetUserDetails();

            UserDetails user = new UserDetails()
            {
                Id = UserDetailsId,
                Name = UserDetails.Name,
                Username = UserDetails.Username,
                Password = UserDetails.Password,
                Balance = UserDetails.Balance - TotalAmount
            };

            UpdateUserDetails(user);
            return RedirectToAction("OrderSummaryConfirmed");

        }

        public ActionResult TransactionView()
        {
            int UserDetailsId = UserIdSession();
            //User Balance
            //Get Current User Data
            UserDetails UserDetails = GetUserDetails();

            ViewBag.Balance = UserDetails.Balance;
            ViewBag.Username = UserDetails.Name;

            response = GlobalVariables.WebApiClient.GetAsync($"{TransactionsController}/GetUserTransactionList/{UserDetailsId}").Result;
            IEnumerable<Transactions> transactionList = response.Content.ReadAsAsync<IEnumerable<Transactions>>().Result;
            return View(transactionList);
        }
        public ActionResult Logout()
        {
            int UserDetailsId = UserIdSession();
            var RemoveUnconfirmedOrders = db.UserCarts.Where(d => d.ConfirmCart == false && d.UserDetailsId == UserDetailsId).ToList();
            db.UserCarts.RemoveRange(RemoveUnconfirmedOrders);

            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/RemoveUnconfirmedOrders/{UserDetailsId}").Result;

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
