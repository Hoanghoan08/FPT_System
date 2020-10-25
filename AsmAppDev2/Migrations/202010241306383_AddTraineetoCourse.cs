namespace AsmAppDev2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTraineetoCourse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignTraineetoCourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TraineeID = c.String(maxLength: 128),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TraineeID)
                .Index(t => t.TraineeID)
                .Index(t => t.CourseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignTraineetoCourses", "TraineeID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AssignTraineetoCourses", "CourseID", "dbo.Courses");
            DropIndex("dbo.AssignTraineetoCourses", new[] { "CourseID" });
            DropIndex("dbo.AssignTraineetoCourses", new[] { "TraineeID" });
            DropTable("dbo.AssignTraineetoCourses");
        }
    }
}
