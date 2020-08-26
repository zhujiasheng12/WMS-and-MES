namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JDJS_WMS_Fixture_Type_Table",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JDJS_WMS_Fixture_Type_Table");
        }
    }
}
