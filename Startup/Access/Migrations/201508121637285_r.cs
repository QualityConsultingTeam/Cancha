namespace Access.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class r : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageField",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        idCenter = c.Int(nullable: false),
                        imgUrl = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Center_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Center", t => t.Center_Id)
                .Index(t => t.Center_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImageField", "Center_Id", "dbo.Center");
            DropIndex("dbo.ImageField", new[] { "Center_Id" });
            DropTable("dbo.ImageField");
        }
    }
}
