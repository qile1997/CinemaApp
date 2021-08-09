namespace CinemaApp.Customer.MVCLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8821 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Halls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HallId = c.Int(nullable: false),
                        HallNo = c.String(),
                        TotalRow = c.Int(nullable: false),
                        TotalColumn = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieId = c.Int(nullable: false),
                        MovieTitle = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        MovieAvailability = c.Boolean(nullable: false),
                        TicketPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovieHalls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieHallId = c.Int(nullable: false),
                        MovieId = c.Int(nullable: false),
                        HallId = c.Int(nullable: false),
                        MovieDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovieHallDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieHallId = c.Int(nullable: false),
                        SeatStatus = c.Int(nullable: false),
                        Seat = c.String(),
                        UserDetailsId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransferMode = c.Int(nullable: false),
                        UserDetailsId = c.Int(nullable: false),
                        TransferAmount = c.String(),
                        Transaction = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        Remarks = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserCarts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketPrice = c.Int(nullable: false),
                        MovieHallsId = c.Int(nullable: false),
                        UserDetailsId = c.Int(nullable: false),
                        MovieDateTime = c.DateTime(nullable: false),
                        HallNo = c.String(),
                        Seat = c.String(),
                        MovieTitle = c.String(),
                        ConfirmCart = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Username = c.String(),
                        Password = c.String(nullable: false),
                        Balance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserDetails");
            DropTable("dbo.UserCarts");
            DropTable("dbo.Transactions");
            DropTable("dbo.MovieHallDetails");
            DropTable("dbo.MovieHalls");
            DropTable("dbo.Movies");
            DropTable("dbo.Halls");
        }
    }
}
