namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class third : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JDJS_WMS_Fixture_Temporary_Table", "InTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JDJS_WMS_Fixture_Temporary_Table", "InTime");
        }
    }
}
