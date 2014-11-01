namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticleHeadersRevert : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "HeaderRu", c => c.String(nullable: false));
            AlterColumn("dbo.Articles", "HeaderEn", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Articles", "HeaderEn", c => c.String());
            AlterColumn("dbo.Articles", "HeaderRu", c => c.String());
        }
    }
}
