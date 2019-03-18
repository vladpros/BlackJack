namespace BlackJack.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Games", "UserId", "dbo.Players");
            DropIndex("dbo.Games", new[] { "UserId" });
            DropColumn("dbo.Games", "UserId");
            DropColumn("dbo.Players", "Point");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "Point", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "UserId", c => c.Long(nullable: false));
            CreateIndex("dbo.Games", "UserId");
            AddForeignKey("dbo.Games", "UserId", "dbo.Players", "Id", cascadeDelete: true);
        }
    }
}
