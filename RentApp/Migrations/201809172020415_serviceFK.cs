namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class serviceFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rents", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "Service_Id", "dbo.Services");
            DropIndex("dbo.Rents", new[] { "Vehicle_Id" });
            DropIndex("dbo.Vehicles", new[] { "Service_Id" });
            AddColumn("dbo.Vehicles", "Available", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Rents", "Vehicle_Id", c => c.Int());
            AlterColumn("dbo.Vehicles", "Service_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Rents", "Vehicle_Id");
            CreateIndex("dbo.Vehicles", "Service_Id");
            AddForeignKey("dbo.Rents", "Vehicle_Id", "dbo.Vehicles", "Id");
            AddForeignKey("dbo.Vehicles", "Service_Id", "dbo.Services", "Id", cascadeDelete: true);
            DropColumn("dbo.Vehicles", "Aveable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Aveable", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Vehicles", "Service_Id", "dbo.Services");
            DropForeignKey("dbo.Rents", "Vehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.Vehicles", new[] { "Service_Id" });
            DropIndex("dbo.Rents", new[] { "Vehicle_Id" });
            AlterColumn("dbo.Vehicles", "Service_Id", c => c.Int());
            AlterColumn("dbo.Rents", "Vehicle_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Vehicles", "Available");
            CreateIndex("dbo.Vehicles", "Service_Id");
            CreateIndex("dbo.Rents", "Vehicle_Id");
            AddForeignKey("dbo.Vehicles", "Service_Id", "dbo.Services", "Id");
            AddForeignKey("dbo.Rents", "Vehicle_Id", "dbo.Vehicles", "Id", cascadeDelete: true);
        }
    }
}
