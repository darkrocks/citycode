namespace Guide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Published = c.Boolean(nullable: false),
                        HeaderRu = c.String(nullable: false),
                        HeaderEn = c.String(nullable: false),
                        TeaserEn = c.String(maxLength: 100),
                        TeaserRu = c.String(maxLength: 100),
                        BodyRu = c.String(),
                        BodyEn = c.String(),
                        CityId = c.Int(nullable: false),
                        StreetNameRu = c.String(),
                        StreetNameEn = c.String(),
                        BuildingNumber = c.Int(),
                        Housing = c.Int(),
                        Importance = c.Short(nullable: false),
                        ThumbnailId = c.Int(),
                        ThumbnailImageUrl = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Images", t => t.ThumbnailId)
                .Index(t => t.CityId)
                .Index(t => t.ThumbnailId);
            
            CreateTable(
                "dbo.ArticleToImages",
                c => new
                    {
                        ArticleId = c.Int(nullable: false),
                        ImageId = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        CaptionRu = c.String(maxLength: 100),
                        CaptionEn = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => new { t.ArticleId, t.ImageId })
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Images", t => t.ImageId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleToSightTypes",
                c => new
                    {
                        ArticleId = c.Int(nullable: false),
                        SightTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ArticleId, t.SightTypeId })
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.SightTypes", t => t.SightTypeId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.SightTypeId);
            
            CreateTable(
                "dbo.SightTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameRu = c.String(nullable: false, maxLength: 100),
                        NameEn = c.String(nullable: false, maxLength: 100),
                        PluralNameRu = c.String(nullable: false, maxLength: 100),
                        PluralNameEn = c.String(nullable: false, maxLength: 100),
                        Published = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameRu = c.String(maxLength: 256),
                        NameEn = c.String(),
                        ThreeLetterCode = c.String(),
                        CountryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameRu = c.String(nullable: false, maxLength: 256),
                        NameEn = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Type = c.String(),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Source = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.Articles", "ThumbnailId", "dbo.Images");
            DropForeignKey("dbo.Articles", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.ArticleToSightTypes", "SightTypeId", "dbo.SightTypes");
            DropForeignKey("dbo.ArticleToSightTypes", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.ArticleToImages", "ImageId", "dbo.Images");
            DropForeignKey("dbo.ArticleToImages", "ArticleId", "dbo.Articles");
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.Cities", new[] { "CountryId" });
            DropIndex("dbo.ArticleToSightTypes", new[] { "SightTypeId" });
            DropIndex("dbo.ArticleToSightTypes", new[] { "ArticleId" });
            DropIndex("dbo.ArticleToImages", new[] { "ImageId" });
            DropIndex("dbo.ArticleToImages", new[] { "ArticleId" });
            DropIndex("dbo.Articles", new[] { "ThumbnailId" });
            DropIndex("dbo.Articles", new[] { "CityId" });
            DropTable("dbo.Users");
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.LogMessages");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
            DropTable("dbo.SightTypes");
            DropTable("dbo.ArticleToSightTypes");
            DropTable("dbo.Images");
            DropTable("dbo.ArticleToImages");
            DropTable("dbo.Articles");
        }
    }
}
