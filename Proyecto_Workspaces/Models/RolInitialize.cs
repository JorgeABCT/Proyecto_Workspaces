using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Workspaces.Models
{
    public class RolInitialize
    {
        public static void Inicializar()
        {
            var rolManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            List<string> Roles = new List<string>();
            Roles.Add("Administrador");
            Roles.Add("Usuario");

            foreach (var rol in Roles)
            {
                if (!rolManager.RoleExists(rol))
                    rolManager.Create(new IdentityRole(rol));
            }

            //Usuario por defecto 
            var adminUser = new ApplicationUser
            {
                UserName = "admin1",
                Email = "admin@workspaces.com",
                Nombre = "Admin",
                PrimerApellido = "Primer Apellido",
                SegundoApellido = "Segundo Apellido",
                PhoneNumber = "12345678",
            };
            string Contraseña = "Admin123";

            if (userManager.FindByEmail(adminUser.Email) == null)
            {
                var creacion = userManager.Create(adminUser, Contraseña);
                if (creacion.Succeeded)
                    userManager.AddToRole(adminUser.Id, "Administrador");
            }
        }
    }
}