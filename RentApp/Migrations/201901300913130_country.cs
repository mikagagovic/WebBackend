namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class country : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Zastava = c.String(),
                        pozivniBroj = c.Int(nullable: false),
                        registarskaOznaka = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Services", "Country_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Services", "Country_Id");
            AddForeignKey("dbo.Services", "Country_Id", "dbo.Countries", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "Country_Id", "dbo.Countries");
            DropIndex("dbo.Services", new[] { "Country_Id" });
            DropColumn("dbo.Services", "Country_Id");
            DropTable("dbo.Countries");
        }
    }
}
