namespace ParrotWIngs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FK_Transactions_ApplicationUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transactions", "PayeeId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Transactions", "RecipientId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.UserAccounts", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Transactions", "PayeeId");
            CreateIndex("dbo.Transactions", "RecipientId");
            CreateIndex("dbo.UserAccounts", "UserId");
            AddForeignKey("dbo.Transactions", "PayeeId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Transactions", "RecipientId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserAccounts", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAccounts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transactions", "RecipientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transactions", "PayeeId", "dbo.AspNetUsers");
            DropIndex("dbo.UserAccounts", new[] { "UserId" });
            DropIndex("dbo.Transactions", new[] { "RecipientId" });
            DropIndex("dbo.Transactions", new[] { "PayeeId" });
            AlterColumn("dbo.UserAccounts", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Transactions", "RecipientId", c => c.String(nullable: false));
            AlterColumn("dbo.Transactions", "PayeeId", c => c.String(nullable: false));
        }
    }
}
