namespace FormDesigner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class T1611061109 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FormInstanceEntity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Creator = c.String(),
                        Modified = c.DateTime(nullable: false),
                        Modifier = c.String(),
                        ContentParse = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.FormInfoEntity", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.FormInfoEntity", "Creator", c => c.String());
            AddColumn("dbo.FormInfoEntity", "Modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.FormInfoEntity", "Modifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FormInfoEntity", "Modifier");
            DropColumn("dbo.FormInfoEntity", "Modified");
            DropColumn("dbo.FormInfoEntity", "Creator");
            DropColumn("dbo.FormInfoEntity", "Created");
            DropTable("dbo.FormInstanceEntity");
        }
    }
}
