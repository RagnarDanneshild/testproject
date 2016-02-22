namespace TestProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileNotes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        Url = c.String(),
                        Type = c.String(),
                        UserId = c.String(),
                        AspNetUsers_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUsers_Id)
                .Index(t => t.AspNetUsers_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileNotes", "AspNetUsers_Id", "dbo.AspNetUsers");
            DropIndex("dbo.FileNotes", new[] { "AspNetUsers_Id" });
            DropTable("dbo.FileNotes");
        }
    }
}
