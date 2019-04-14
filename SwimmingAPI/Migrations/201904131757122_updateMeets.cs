namespace SwimmingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateMeets : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Meets", "PoolLength", c => c.String(nullable: false, maxLength: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Meets", "PoolLength", c => c.Int(nullable: false));
        }
    }
}
