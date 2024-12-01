namespace Proyecto_Workspaces.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigracionV3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Salas_Reuniones", "Activo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Salas_Reuniones", "Activo");
        }
    }
}
