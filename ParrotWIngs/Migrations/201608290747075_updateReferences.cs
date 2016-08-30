namespace ParrotWIngs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateReferences : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transactions", "PayeeId", c => c.String(nullable: false));
            AlterColumn("dbo.Transactions", "RecipientId", c => c.String(nullable: false));
            AlterColumn("dbo.UserAccounts", "UserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserAccounts", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Transactions", "RecipientId", c => c.Int(nullable: false));
            AlterColumn("dbo.Transactions", "PayeeId", c => c.Int(nullable: false));
        }
    }
}
