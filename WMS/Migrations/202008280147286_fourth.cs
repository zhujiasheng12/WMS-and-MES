namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fourth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JDJS_WMS_Fixture_System_Table",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeId = c.Int(),
                        FXNum = c.Int(),
                        FixtureOrderNum = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        VenderName = c.String(unicode: false),
                        SerialCode = c.String(unicode: false),
                        Desc = c.String(unicode: false),
                        FileName = c.String(unicode: false),
                        Remark = c.String(unicode: false),
                        StockCurrNum = c.Int(),
                        StockAllNum = c.Int(),
                        CreateTime = c.DateTime(),
                        AlterTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JDJS_WMS_Fixture_System_Table");
        }
    }
}
