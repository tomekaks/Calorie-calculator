namespace CalorieCalculator.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingSomeProperies : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DailySums", "Date", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Settings", "Key", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Settings", "Value", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Settings", "Value", c => c.String());
            AlterColumn("dbo.Settings", "Key", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String());
            AlterColumn("dbo.DailySums", "Date", c => c.String());
        }
    }
}
