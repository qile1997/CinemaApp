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

        private const string MovieController = "Movies";

        private const string HallController = "Hall";

        private const string UserDetailsController = "UserDetails";

        private const string TransactionsController = "Transactions";

        private const string MovieHallDetailsController = "MovieHallDetails";

        private const string MovieHallController = "MovieHall";

        private const string UserCartController = "UserCart";

        private UserDetails _userDetails { get; set; }
        private Hall _hallData { get; set; }
        private Movie _movieData { get; set; }
        private MovieHall _movieHallData { get; set; }
        private int SessionId { get; set; }
        private int MovieId { get; set; }
        private int MovieHallId { get; set; }

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
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/{UserIdSession()}").Result;
            return response.Content.ReadAsAsync<UserDetails>().Result;
        }
        //Get Movie data
        public Movie GetMovieData()
        {
            response = GlobalVariables.WebApiClient.GetAsync($"{MovieController}/{MovieSessionId()}").Result;
            return response.Content.ReadAsAsync<Movie>().Result;
        }
        //Get Hall data
        public Hall GetHallData()
        {
            response = GlobalVariables.WebApiClient.GetAsync($"{MovieController}/GetHallData/{MovieHallIdSession()}").Result;
            return response.Content.ReadAsAsync<Hall>().Result;
        }
        //Get MovieHall Data
        public MovieHall GetMovieHallData()
        {
            response = GlobalVariables.WebApiClient.GetAsync($"{MovieController}/GetMovieHallData/{MovieHallIdSession()}").Result;
            return response.Content.ReadAsAsync<MovieHall>().Result;
        }
        //Get Ticket Price From Cart
        public double GetTicketPrice()
        {
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/TicketTotal/{UserIdSession()}").Result;
            return response.Content.ReadAsAsync<double>().Result;
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
            response = GlobalVariables.WebApiClient.GetAsync(MovieController).Result;
            IEnumerable<Movie> movieList = response.Content.ReadAsAsync<IEnumerable<Movie>>().Result;
            return View(movieList);
        }

        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage(UserDetails userDetails)
        {
            response = GlobalVariables.WebApiClient.PostAsJsonAsync($"{UserDetailsController}/LoginCheck", userDetails).Result;

            _userDetails = response.Content.ReadAsAsync<UserDetails>().Result;

            if (_userDetails == null)
            {
                ViewBag.FailMessage = "Username / Password invalid";
                return View();
            }

            Session["UserId"] = _userDetails.Id;
            return RedirectToAction("MovieList");
        }

        public ActionResult MovieList()
        {
            response = GlobalVariables.WebApiClient.GetAsync($"{MovieController}/GetAvailableMovies").Result;
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
            MovieId = MovieSessionId();

            response = GlobalVariables.WebApiClient.GetAsync($"{MovieController}/GetSelectedMovies/{MovieId}").Result;
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
            SessionId = UserIdSession();
            MovieHallId = MovieHallIdSession();

            //5 minutes for payment
            Session["PaymentId"] = SessionId;
            Session.Timeout = 5;

            response = GlobalVariables.WebApiClient.GetAsync($"{MovieController}/GetMovieSeats/{MovieHallId}").Result;
            IEnumerable<MovieHallDetails> movieSeats = response.Content.ReadAsAsync<IEnumerable<MovieHallDetails>>().Result;

            //Get Current Hall Data
            _hallData = GetHallData();

            //Get Movie Data
            _movieData = GetMovieData();

            //Get Current Movie Hall Data
            _movieHallData = GetMovieHallData();

            //Get Current User Data
            _userDetails = GetUserDetails();

            //Ticket Total Price
            double TicketTotal = GetTicketPrice();

            ViewBag.CurrentBalance = _userDetails.Balance;
            ViewBag.RemainingBalance = _userDetails.Balance - TicketTotal;
            Session["BalanceCount"] = _userDetails.Balance - TicketTotal;
            ViewBag.Username = _userDetails.Name;
            ViewBag.HallNo = _hallData.HallNo;
            ViewBag.MovieId = _movieData.MovieId;
            ViewBag.MovieTitle = _movieData.MovieTitle;
            ViewBag.MovieDateTime = _movieHallData.MovieDateTime.ToString("h:mm tt");
            ViewBag.TicketPrice = _movieData.TicketPrice;
            ViewBag.UserDetailsId = SessionId;
            return View(movieSeats);
        }

        //Ajax
        [HttpPost]
        public ActionResult UpdateCart(string MovieSeat)
        {
            AppDbContext db = new AppDbContext();

            SessionId = UserIdSession();
            MovieId = MovieSessionId();
            MovieHallId = MovieHallIdSession();

            //Replacing Empty / Taken Seat (Get current movie hall and seat)
            var MovieHallSeat = db.MovieHallDetails.Where(d => d.MovieSeat == MovieSeat && d.MovieHallId == MovieHallId).SingleOrDefault();

            //Get Movie Data
            _movieData = GetMovieData();

            //Get Current Hall Data
            _hallData = GetHallData();

            //Get Current Movie Hall Data
            _movieHallData = GetMovieHallData();

            //Get Current User Data
            _userDetails = GetUserDetails();

            //Ticket Total Price
            double TicketTotal = GetTicketPrice();

            //Calculate minimum balance needed for ticket
            if (TicketTotal + _movieData.TicketPrice > _userDetails.Balance && MovieHallSeat.SeatStatus == Status.E)
            {
                return Json(new { Button = true, Msg = _userDetails.Name + " , you need minimum RM" + (_movieData.TicketPrice - Convert.ToDouble(Session["BalanceCount"])) + " to buy seat " + "(" + MovieSeat + ")" }, JsonRequestBehavior.AllowGet);
            }

            //Update if seat not occupied
            if (MovieHallSeat.UserDetailsId == null)
            {
                //Update seat in movie hall
                MovieHallDetails mhd = new MovieHallDetails
                {
                    Id = MovieHallSeat.Id,
                    MovieHallId = MovieHallId,
                    MovieSeat = MovieSeat,
                    SeatStatus = Status.A, //Add to cart (A)
                    UserDetailsId = SessionId
                };

                response = GlobalVariables.WebApiClient.PutAsJsonAsync($"{MovieHallDetailsController}/{MovieHallSeat.Id}", mhd).Result;

                //Update user cart
                UserCart userCart = new UserCart()
                {
                    UserDetailsId = SessionId,
                    MovieHallsId = MovieHallId,
                    MovieTitle = _movieData.MovieTitle,
                    MovieDateTime = _movieHallData.MovieDateTime,
                    HallNo = _hallData.HallNo,
                    TicketPrice = _movieData.TicketPrice,
                    Seat = MovieSeat,
                    ConfirmCart = false
                };

                response = GlobalVariables.WebApiClient.PostAsJsonAsync($"{UserCartController}", userCart).Result;
                return Json(JsonRequestBehavior.AllowGet);
            }
            else if (MovieHallSeat.UserDetailsId != null)
            {
                var removeCart = db.UserCarts.Where(d => d.Seat == MovieSeat && d.UserDetailsId == SessionId && d.MovieHallsId == MovieHallId).SingleOrDefault();
                response = GlobalVariables.WebApiClient.DeleteAsync($"{UserCartController}/{removeCart.Id}").Result;

                //Empty seat in movie hall
                MovieHallDetails mhd = new MovieHallDetails
                {
                    Id = MovieHallSeat.Id,
                    MovieHallId = MovieHallId,
                    MovieSeat = MovieSeat,
                    SeatStatus = Status.E,
                    UserDetailsId = null
                };

                response = GlobalVariables.WebApiClient.PutAsJsonAsync($"{MovieHallDetailsController}/{MovieHallSeat.Id}", mhd).Result;
                return Json(JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new InvalidOperationException("Not implemented yet");
            }
        }

        public ActionResult ClearHistory()
        {
            SessionId = UserIdSession();
            response = GlobalVariables.WebApiClient.GetAsync($"{TransactionsController}/GetUserTransactionList/{SessionId}").Result;
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

            SessionId = UserIdSession();
            int CartItemId = Convert.ToInt32(Session["RemoveCartItemId"]);
            var UserCart = db.UserCarts.Where(d => d.Id == CartItemId).SingleOrDefault();
            db.UserCarts.Remove(UserCart);
            db.SaveChanges();

            //Convert Seat from T to E
            var MovieHallDetails = db.MovieHallDetails.Where(d => d.MovieHallId == UserCart.MovieHallsId && d.UserDetailsId == UserCart.UserDetailsId && d.SeatStatus == Status.O && d.MovieSeat == UserCart.Seat).SingleOrDefault();
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
                UserDetailsId = SessionId,
                TransferMode = _trans.TransferMode
            };

            UpdateTransaction(trans);
            //Return Balance
            //Get Current User Data
            UserDetails UserDetails = GetUserDetails();

            UserDetails user = new UserDetails()
            {
                Id = SessionId,
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
            _userDetails = GetUserDetails();

            //Ticket Total Price
            double TicketTotal = GetTicketPrice();

            var OrderSummary = GetOrderSummary();

            ViewBag.Count = OrderSummary.Count();
            ViewBag.RemainingBalance = _userDetails.Balance - TicketTotal;
            ViewBag.Balance = _userDetails.Balance;
            ViewBag.Username = _userDetails.Name;
            return View(OrderSummary);
        }

        public IEnumerable<UserCart> GetOrderSummary()
        {
            SessionId = UserIdSession();

            response = GlobalVariables.WebApiClient.GetAsync($"{UserCartController}/GetUnorderedSeats/{SessionId}").Result;
            IEnumerable<UserCart> OrderSummary = response.Content.ReadAsAsync<IEnumerable<UserCart>>().Result;
            return OrderSummary;
        }

        public ActionResult OrderSummaryConfirmed()
        {
            //Get Current User Data  
            _userDetails = GetUserDetails();
            response = GlobalVariables.WebApiClient.GetAsync($"{UserCartController}/OrderSummaryConfirmedList/{UserIdSession()}").Result;
            IEnumerable<UserCart> OrderSummaryConfirmedList = response.Content.ReadAsAsync<IEnumerable<UserCart>>().Result;

            ViewBag.Balance = _userDetails.Balance;
            ViewBag.Username = _userDetails.Name;
            return View(OrderSummaryConfirmedList);
        }

        public ActionResult Payment()
        {
            //Sum of cart seats ticket price
            ViewBag.TotalAmount = GetOrderSummary().Sum(d => d.TicketPrice);
            ViewBag.Balance = GetUserDetails().Balance;
            return View();
        }

        [HttpPost]
        public ActionResult Payment(int TransferMode, double TotalAmount)
        {
            SessionId = UserIdSession();
            response = GlobalVariables.WebApiClient.GetAsync($"{UserCartController}/ReplaceUnorderedSeats/{SessionId}").Result;

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
                UserDetailsId = SessionId,
                TransferMode = _trans.TransferMode,
                Remarks = "-"
            };
            UpdateTransaction(trans);

            //Get Current User Data
            _userDetails = GetUserDetails();

            UserDetails user = new UserDetails()
            {
                Id = SessionId,
                Name = _userDetails.Name,
                Username = _userDetails.Username,
                Password = _userDetails.Password,
                Balance = _userDetails.Balance - TotalAmount
            };

            UpdateUserDetails(user);
            return RedirectToAction("OrderSummaryConfirmed");

        }

        public ActionResult TransactionView()
        {
            //Get Current User Data
            _userDetails = GetUserDetails();

            ViewBag.Balance = _userDetails.Balance;
            ViewBag.Username = _userDetails.Name;

            response = GlobalVariables.WebApiClient.GetAsync($"{TransactionsController}/GetUserTransactionList/{UserIdSession()}").Result;
            IEnumerable<Transactions> transactionList = response.Content.ReadAsAsync<IEnumerable<Transactions>>().Result;
            return View(transactionList);
        }

        public ActionResult Logout()
        {
            db.SaveChanges();
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/RemoveUnconfirmedOrders/{UserIdSession()}").Result;

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
