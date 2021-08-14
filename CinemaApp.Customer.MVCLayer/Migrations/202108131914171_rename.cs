namespace CinemaApp.Customer.MVCLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserCarts", "MovieSeat", c => c.String());
            DropColumn("dbo.UserCarts", "Seat");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserCarts", "Seat", c => c.String());
            DropColumn("dbo.UserCarts", "MovieSeat");
        }
    }
}
