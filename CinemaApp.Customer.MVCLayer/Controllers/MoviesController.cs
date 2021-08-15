using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance;
using CinemaApp.Persistance.Service;
using CinemaApp.Persistance.Services;

namespace CinemaApp.Customer.MVCLayer.Controllers
{
    public class MoviesController : Controller
    {
        private MovieService _movieService = new MovieService();
        private CinemaCustomerService _cinemaCustomerService = new CinemaCustomerService();

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
        private MovieHallDetails _movieHallDetails { get; set; }
        private int _userDetailsId { get; set; }
        private int _movieId { get; set; }
        private int _movieHallId { get; set; }
        public double _ticketTotal { get; set; }

        public int UserSessionId()
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
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/{UserSessionId()}").Result;
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
            _movieId = MovieSessionId();

            response = GlobalVariables.WebApiClient.GetAsync($"{MovieController}/GetSelectedMovies/{_movieId}").Result;
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
            _userDetailsId = UserSessionId();
            _movieHallId = MovieHallIdSession();

            //5 minutes for payment
            Session["PaymentId"] = _userDetailsId;
            Session.Timeout = 5;

            response = GlobalVariables.WebApiClient.GetAsync($"{MovieController}/GetMovieSeats/{_movieHallId}").Result;
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
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/TicketTotal/{_userDetailsId}").Result;
             _ticketTotal = response.Content.ReadAsAsync<double>().Result;

            ViewBag.CurrentBalance = _userDetails.Balance;
            ViewBag.RemainingBalance = _userDetails.Balance - _ticketTotal;
            Session["BalanceCount"] = _userDetails.Balance - _ticketTotal;
            ViewBag.Username = _userDetails.Name;
            ViewBag.HallNo = _hallData.HallNo;
            ViewBag.MovieId = _movieData.MovieId;
            ViewBag.MovieTitle = _movieData.MovieTitle;
            ViewBag.MovieDateTime = _movieHallData.MovieDateTime.ToString("h:mm tt");
            ViewBag.TicketPrice = _movieData.TicketPrice;
            ViewBag.UserDetailsId = _userDetailsId;
            return View(movieSeats);
        }

