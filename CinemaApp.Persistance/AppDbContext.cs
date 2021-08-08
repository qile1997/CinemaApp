using CinemaApp.DomainEntity.Model;
using System.Data.Entity;

namespace CinemaApp.Persistance
{
    public class AppDbContext : DbContext
    {
        // Your context has been configured to use a 'AppDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'CinemaApp.Admin.AppDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AppDbContext' 
        // connection string in the application configuration file.
        public AppDbContext()
            : base("name=AppDbContext")
        {
        }
        public DbSet<Hall> Hall { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<MovieHall> MovieHall { get; set; }
        public DbSet<MovieHallDetails> MovieHallDetails { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<UserCart> UserCarts { get; set; }
        public DbSet<Transactions> Transactions { get; set; }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}