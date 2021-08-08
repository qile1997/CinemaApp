using CinemaApp.Persistance.Repository;
using System;
using System.Net.Http;

namespace CinemaApp.Admin
{
    class Program
    {
        static void Main(string[] args)
        {
            TableRepository tableRepo = new TableRepository();
            const string Controller = "Admin";
            HttpResponseMessage response;
            while (true)
            {
                Console.WriteLine("Admin/ Program Initialization");
                Console.WriteLine("1. Clear All Data");
                Console.WriteLine("0. Initialize data");
                Console.WriteLine("0a. Users");
                Console.WriteLine("0b. Movies");
                Console.WriteLine("0c. Halls");
                Console.WriteLine("0d. Movie Halls");
                Console.WriteLine("0e. Movie Hall Details");
                Console.WriteLine("0f. Empty User Cart");
                Console.WriteLine("2. Print Data");
                Console.WriteLine("2a. Print all users");
                Console.WriteLine("2b. Print all movies");
                Console.WriteLine("2c. Print all halls");
                Console.WriteLine("2d. Print all movie halls");
                Console.WriteLine("2e. Print all movie hall details");

                string opt = Console.ReadLine();

                switch (opt)
                {
                    case "1":
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/ClearAllData").Result;
                        break;
                    case "0":
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GenerateUserDetails").Result;
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GenerateMovies").Result;
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GenerateHalls").Result;
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GenerateMovieHalls").Result;
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GenerateMovieHallDetails").Result;
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/ClearUserCart").Result;
                        break;
                    case "0a":
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GenerateUserDetails").Result;
                        break;
                    case "0b":
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GenerateMovies").Result;
                        break;
                    case "0c":
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GenerateHalls").Result;
                        break;
                    case "0d":
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GenerateMovieHalls").Result;
                        break;
                    case "0e":
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/GenerateMovieHallDetails").Result;
                        break;
                    case "0f":
                        response = GlobalVariables.WebApiClient.GetAsync($"{Controller}/ClearUserCart").Result;
                        break;
                    case "2":
                        tableRepo.PrintUserTable();
                        tableRepo.PrintMoviesTable();
                        tableRepo.PrintHallsTable();
                        tableRepo.PrintMovieHallsTable();
                        tableRepo.PrintMovieHallDetailsTable();
                        break;
                    case "2a":
                        tableRepo.PrintUserTable();
                        break;
                    case "2b":
                        tableRepo.PrintMoviesTable();
                        break;
                    case "2c":
                        tableRepo.PrintHallsTable();
                        break;
                    case "2d":
                        tableRepo.PrintMovieHallsTable();
                        break;
                    case "2e":
                        tableRepo.PrintMovieHallDetailsTable();
                        break;
                }
            }
        }
    }
}
