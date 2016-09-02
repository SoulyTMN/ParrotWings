namespace ParrotWIngs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new_fields_Transactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "ResultingPayeeBalance", c => c.Double(nullable: false));
            AddColumn("dbo.Transactions", "ResultingRecipientBalance", c => c.Double(nullable: false));
            DropColumn("dbo.Transactions", "ResultingBalance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "ResultingBalance", c => c.Double(nullable: false));
            DropColumn("dbo.Transactions", "ResultingRecipientBalance");
            DropColumn("dbo.Transactions", "ResultingPayeeBalance");
        }
    }
}
