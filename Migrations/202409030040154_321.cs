namespace ConcessionariaMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _321 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fabricantes", "BitAtivo", c => c.Boolean(nullable: false));
            DropColumn("dbo.Fabricantes", "IsDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fabricantes", "IsDeleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Fabricantes", "BitAtivo");
        }
    }
}
