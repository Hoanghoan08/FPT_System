namespace AsmAppDev2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTraineeUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TraineeUsers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TraineeID = c.String(nullable: false, maxLength: 128),
                        Full_Name = c.String(),
                        Email = c.String(),
                        Education = c.String(),
                        Programming_Language = c.String(),
                        Experience_Details = c.String(),
                        Department = c.String(),
                        Phone = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.TraineeID, cascadeDelete: true)
                .Index(t => t.TraineeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TraineeUsers", "TraineeID", "dbo.AspNetUsers");
            DropIndex("dbo.TraineeUsers", new[] { "TraineeID" });
            DropTable("dbo.TraineeUsers");
        }
    }
}
