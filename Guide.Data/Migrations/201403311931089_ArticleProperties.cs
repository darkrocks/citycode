namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticleProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Lat", c => c.String());
            AddColumn("dbo.Articles", "Lng", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Lng");
            DropColumn("dbo.Articles", "Lat");
        }
    }
}
