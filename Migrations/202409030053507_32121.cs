namespace ConcessionariaMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _32121 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendas", "BitAtivo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vendas", "BitAtivo");
        }
    }
}
