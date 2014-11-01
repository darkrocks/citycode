using Guide.Model.Entities;

namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "ImageType", c => c.Short(nullable: false, defaultValue: (short)ImageTypes.Article));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "ImageType");
        }
    }
}
