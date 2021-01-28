namespace FCInformesSolucion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigurationEmailMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configurations", "EmailFrom", c => c.String(maxLength: 250));
            AddColumn("dbo.Configurations", "SmtpServer", c => c.String(maxLength: 250));
            AddColumn("dbo.Configurations", "SmptUser", c => c.String(maxLength: 250));
            AddColumn("dbo.Configurations", "SmptPassword", c => c.String(maxLength: 250));
            AddColumn("dbo.Configurations", "SmptEnableSsl", c => c.Boolean(nullable: false));
            AddColumn("dbo.Configurations", "SmptPort", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Configurations", "SmptPort");
            DropColumn("dbo.Configurations", "SmptEnableSsl");
            DropColumn("dbo.Configurations", "SmptPassword");
            DropColumn("dbo.Configurations", "SmptUser");
            DropColumn("dbo.Configurations", "SmtpServer");
            DropColumn("dbo.Configurations", "EmailFrom");
        }
    }
}
