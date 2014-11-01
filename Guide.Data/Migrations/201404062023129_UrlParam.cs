namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UrlParam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "UrlParam", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "UrlParam");
        }
    }
}
