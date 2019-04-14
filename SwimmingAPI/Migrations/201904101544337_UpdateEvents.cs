namespace SwimmingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventAge", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "EventAge");
        }
    }
}
