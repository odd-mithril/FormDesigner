namespace FormDesigner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class T1611061119 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormInfoEntity", "FormName", c => c.String());
            AddColumn("dbo.FormInfoEntity", "FormDesc", c => c.String());
            AddColumn("dbo.FormInstanceEntity", "FormName", c => c.String());
            AddColumn("dbo.FormInstanceEntity", "FormDesc", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FormInstanceEntity", "FormDesc");
            DropColumn("dbo.FormInstanceEntity", "FormName");
            DropColumn("dbo.FormInfoEntity", "FormDesc");
            DropColumn("dbo.FormInfoEntity", "FormName");
        }
    }
}
