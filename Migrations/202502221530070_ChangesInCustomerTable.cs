namespace NotifyHub.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInCustomerTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.customers", "ConnectionDetail", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.users", "LastLoginDate", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.users", "LastLoginDate", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.customers", "ConnectionDetail");
        }
    }
}
