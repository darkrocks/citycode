namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticleHeaderRu : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "HeaderRu", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Articles", "HeaderRu", c => c.String(nullable: false));
        }
    }
}
