namespace AsmAppDev2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTopic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .Index(t => t.CourseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Topics", "CourseID", "dbo.Courses");
            DropIndex("dbo.Topics", new[] { "CourseID" });
            DropTable("dbo.Topics");
        }
    }
}
