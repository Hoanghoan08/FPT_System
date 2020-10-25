namespace AsmAppDev2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrainertoTopic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignTrainertoTopics",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TrainerID = c.String(maxLength: 128),
                        TopicID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Topics", t => t.TopicID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TrainerID)
                .Index(t => t.TrainerID)
                .Index(t => t.TopicID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignTrainertoTopics", "TrainerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AssignTrainertoTopics", "TopicID", "dbo.Topics");
            DropIndex("dbo.AssignTrainertoTopics", new[] { "TopicID" });
            DropIndex("dbo.AssignTrainertoTopics", new[] { "TrainerID" });
            DropTable("dbo.AssignTrainertoTopics");
        }
    }
}
