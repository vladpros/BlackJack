namespace DataBaseControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Turns", "CardLear", c => c.Int(nullable: false));
            AddColumn("dbo.Turns", "CardNumber", c => c.Int(nullable: false));
            DropColumn("dbo.Turns", "LearCard");
            DropColumn("dbo.Turns", "NumberCard");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Turns", "NumberCard", c => c.Int(nullable: false));
            AddColumn("dbo.Turns", "LearCard", c => c.Int(nullable: false));
            DropColumn("dbo.Turns", "CardNumber");
            DropColumn("dbo.Turns", "CardLear");
        }
    }
}
