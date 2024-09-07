namespace ConcessionariaMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _321212124 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Concessionarias", "BitAtivo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Concessionarias", "BitAtivo");
        }
    }
}
