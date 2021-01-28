namespace FCInformesSolucion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcessStatusMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "RequestProcessStatus", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "RequestProcessStatus");
        }
    }
}
