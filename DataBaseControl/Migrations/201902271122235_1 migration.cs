namespace DataBaseControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1migration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PlayerGames", newName: "GamePlayers");
            DropForeignKey("dbo.Cards", "PlayerInTurn_Id", "dbo.PlayerInTurns");
            DropIndex("dbo.Cards", new[] { "PlayerInTurn_Id" });
            DropPrimaryKey("dbo.GamePlayers");
            CreateTable(
                "dbo.PlayerInTurnCards",
                c => new
                    {
                        PlayerInTurn_Id = c.Long(nullable: false),
                        Card_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.PlayerInTurn_Id, t.Card_Id })
                .ForeignKey("dbo.PlayerInTurns", t => t.PlayerInTurn_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cards", t => t.Card_Id, cascadeDelete: true)
                .Index(t => t.PlayerInTurn_Id)
                .Index(t => t.Card_Id);
            
            AddPrimaryKey("dbo.GamePlayers", new[] { "Game_Id", "Player_Id" });
            DropColumn("dbo.Cards", "PlayerInTurn_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cards", "PlayerInTurn_Id", c => c.Long());
            DropForeignKey("dbo.PlayerInTurnCards", "Card_Id", "dbo.Cards");
            DropForeignKey("dbo.PlayerInTurnCards", "PlayerInTurn_Id", "dbo.PlayerInTurns");
            DropIndex("dbo.PlayerInTurnCards", new[] { "Card_Id" });
            DropIndex("dbo.PlayerInTurnCards", new[] { "PlayerInTurn_Id" });
            DropPrimaryKey("dbo.GamePlayers");
            DropTable("dbo.PlayerInTurnCards");
            AddPrimaryKey("dbo.GamePlayers", new[] { "Player_Id", "Game_Id" });
            CreateIndex("dbo.Cards", "PlayerInTurn_Id");
            AddForeignKey("dbo.Cards", "PlayerInTurn_Id", "dbo.PlayerInTurns", "Id");
            RenameTable(name: "dbo.GamePlayers", newName: "PlayerGames");
        }
    }
}
