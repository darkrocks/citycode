namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticleProperties2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Coordinates", c => c.String());
            DropColumn("dbo.Articles", "Lat");
            DropColumn("dbo.Articles", "Lng");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "Lng", c => c.String());
            AddColumn("dbo.Articles", "Lat", c => c.String());
            DropColumn("dbo.Articles", "Coordinates");
        }
    }
}
