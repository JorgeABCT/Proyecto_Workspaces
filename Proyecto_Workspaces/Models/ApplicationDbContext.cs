using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Proyecto_Workspaces.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {}

        public DbSet<Salas_Reuniones> SalasReuniones { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Salas_Reuniones>()
                .HasMany(s => s.Equipos)
                .WithMany(e => e.Salas)
                .Map(cs =>
                {
                    cs.MapLeftKey("SalaID");
                    cs.MapRightKey("EquipoID");
                    cs.ToTable("Salas_Equipo");
                });

            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}