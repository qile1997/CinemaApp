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
        private static Data data = new Data();
        static void Main(string[] args)
        {

            data.GenerateUserData();
            data.GenerateMovieData();
            data.GenerateMovieTime();
            data.GenerateMovieSeats();

            InitializeApp();

        }
        public static void InitializeApp()
        {
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
                                string MovieInput = "";
                                if (item.MovieAvailability)
                                {
                                    MovieInput = "Now Showing";
                                }
                                else
                                {
                                    MovieInput = "Coming Soon";
                                }

                                table.AddRow(item.MovieId, item.MovieTitle, item.ReleaseDate, MovieInput);
                            }

                            table.Write();
                            Console.WriteLine("Login to buy a movie ticket of your favourite movie. ");
                            break;
                        case 2:

                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter your username: ");
                                string username = Console.ReadLine();
                                Console.WriteLine("Enter your password: ");
                                string password = Console.ReadLine();

                                var user = (from d in data.UserDetail
                                            where d.Username == username && d.Password == password
                                            select d).SingleOrDefault();

                                while (true)
                                {
                                    if (user != null)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("You login as " + user.Name);

                                        while (true)
                                        {
                                            Console.WriteLine("Type 'L' to logout ");

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

                                            string Id = Console.ReadLine();

                                            if (Id == "L")
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Logging out...");
                                                Thread.Sleep(1000);
                                                Console.Clear();
                                                InitializeApp();
                                            }
                                            else
                                            {
                                                var selectMovie = (from d in data.MovieList
                                                                   where d.MovieId == Convert.ToInt32(Id) && d.MovieAvailability == true
                                                                   select d).SingleOrDefault();

                                                if (selectMovie != null)
                                                {
                                                    Console.Clear();
                                                    while (true)
                                                    {
                                                        Console.WriteLine("Your movie selection: " + selectMovie.MovieTitle);

                                                        var table2 = new ConsoleTable("Id", "Date Start Time");

                                                        var _selectMovieTime = (from d in data.MovieTime
                                                                                where d.MovieId == selectMovie.MovieId
                                                                                select d).ToList();

                                                        foreach (var item in _selectMovieTime)
                                                        {
                                                            table2.AddRow(item.Id, item.MovieTime);
                                                        }

                                                        table2.Write();

                                                        Console.Write("Enter ID to choose the movie time : ");
                                                        Console.Write("\n Select a movie / Type 'R' to return: ");

                                                        Id = Console.ReadLine();

                                                        if (Id == "R")
                                                        {
                                                            Console.Clear();
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            var selectMovieTime = (from d in data.MovieTime
                                                                                   where d.Id == Convert.ToInt32(Id)
                                                                                   select d).SingleOrDefault();

                                                            if (selectMovieTime != null)
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" Cinema Hall Seating");
                                                                Console.WriteLine("");
                                                                Console.WriteLine(" T: Taken     E:Empty");
                                                                Console.WriteLine("");

                                                                var selectMovieSeats = (from d in data.MovieSeats
                                                                                        where d.MovieId == selectMovieTime.Id
                                                                                        select d).ToList();

                                                                foreach (var item in selectMovieSeats)
                                                                {
                                                                    Console.Write(" {0},{1} {2} ", item.SeatRow, item.SeatColumn, item.SeatStatus);
                                                                    item.Seat = item.SeatRow + "," + item.SeatColumn;

                                                                    if (item.SeatColumn == 10)
                                                                    {
                                                                        Console.WriteLine("");
                                                                    }
                                                                }

                                                                Console.WriteLine("");
                                                                while (true)
                                                                {
                                                                    Console.Write(" Enter a seat number(row , column)/ Type 'R' to return: ");
                                                                    string seatNo = Console.ReadLine();
                                                                    Console.WriteLine("");

                                                                    if (seatNo == "R")
                                                                    {
                                                                        Console.Clear();
                                                                        break;
                                                                    }
                                                                    else
                                                                    {

                                                                        var CheckMovieSeat = (from d in selectMovieSeats
                                                                                              where d.Seat == seatNo && d.SeatStatus == Status.E
                                                                                              select d).SingleOrDefault();

                                                                        if (CheckMovieSeat != null)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(user.Name + ", your ticket has been purchased. ");
                                                                            CheckMovieSeat.SeatStatus = Status.T;
                                                                            break;
                                                                        }
                                                                        else
                                                                        {
                                                                            Console.WriteLine("This seat is unavailable. Try again ");
                                                                        }
                                                                    }
                                                                }
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
                                                }
                                                else
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("ID does not exist. Try again ");
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Login failed");
                                        break;
                                    }
                                }
                            }
                        case 3:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Please pick option 1-3");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please pick option 1-3");
                }
            }
        }
    }
}
