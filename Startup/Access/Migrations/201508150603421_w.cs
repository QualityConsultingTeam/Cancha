namespace Access.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class w : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Service", "Center_Id", c => c.Int());
            CreateIndex("dbo.Service", "Center_Id");
            AddForeignKey("dbo.Service", "Center_Id", "dbo.Center", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Service", "Center_Id", "dbo.Center");
            DropIndex("dbo.Service", new[] { "Center_Id" });
            DropColumn("dbo.Service", "Center_Id");
        }
    }
}
