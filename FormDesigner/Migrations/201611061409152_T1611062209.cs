namespace FormDesigner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class T1611062209 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormInfoEntity", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FormInfoEntity", "Content");
        }
    }
}
