namespace BlackJack.DataBaseAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameWinners",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameId = c.Long(nullable: false),
                        PlayerId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GameWinners");
        }
    }
}
