using CinemaApp.DomainEntity.Model;
using CinemaApp.Persistance;
using CinemaApp.Persistance.Repository;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaApp.Admin
{
    class Program
    {
        static void Main(string[] args)
        {
            CinemaAdminRepository cinemaAdminRepo = new CinemaAdminRepository();
            TableRepository tableRepo = new TableRepository();

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
                        cinemaAdminRepo.ClearAllData();
                        break;
                    case "0":
                        cinemaAdminRepo.GenerateUserDetails();
                        cinemaAdminRepo.GenerateMovies();
                        cinemaAdminRepo.GenerateHalls();
                        cinemaAdminRepo.GenerateMovieHalls();
                        cinemaAdminRepo.GenerateMovieHallDetails();
                        break;
                    case "0a":
                        cinemaAdminRepo.GenerateUserDetails();
                        break;
                    case "0b":
                        cinemaAdminRepo.GenerateMovies();
                        break;
                    case "0c":
                        cinemaAdminRepo.GenerateHalls();
                        break;
                    case "0d":
                        cinemaAdminRepo.GenerateMovieHalls();
                        break;
                    case "0e":
                        cinemaAdminRepo.GenerateMovieHallDetails();
                        break;
                    case "0f":
                        cinemaAdminRepo.ClearUserCart();
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
