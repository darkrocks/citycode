namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "Size", c => c.String());
            DropColumn("dbo.Images", "Sizes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "Sizes", c => c.String());
            DropColumn("dbo.Images", "Size");
        }
    }
}
