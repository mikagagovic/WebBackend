namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class incoming : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rents", "Vehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.Rents", new[] { "Vehicle_Id" });
            AddColumn("dbo.Rents", "Vehicle_Id1", c => c.Int());
            AlterColumn("dbo.Rents", "Vehicle_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Rents", "Vehicle_Id1");
            AddForeignKey("dbo.Rents", "Vehicle_Id1", "dbo.Vehicles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rents", "Vehicle_Id1", "dbo.Vehicles");
            DropIndex("dbo.Rents", new[] { "Vehicle_Id1" });
            AlterColumn("dbo.Rents", "Vehicle_Id", c => c.Int());
            DropColumn("dbo.Rents", "Vehicle_Id1");
            CreateIndex("dbo.Rents", "Vehicle_Id");
            AddForeignKey("dbo.Rents", "Vehicle_Id", "dbo.Vehicles", "Id");
        }
    }
}
