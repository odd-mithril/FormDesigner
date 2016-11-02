namespace FormDesigner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class T1611021059 : DbMigration
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
        }

        public override void Down()
        {
            DropTable("dbo.FormInfoEntity");
        }
    }
}
