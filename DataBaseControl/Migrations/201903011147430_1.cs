namespace DataBaseControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoundNumber = c.Long(nullable: false),
                        GameStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Point = c.Int(nullable: false),
                        PlayerType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Turns",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TurnId = c.Long(nullable: false),
                        PlayerId = c.Long(nullable: false),
                        LearCard = c.Int(nullable: false),
                        NumberCard = c.Int(nullable: false),
                        PlayerStatus = c.Int(nullable: false),
                        Round_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Players", t => t.PlayerId, cascadeDelete: true)
                .ForeignKey("dbo.Rounds", t => t.Round_Id)
                .Index(t => t.PlayerId)
                .Index(t => t.Round_Id);
            
            CreateTable(
                "dbo.Rounds",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NumberInGame = c.Long(nullable: false),
                        GameId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.PlayerGames",
                c => new
                    {
                        Player_Id = c.Long(nullable: false),
                        Game_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_Id, t.Game_Id })
                .ForeignKey("dbo.Players", t => t.Player_Id, cascadeDelete: true)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .Index(t => t.Player_Id)
                .Index(t => t.Game_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Turns", "Round_Id", "dbo.Rounds");
            DropForeignKey("dbo.Rounds", "GameId", "dbo.Games");
            DropForeignKey("dbo.Turns", "PlayerId", "dbo.Players");
            DropForeignKey("dbo.PlayerGames", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.PlayerGames", "Player_Id", "dbo.Players");
            DropIndex("dbo.PlayerGames", new[] { "Game_Id" });
            DropIndex("dbo.PlayerGames", new[] { "Player_Id" });
            DropIndex("dbo.Rounds", new[] { "GameId" });
            DropIndex("dbo.Turns", new[] { "Round_Id" });
            DropIndex("dbo.Turns", new[] { "PlayerId" });
            DropTable("dbo.PlayerGames");
            DropTable("dbo.Rounds");
            DropTable("dbo.Turns");
            DropTable("dbo.Players");
            DropTable("dbo.Games");
        }
    }
}
