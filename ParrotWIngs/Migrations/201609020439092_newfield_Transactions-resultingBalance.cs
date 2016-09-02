namespace ParrotWIngs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newfield_TransactionsresultingBalance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "ResultingBalance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "ResultingBalance");
        }
    }
}
