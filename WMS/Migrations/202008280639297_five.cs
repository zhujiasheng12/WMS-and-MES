namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class five : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JDJS_WMS_Fixture_Temporary_Table", "StockNum", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JDJS_WMS_Fixture_Temporary_Table", "StockNum");
        }
    }
}
