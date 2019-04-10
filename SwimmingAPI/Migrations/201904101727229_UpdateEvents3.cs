namespace SwimmingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEvents3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventGender", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "EventGender");
        }
    }
}
