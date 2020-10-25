namespace AsmAppDev2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrainerUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainerUsers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TrainerID = c.String(nullable: false, maxLength: 128),
                        Full_Name = c.String(),
                        Working_Place = c.String(),
                        Phone = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.TrainerID, cascadeDelete: true)
                .Index(t => t.TrainerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainerUsers", "TrainerID", "dbo.AspNetUsers");
            DropIndex("dbo.TrainerUsers", new[] { "TrainerID" });
            DropTable("dbo.TrainerUsers");
        }
    }
}
