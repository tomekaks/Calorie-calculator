namespace CalorieCalculator.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailySums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.String(),
                        Calories = c.Double(nullable: false),
                        Proteins = c.Double(nullable: false),
                        Carbs = c.Double(nullable: false),
                        Fats = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Calories = c.Double(nullable: false),
                        Proteins = c.Double(nullable: false),
                        Carbs = c.Double(nullable: false),
                        Fats = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Settings");
            DropTable("dbo.Products");
            DropTable("dbo.DailySums");
        }
    }
}
