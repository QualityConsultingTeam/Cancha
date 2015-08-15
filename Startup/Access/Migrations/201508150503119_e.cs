namespace Access.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class e : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageField", "header1", c => c.String(maxLength: 50));
            AddColumn("dbo.ImageField", "header2", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageField", "header2");
            DropColumn("dbo.ImageField", "header1");
        }
    }
}
