namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class branchTook : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rents", "Branch_Id", "dbo.Branches");
            DropIndex("dbo.Rents", new[] { "Branch_Id" });
            AddColumn("dbo.Rents", "BranchTook_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "BranchReturn_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "BranchReturn_Id1", c => c.Int());
            AlterColumn("dbo.Rents", "Branch_Id", c => c.Int());
            CreateIndex("dbo.Rents", "BranchTook_Id");
            CreateIndex("dbo.Rents", "Branch_Id");
            CreateIndex("dbo.Rents", "BranchReturn_Id1");
            AddForeignKey("dbo.Rents", "BranchReturn_Id1", "dbo.Branches", "Id");
            AddForeignKey("dbo.Rents", "BranchTook_Id", "dbo.Branches", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Rents", "Branch_Id", "dbo.Branches", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rents", "Branch_Id", "dbo.Branches");
            DropForeignKey("dbo.Rents", "BranchTook_Id", "dbo.Branches");
            DropForeignKey("dbo.Rents", "BranchReturn_Id1", "dbo.Branches");
            DropIndex("dbo.Rents", new[] { "BranchReturn_Id1" });
            DropIndex("dbo.Rents", new[] { "Branch_Id" });
            DropIndex("dbo.Rents", new[] { "BranchTook_Id" });
            AlterColumn("dbo.Rents", "Branch_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Rents", "BranchReturn_Id1");
            DropColumn("dbo.Rents", "BranchReturn_Id");
            DropColumn("dbo.Rents", "BranchTook_Id");
            CreateIndex("dbo.Rents", "Branch_Id");
            AddForeignKey("dbo.Rents", "Branch_Id", "dbo.Branches", "Id", cascadeDelete: true);
        }
    }
}
