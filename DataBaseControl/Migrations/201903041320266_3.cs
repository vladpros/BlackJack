namespace BlackJack.DataBaseAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Turns", "CountPoint");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Turns", "CountPoint", c => c.Long(nullable: false));
        }
    }
}
