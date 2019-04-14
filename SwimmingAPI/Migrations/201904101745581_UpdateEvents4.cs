namespace SwimmingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEvents4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "EventGender", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "EventGender", c => c.Int(nullable: false));
        }
    }
}
