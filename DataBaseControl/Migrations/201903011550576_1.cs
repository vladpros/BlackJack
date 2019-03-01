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
                        TurnNumber = c.Long(nullable: false),
                        GameStatus = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Players", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Turns",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LearCard = c.Int(nullable: false),
                        NumberCard = c.Int(nullable: false),
                        Game_Id = c.Long(),
                        Player_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .ForeignKey("dbo.Players", t => t.Player_Id)
                .Index(t => t.Game_Id)
                .Index(t => t.Player_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Turns", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.Games", "UserId", "dbo.Players");
            DropForeignKey("dbo.Turns", "Game_Id", "dbo.Games");
            DropIndex("dbo.Turns", new[] { "Player_Id" });
            DropIndex("dbo.Turns", new[] { "Game_Id" });
            DropIndex("dbo.Games", new[] { "UserId" });
            DropTable("dbo.Players");
            DropTable("dbo.Turns");
            DropTable("dbo.Games");
        }
    }
}
