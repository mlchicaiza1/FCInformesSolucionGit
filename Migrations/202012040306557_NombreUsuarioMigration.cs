namespace FCInformesSolucion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NombreUsuarioMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "UserName", c => c.String(maxLength: 100));
            AddColumn("dbo.Requests", "ProcessUserName", c => c.String(maxLength: 100));
            DropColumn("dbo.Requests", "UserId");
            DropColumn("dbo.Requests", "ProcessUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "ProcessUserId", c => c.String(maxLength: 36));
            AddColumn("dbo.Requests", "UserId", c => c.String(maxLength: 36));
            DropColumn("dbo.Requests", "ProcessUserName");
            DropColumn("dbo.Requests", "UserName");
        }
    }
}
