namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documentUpload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rents", "StartDate", c => c.DateTime());
            AddColumn("dbo.Rents", "EndDate", c => c.DateTime());
            AddColumn("dbo.Rents", "Image", c => c.String());
            DropColumn("dbo.Rents", "Start");
            DropColumn("dbo.Rents", "End");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rents", "End", c => c.DateTime());
            AddColumn("dbo.Rents", "Start", c => c.DateTime());
            DropColumn("dbo.Rents", "Image");
            DropColumn("dbo.Rents", "EndDate");
            DropColumn("dbo.Rents", "StartDate");
        }
    }
}
