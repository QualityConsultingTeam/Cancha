namespace Access.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jjj : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ImageField", "Center_Id", "dbo.Center");
            DropForeignKey("dbo.Service", "Center_Id", "dbo.Center");
            DropIndex("dbo.ImageField", new[] { "Center_Id" });
            DropColumn("dbo.ImageField", "IdCenter");
            DropColumn("dbo.Service", "IdCenter");
            RenameColumn(table: "dbo.ImageField", name: "Center_Id", newName: "IdCenter");
            RenameColumn(table: "dbo.Service", name: "Center_Id", newName: "IdCenter");
            RenameIndex(table: "dbo.Service", name: "IX_Center_Id", newName: "IX_IdCenter");
            AlterColumn("dbo.ImageField", "IdCenter", c => c.Int());
            CreateIndex("dbo.ImageField", "IdCenter");
            AddForeignKey("dbo.ImageField", "IdCenter", "dbo.Center", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Service", "IdCenter", "dbo.Center", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Service", "IdCenter", "dbo.Center");
            DropForeignKey("dbo.ImageField", "IdCenter", "dbo.Center");
            DropIndex("dbo.ImageField", new[] { "IdCenter" });
            AlterColumn("dbo.ImageField", "IdCenter", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Service", name: "IX_IdCenter", newName: "IX_Center_Id");
            RenameColumn(table: "dbo.Service", name: "IdCenter", newName: "Center_Id");
            RenameColumn(table: "dbo.ImageField", name: "IdCenter", newName: "Center_Id");
            AddColumn("dbo.Service", "IdCenter", c => c.Int());
            AddColumn("dbo.ImageField", "IdCenter", c => c.Int(nullable: false));
            CreateIndex("dbo.ImageField", "Center_Id");
            AddForeignKey("dbo.Service", "Center_Id", "dbo.Center", "Id");
            AddForeignKey("dbo.ImageField", "Center_Id", "dbo.Center", "Id");
        }
    }
}
