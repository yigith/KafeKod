namespace KafeKod.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UrunStokSutun : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Urunler", "StoktaYok", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Urunler", "StoktaYok");
        }
    }
}
