namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticleProperties3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Coordinate1", c => c.String());
            AddColumn("dbo.Articles", "Coordinate2", c => c.String());
            DropColumn("dbo.Articles", "Coordinates");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "Coordinates", c => c.String());
            DropColumn("dbo.Articles", "Coordinate2");
            DropColumn("dbo.Articles", "Coordinate1");
        }
    }
}
