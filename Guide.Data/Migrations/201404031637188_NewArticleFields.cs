namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewArticleFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Phone", c => c.String());
            AddColumn("dbo.Articles", "Email", c => c.String());
            AddColumn("dbo.Articles", "Website", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Website");
            DropColumn("dbo.Articles", "Email");
            DropColumn("dbo.Articles", "Phone");
        }
    }
}
