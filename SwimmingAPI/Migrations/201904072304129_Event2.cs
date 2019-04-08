namespace SwimmingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Event2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventResults",
                c => new
                    {
                        EventResultId = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Hours = c.Int(nullable: false),
                        Minutes = c.Int(nullable: false),
                        Seconds = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventResultId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventResults", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventResults", "EventId", "dbo.Events");
            DropIndex("dbo.EventResults", new[] { "UserId" });
            DropIndex("dbo.EventResults", new[] { "EventId" });
            DropTable("dbo.EventResults");
        }
    }
}
