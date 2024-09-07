namespace ConcessionariaMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Veiculoes", "Preco", c => c.Single(nullable: false));
            AlterColumn("dbo.Vendas", "PrecoVenda", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vendas", "PrecoVenda", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Veiculoes", "Preco", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
