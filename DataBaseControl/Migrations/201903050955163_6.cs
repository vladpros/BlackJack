namespace DataBaseControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "PlayerId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "PlayerId");
        }
    }
}
