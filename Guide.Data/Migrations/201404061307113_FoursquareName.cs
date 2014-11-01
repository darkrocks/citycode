namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FoursquareName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "FoursquareName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "FoursquareName");
        }
    }
}
