namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCoordsNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Lat", c => c.String());
            AddColumn("dbo.Articles", "Lng", c => c.String());
            DropColumn("dbo.Articles", "Coordinate1");
            DropColumn("dbo.Articles", "Coordinate2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "Coordinate2", c => c.String());
            AddColumn("dbo.Articles", "Coordinate1", c => c.String());
            DropColumn("dbo.Articles", "Lng");
            DropColumn("dbo.Articles", "Lat");
        }
    }
}
