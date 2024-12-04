namespace Proyecto_Workspaces.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Proyecto_Workspaces.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Proyecto_Workspaces.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.Estados.AddOrUpdate(
                e => e.EstadoID,
                new Models.Estado { EstadoID = 1, EstadoNombre = "Aprobada para su uso" },
                new Models.Estado { EstadoID = 2, EstadoNombre = "Denegada para el uso" },
                new Models.Estado { EstadoID = 3, EstadoNombre = "Aprobada con modificaciones para su uso" }

                );
        }
    }
}
