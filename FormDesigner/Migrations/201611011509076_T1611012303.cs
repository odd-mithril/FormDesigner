namespace FormDesigner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class T1611012303 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FormInfoEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ContentParse = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.FormInfo");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FormInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormName = c.String(),
                        FormDesc = c.String(),
                        Content = c.String(),
                        ContentParse = c.String(),
                        ContentData = c.String(),
                        Data = c.String(),
                        Fields = c.Int(nullable: false),
                        AddFields = c.String(),
                        IsDel = c.Short(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        DateLine = c.DateTime(nullable: false),
                        Action = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.FormInfoEntity");
        }
    }
}
