namespace ConcessionariaMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdada : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.CepInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CepInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cep = c.String(nullable: false),
                        Logradouro = c.String(nullable: false),
                        Cidade = c.String(nullable: false),
                        Estado = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
