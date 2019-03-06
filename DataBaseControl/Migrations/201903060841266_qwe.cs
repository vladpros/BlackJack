namespace BlackJackDataBaseAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qwe : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GameWinners", newName: "GameResults");
            AlterColumn("dbo.Players", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Players", "Name", c => c.String(nullable: false));
            RenameTable(name: "dbo.GameResults", newName: "GameWinners");
        }
    }
}
