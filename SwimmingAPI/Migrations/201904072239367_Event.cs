namespace SwimmingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Event : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        EventCode = c.Int(nullable: false),
                        Round = c.String(nullable: false),
                        MeetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Meets", t => t.MeetId, cascadeDelete: true)
                .Index(t => t.MeetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "MeetId", "dbo.Meets");
            DropIndex("dbo.Events", new[] { "MeetId" });
            DropTable("dbo.Events");
        }
    }
}
