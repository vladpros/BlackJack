namespace DataBaseControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class q : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GamePlayers", newName: "PlayerGames");
            DropForeignKey("dbo.PlayerInTurnCards", "PlayerInTurn_Id", "dbo.PlayerInTurns");
            DropForeignKey("dbo.PlayerInTurnCards", "Card_Id", "dbo.Cards");
            DropIndex("dbo.PlayerInTurnCards", new[] { "PlayerInTurn_Id" });
            DropIndex("dbo.PlayerInTurnCards", new[] { "Card_Id" });
            DropPrimaryKey("dbo.PlayerGames");
            AddColumn("dbo.PlayerInTurns", "Card", c => c.String());
            AddPrimaryKey("dbo.PlayerGames", new[] { "Player_Id", "Game_Id" });
            DropTable("dbo.Cards");
            DropTable("dbo.PlayerInTurnCards");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PlayerInTurnCards",
                c => new
                    {
                        PlayerInTurn_Id = c.Long(nullable: false),
                        Card_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.PlayerInTurn_Id, t.Card_Id });
            
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NumberCard = c.Int(nullable: false),
                        LearCard = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropPrimaryKey("dbo.PlayerGames");
            DropColumn("dbo.PlayerInTurns", "Card");
            AddPrimaryKey("dbo.PlayerGames", new[] { "Game_Id", "Player_Id" });
            CreateIndex("dbo.PlayerInTurnCards", "Card_Id");
            CreateIndex("dbo.PlayerInTurnCards", "PlayerInTurn_Id");
            AddForeignKey("dbo.PlayerInTurnCards", "Card_Id", "dbo.Cards", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PlayerInTurnCards", "PlayerInTurn_Id", "dbo.PlayerInTurns", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.PlayerGames", newName: "GamePlayers");
        }
    }
}
