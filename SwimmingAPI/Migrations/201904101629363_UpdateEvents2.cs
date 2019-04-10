namespace SwimmingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEvents2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "EventCode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "EventCode", c => c.Int(nullable: false));
        }
    }
}
