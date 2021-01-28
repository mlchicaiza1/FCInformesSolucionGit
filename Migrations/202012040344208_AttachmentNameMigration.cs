namespace FCInformesSolucion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttachmentNameMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Attachment1Name", c => c.String(maxLength: 250));
            DropColumn("dbo.Requests", "Attachment1Path");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "Attachment1Path", c => c.String(maxLength: 250));
            DropColumn("dbo.Requests", "Attachment1Name");
        }
    }
}
