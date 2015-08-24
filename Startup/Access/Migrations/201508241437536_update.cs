namespace Access.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CenterAccount", "CenterId", "dbo.Center");
            DropIndex("dbo.CenterAccount", new[] { "CenterId" });
            DropTable("dbo.CenterAccount");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CenterAccount",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Guid(nullable: false),
                        CenterId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UserSign = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.CenterAccount", "CenterId");
            AddForeignKey("dbo.CenterAccount", "CenterId", "dbo.Center", "Id", cascadeDelete: true);
        }
    }
}
