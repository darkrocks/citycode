namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Litera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Litera", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Litera");
        }
    }
}
