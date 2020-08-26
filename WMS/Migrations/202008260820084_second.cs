namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JDJS_WMS_Fixture_Temporary_Table",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        FixtureOrderNum = c.String(unicode: false),
                        FixtureSpecification = c.String(unicode: false),
                        Remark = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JDJS_WMS_Fixture_Temporary_Table");
        }
    }
}
