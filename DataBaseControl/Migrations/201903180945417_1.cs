namespace BlackJack.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameResults",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameId = c.Long(nullable: false),
                        PlayerId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameStatus = c.Int(nullable: false),
                        BotsNumber = c.Int(nullable: false),
                        PlayerId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Turns",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PlayerId = c.Long(nullable: false),
                        GameId = c.Long(nullable: false),
                        LearCard = c.Int(nullable: false),
                        NumberCard = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        PlayerType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Turns", "PlayerId", "dbo.Players");
            DropForeignKey("dbo.Turns", "GameId", "dbo.Games");
            DropIndex("dbo.Turns", new[] { "GameId" });
            DropIndex("dbo.Turns", new[] { "PlayerId" });
            DropTable("dbo.Players");
            DropTable("dbo.Turns");
            DropTable("dbo.Games");
            DropTable("dbo.GameResults");
        }
    }
}
