namespace SwimmingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Meets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meets",
                c => new
                    {
                        MeetId = c.Int(nullable: false, identity: true),
                        MeetName = c.String(nullable: false, maxLength: 48),
                        MeetVenue = c.String(nullable: false, maxLength: 20),
                        MeetDate = c.DateTime(nullable: false),
                        PoolLength = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MeetId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Meets");
        }
    }
}
