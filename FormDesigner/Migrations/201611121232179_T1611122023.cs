namespace FormDesigner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class T1611122023 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormInfoEntity", "Fields", c => c.String());
            AddColumn("dbo.FormInstanceEntity", "Fields", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FormInstanceEntity", "Fields");
            DropColumn("dbo.FormInfoEntity", "Fields");
        }
    }
}
