namespace BlackJackDataBaseAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Turns", "CountPoint", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Turns", "CountPoint");
        }
    }
}
