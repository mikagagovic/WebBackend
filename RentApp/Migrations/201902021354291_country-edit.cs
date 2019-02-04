namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class countryedit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Countries", "Flag", c => c.String());
            AddColumn("dbo.Countries", "CallNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Countries", "Registration", c => c.String());
            DropColumn("dbo.Countries", "Zastava");
            DropColumn("dbo.Countries", "pozivniBroj");
            DropColumn("dbo.Countries", "registarskaOznaka");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Countries", "registarskaOznaka", c => c.String());
            AddColumn("dbo.Countries", "pozivniBroj", c => c.Int(nullable: false));
            AddColumn("dbo.Countries", "Zastava", c => c.String());
            DropColumn("dbo.Countries", "Registration");
            DropColumn("dbo.Countries", "CallNumber");
            DropColumn("dbo.Countries", "Flag");
        }
    }
}
