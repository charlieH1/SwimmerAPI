namespace SwimmingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEvents5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventResults", "Time", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.EventResults", "Hours");
            DropColumn("dbo.EventResults", "Minutes");
            DropColumn("dbo.EventResults", "Seconds");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventResults", "Seconds", c => c.Int(nullable: false));
            AddColumn("dbo.EventResults", "Minutes", c => c.Int(nullable: false));
            AddColumn("dbo.EventResults", "Hours", c => c.Int(nullable: false));
            DropColumn("dbo.EventResults", "Time");
        }
    }
}
