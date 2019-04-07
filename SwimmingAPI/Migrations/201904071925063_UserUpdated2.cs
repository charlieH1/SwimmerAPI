namespace SwimmingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserUpdated2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FamilyName", c => c.String());
            AddColumn("dbo.AspNetUsers", "GivenName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Club", c => c.String());
            DropColumn("dbo.AspNetUsers", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            DropColumn("dbo.AspNetUsers", "Club");
            DropColumn("dbo.AspNetUsers", "GivenName");
            DropColumn("dbo.AspNetUsers", "FamilyName");
        }
    }
}
