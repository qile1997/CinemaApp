using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        //public static int MovieId;
        //public static int GetId()
        //{
        //    return ++MovieId;
        //}
        private static Data data = new Data();
        static void Main(string[] args)
        {
            data.GenerateUserData();
            data.GenerateMovieData();
            data.GenerateMovieTime();
            data.GenerateMovieSeats();

            while (true)
            {
                Console.WriteLine("1. View all movies");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit app");

                Console.Write("Enter your option: ");

                bool checkoption = int.TryParse(Console.ReadLine(), out int option);

                if (checkoption)
                {
                    switch (option)
                    {
                        case 1:

                            var table = new ConsoleTable("Id", "Movie Title", "Release Date", "Movie Available");

                            foreach (var item in data.MovieList)
                            {
                                string input = "";
                                if (item.MovieAvailability)
                                {
                                    input = "Now Showing";
                                }
                                else
                                {
                                    input = "Coming Soon";
                                }

                                table.AddRow(item.MovieId, item.MovieTitle, item.ReleaseDate, input);
                            }

                            table.Write();
                            Console.WriteLine("Login to buy a movie ticket of your favourite movie. ");
                            break;
                        case 2:

                            while (true)
                            {
                                string username;
                                string password;
                                Console.Clear();
                                Console.WriteLine("Enter your username: ");
                                username = Console.ReadLine();
                                Console.WriteLine("Enter your password: ");
                                password = Console.ReadLine();

                                foreach (UserDetails user in data.UserDetail)
                                {
                                    if (username == user.Username && password == user.Password)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("You login as " + user.Name);

                                        while (true)
                                        { 
                                            var NowShowing = (from d in data.MovieList
                                                              where d.MovieAvailability == true
                                                              select d).ToList();

                                            var table1 = new ConsoleTable("Id", "Movie Title", "Release Date", "Movie Available");

                                            foreach (var item in NowShowing)
                                            {
                                                string input = "";
                                                if (item.MovieAvailability)
                                                {
                                                    input = "Now Showing";
                                                }
                                                table1.AddRow(item.MovieId, item.MovieTitle, item.ReleaseDate, input);
                                            }

                                            table1.Write();

                                            Console.Write("\n Select a movie : ");

                                            var Id = Convert.ToInt32(Console.ReadLine());

                                            var selectMovie = (from d in data.MovieList
                                                               where d.MovieId == Id && d.MovieAvailability == true
                                                               select d).SingleOrDefault();

                                            if (selectMovie != null)
                                            {
                                                Console.Clear();

                                                while (true)
                                                {
                                                    Console.WriteLine("Your movie selection: " + selectMovie.MovieTitle);

                                                    var table2 = new ConsoleTable("Id", "Date Start Time");

                                                    foreach (var item in data.MovieTime)
                                                    {
                                                        table2.AddRow(item.Id, item.MovieTime);
                                                    }

                                                    table2.Write();

                                                    Console.Write("Enter ID to choose the movie time : ");
                                                    Console.Write("\n Select a movie : ");

                                                    Id = Convert.ToInt32(Console.ReadLine());

                                                    var selectMovie1 = (from d in data.MovieTime
                                                                        where d.Id == Id
                                                                        select d).SingleOrDefault();

                                                    if (selectMovie1 != null)
                                                    {
                                                        Console.Clear();
                                                        CinemaHallSeat();
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("ID does not exist. Try again ");
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.Clear();
                                                Console.WriteLine("ID does not exist. Try again ");
                                                continue;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Login failed");
                                        break;
                                    }
                                }
                                break;
                            }
                            break;
                        case 3:
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please pick option 1-3");
                }
            }

        }
        public static void CinemaHallSeat()
        {
            List<MovieSeats> seat = new List<MovieSeats>();

            Console.WriteLine(" Cinema Hall Seating");
            Console.WriteLine("");
            Console.WriteLine(" T: Taken     E:Empty");
            Console.WriteLine("");

            foreach (var item in data.MovieSeats)
            {
                Console.Write(" {0},{1} {2}", item.SeatRow, item.SeatColumn, item.SeatStatus);
                item.Seat = item.SeatRow + "," + item.SeatColumn;

                if (item.SeatColumn == 10)
                {
                    Console.WriteLine("");
                }
            }
            Console.WriteLine("");
            while (true)
            {
                Console.Write(" Enter a seat number(row , column): ");
                string seatNo = Console.ReadLine();
                Console.WriteLine("");

                var CheckMovieSeat = (from d in data.MovieSeats
                                      where d.Seat == seatNo && d.SeatStatus == Status.E
                                      select d).SingleOrDefault();

                if (CheckMovieSeat != null)
                {
                    Console.WriteLine("Your ticket has been purchased. ");
                    CheckMovieSeat.SeatStatus = Status.T;
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.WriteLine("This seat is unavailable. Try again ");
                }
            }  
        }
    }
}
