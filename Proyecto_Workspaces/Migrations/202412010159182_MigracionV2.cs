namespace Proyecto_Workspaces.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigracionV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Salas_Reuniones", "Capacidad", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Salas_Reuniones", "Capacidad");
        }
    }
}
