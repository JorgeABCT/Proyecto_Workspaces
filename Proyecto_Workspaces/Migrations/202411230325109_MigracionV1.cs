﻿namespace Proyecto_Workspaces.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigracionV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Equipoes",
                c => new
                    {
                        EquipoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Serie = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EquipoId);
            
            CreateTable(
                "dbo.Salas_Reuniones",
                c => new
                    {
                        SalasId = c.Int(nullable: false, identity: true),
                        SalasNombre = c.String(nullable: false),
                        Hora_Apertura = c.Time(nullable: false, precision: 7),
                        Hora_Clausura = c.Time(nullable: false, precision: 7),
                        Ubicacion = c.String(nullable: false),
                        DisponibilidadEquipo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SalasId);
            
            CreateTable(
                "dbo.Estadoes",
                c => new
                    {
                        EstadoID = c.Int(nullable: false, identity: true),
                        EstadoNombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EstadoID);
            
            CreateTable(
                "dbo.Reservas",
                c => new
                    {
                        ReservaId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        SalaId = c.Int(nullable: false),
                        EstadoId = c.Int(nullable: false),
                        FechaReservacion = c.DateTime(nullable: false),
                        FechaFinalizacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReservaId)
                .ForeignKey("dbo.Estadoes", t => t.EstadoId, cascadeDelete: true)
                .ForeignKey("dbo.Salas_Reuniones", t => t.SalaId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SalaId)
                .Index(t => t.EstadoId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        PrimerApellido = c.String(),
                        SegundoApellido = c.String(),
                        Puesto = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Salas_Equipo",
                c => new
                    {
                        SalaID = c.Int(nullable: false),
                        EquipoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SalaID, t.EquipoID })
                .ForeignKey("dbo.Salas_Reuniones", t => t.SalaID, cascadeDelete: true)
                .ForeignKey("dbo.Equipoes", t => t.EquipoID, cascadeDelete: true)
                .Index(t => t.SalaID)
                .Index(t => t.EquipoID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Reservas", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reservas", "SalaId", "dbo.Salas_Reuniones");
            DropForeignKey("dbo.Reservas", "EstadoId", "dbo.Estadoes");
            DropForeignKey("dbo.Salas_Equipo", "EquipoID", "dbo.Equipoes");
            DropForeignKey("dbo.Salas_Equipo", "SalaID", "dbo.Salas_Reuniones");
            DropIndex("dbo.Salas_Equipo", new[] { "EquipoID" });
            DropIndex("dbo.Salas_Equipo", new[] { "SalaID" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Reservas", new[] { "EstadoId" });
            DropIndex("dbo.Reservas", new[] { "SalaId" });
            DropIndex("dbo.Reservas", new[] { "UserId" });
            DropTable("dbo.Salas_Equipo");
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Reservas");
            DropTable("dbo.Estadoes");
            DropTable("dbo.Salas_Reuniones");
            DropTable("dbo.Equipoes");
        }
    }
}
