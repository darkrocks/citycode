namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticleHeaderEn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "HeaderEn", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Articles", "HeaderEn", c => c.String(nullable: false));
        }
    }
}
