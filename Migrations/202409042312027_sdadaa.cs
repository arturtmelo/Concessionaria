namespace ConcessionariaMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdadaa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Concessionarias", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Concessionarias", "RowVersion");
        }
    }
}
