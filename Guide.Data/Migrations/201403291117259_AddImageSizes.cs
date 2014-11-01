namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageSizes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "Sizes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "Sizes");
        }
    }
}
