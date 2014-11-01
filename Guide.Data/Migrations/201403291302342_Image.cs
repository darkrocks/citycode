namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Image : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Images", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Images", "FileName", c => c.String(nullable: false));
        }
    }
}