        //Ajax
        [HttpPost]
        public ActionResult UpdateMovieSeat(string MovieSeat)
        {
            _userDetailsId = UserSessionId();
            _movieId = MovieSessionId();
            _movieHallId = MovieHallIdSession();

            //Replacing Empty / Taken Seat Status (Get current movie hall and seat)
            response = GlobalVariables.WebApiClient.GetAsync($"{MovieHallDetailsController}/GetMovieHallSeat/{MovieSeat}/{_movieHallId}").Result;
            var movieHallSeat = response.Content.ReadAsAsync<MovieHallDetails>().Result;

            //Get Movie Data
            _movieData = GetMovieData();

            //Get Current Hall Data
            _hallData = GetHallData();

            //Get Current Movie Hall Data
            _movieHallData = GetMovieHallData();

            //Get Current User Data
            _userDetails = GetUserDetails();

            //Ticket Total Price
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/TicketTotal/{_userDetailsId}").Result;
            _ticketTotal = response.Content.ReadAsAsync<double>().Result;

            //Calculate minimum balance needed for ticket
            if (_ticketTotal + _movieData.TicketPrice > _userDetails.Balance && movieHallSeat.SeatStatus == Status.E)
            {
                return Json(new { Button = true, Msg = _userDetails.Name + " , you need minimum RM" + (_movieData.TicketPrice - Convert.ToDouble(Session["BalanceCount"])) + " to buy seat " + "(" + MovieSeat + ")" }, JsonRequestBehavior.AllowGet);
            }

            //Update if seat not occupied
            if (movieHallSeat.UserDetailsId == null)
            {
                //Update seat in movie hall
                MovieHallDetails movieHallDetails = new MovieHallDetails
                {
                    Id = movieHallSeat.Id,
                    MovieHallId = _movieHallId,
                    MovieSeat = MovieSeat,
                    SeatStatus = Status.A, //Add to cart (A)
                    UserDetailsId = _userDetailsId
                };

                response = GlobalVariables.WebApiClient.PutAsJsonAsync($"{MovieHallDetailsController}/{movieHallSeat.Id}", movieHallDetails).Result;

                //Update user cart
                UserCart userCart = new UserCart()
                {
                    UserDetailsId = _userDetailsId,
                    MovieHallsId = _movieHallId,
                    MovieTitle = _movieData.MovieTitle,
                    MovieDateTime = _movieHallData.MovieDateTime,
                    HallNo = _hallData.HallNo,
                    TicketPrice = _movieData.TicketPrice,
                    MovieSeat = MovieSeat,
                    ConfirmCart = false
                };

                response = GlobalVariables.WebApiClient.PostAsJsonAsync($"{UserCartController}", userCart).Result;
                return Json(JsonRequestBehavior.AllowGet);
            }
            else if (movieHallSeat.UserDetailsId != null)
            {
                var Id = _movieService.EmptyUserCartId(MovieSeat, _userDetailsId, _movieHallId);

                response = GlobalVariables.WebApiClient.DeleteAsync($"{UserCartController}/{Id}").Result;

                //Empty seat in movie hall
                MovieHallDetails movieHallDetails = new MovieHallDetails
                {
                    Id = movieHallSeat.Id,
                    MovieHallId = _movieHallId,
                    MovieSeat = MovieSeat,
                    SeatStatus = Status.E,
                    UserDetailsId = null
                };

                response = GlobalVariables.WebApiClient.PutAsJsonAsync($"{MovieHallDetailsController}/{movieHallSeat.Id}", movieHallDetails).Result;
                return Json(JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new InvalidOperationException("Not implemented yet");
            }
        }

        public ActionResult ClearHistory()
        {
            _userDetailsId = UserSessionId();

            response = GlobalVariables.WebApiClient.GetAsync($"{TransactionsController}/GetUserTransactionList/{_userDetailsId}").Result;
            IEnumerable<Transactions> transactionList = response.Content.ReadAsAsync<IEnumerable<Transactions>>().Result;
            _cinemaCustomerService.RemoveAllTransaction(transactionList);
            _cinemaCustomerService.SaveChanges();

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
            _userDetailsId = UserSessionId();

            var userCart = _cinemaCustomerService.GetUserCartById(Convert.ToInt32(Session["RemoveCartItemId"]));

            _cinemaCustomerService.RemoveUserCart(userCart);
            _cinemaCustomerService.SaveChanges();

            //Convert Seat from T to E
            response = GlobalVariables.WebApiClient.GetAsync($"{MovieHallDetailsController}/GetMovieHallSeat/{userCart.MovieSeat}/{userCart.MovieHallsId}").Result;
            var movieHallSeat = response.Content.ReadAsAsync<MovieHallDetails>().Result;

            _movieService.ConvertSeatStatus(movieHallSeat);
            _movieService.SaveChanges();

            //Add Transaction
            Transactions transaction = new Transactions()
            {
                Transaction = TransactionType.Refund,
                TransactionDate = DateTime.Now,
                TransferAmount = "+" + TotalAmount,
                UserDetailsId = _userDetailsId,
            };

            transaction.TransferMode = _movieService.GetTransactionMode(TransferMode);
            transaction.Remarks = Remarks == "" ? "-" : Remarks;

            response = GlobalVariables.WebApiClient.PostAsJsonAsync($"{TransactionsController}", transaction).Result;

            //Return Balance

            //Get Current User Data
            _userDetails = GetUserDetails();

            _userDetails.Balance += Convert.ToDouble(TotalAmount);
            response = GlobalVariables.WebApiClient.PutAsJsonAsync($"{UserDetailsController}", _userDetails).Result;
            return RedirectToAction("OrderSummaryConfirmed");
        }

        public ActionResult OrderSummary()
        {
            //Get Current User Data
            _userDetailsId = UserSessionId();
            _userDetails = GetUserDetails();

            //Ticket Total Price
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/TicketTotal/{_userDetailsId}").Result;
            _ticketTotal = response.Content.ReadAsAsync<double>().Result;

            var OrderSummary = GetOrderSummary();

            ViewBag.Count = OrderSummary.Count();
            ViewBag.RemainingBalance = _userDetails.Balance - _ticketTotal;
            ViewBag.Balance = _userDetails.Balance;
            ViewBag.Username = _userDetails.Name;
            return View(OrderSummary);
        }

        public IEnumerable<UserCart> GetOrderSummary()
        {
            _userDetailsId = UserSessionId();

            response = GlobalVariables.WebApiClient.GetAsync($"{UserCartController}/GetUnorderedSeats/{_userDetailsId}").Result;
            IEnumerable<UserCart> OrderSummary = response.Content.ReadAsAsync<IEnumerable<UserCart>>().Result;
            return OrderSummary;
        }

        public ActionResult OrderSummaryConfirmed()
        {
            //Get Current User Data  
            _userDetails = GetUserDetails();
            _userDetailsId = UserSessionId();
            response = GlobalVariables.WebApiClient.GetAsync($"{UserCartController}/OrderSummaryConfirmedList/{_userDetailsId}").Result;
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
            _userDetailsId = UserSessionId();
            response = GlobalVariables.WebApiClient.GetAsync($"{UserCartController}/ReplaceUnorderedSeats/{_userDetailsId}").Result;

            response = GlobalVariables.WebApiClient.GetAsync($"{UserCartController}/GetTransferMode/{TransferMode}").Result;
            Transfer transferMode = response.Content.ReadAsAsync<Transfer>().Result;

            Transactions transaction = new Transactions()
            {
                Transaction = TransactionType.Purchase,
                TransactionDate = DateTime.Now,
                TransferAmount = "-" + TotalAmount,
                UserDetailsId = _userDetailsId,
                TransferMode = transferMode,
                Remarks = "-"
            };
            response = GlobalVariables.WebApiClient.PostAsJsonAsync($"{TransactionsController}", transaction).Result;

            //Get Current User Data
            _userDetails = GetUserDetails();

            UserDetails userDetails = new UserDetails()
            {
                Id = _userDetailsId,
                Name = _userDetails.Name,
                Username = _userDetails.Username,
                Password = _userDetails.Password,
                Balance = _userDetails.Balance - TotalAmount
            };

            response = GlobalVariables.WebApiClient.PutAsJsonAsync($"{UserDetailsController}", userDetails).Result;
            return RedirectToAction("OrderSummaryConfirmed");

        }

        public ActionResult TransactionView()
        {
            //Get Current User Data
            _userDetails = GetUserDetails();

            ViewBag.Balance = _userDetails.Balance;
            ViewBag.Username = _userDetails.Name;

            response = GlobalVariables.WebApiClient.GetAsync($"{TransactionsController}/GetUserTransactionList/{UserSessionId()}").Result;
            IEnumerable<Transactions> transactionList = response.Content.ReadAsAsync<IEnumerable<Transactions>>().Result;
            return View(transactionList);
        }

        public ActionResult Logout()
        {
            _cinemaCustomerService.SaveChanges();
            response = GlobalVariables.WebApiClient.GetAsync($"{UserDetailsController}/RemoveUnconfirmedOrders/{UserSessionId()}").Result;

            Session.Abandon();
            return RedirectToAction("Index");
        }

        public ActionResult ViewCart()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            AppDbContext db = new AppDbContext();

            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
