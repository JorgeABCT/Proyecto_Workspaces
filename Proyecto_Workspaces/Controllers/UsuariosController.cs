using Microsoft.AspNet.Identity.EntityFramework;
using Proyecto_Workspaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Workspaces.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationUsers
        public ActionResult Index()
        {
            var usuarios = db.Users.ToList();
            return View(usuarios);
        }

        // GET: ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var roles = applicationUser.Roles.Select(r => r.RoleId).ToList();
            ViewBag.RolUser = db.Roles.Where(r => roles.Contains(r.Id)).FirstOrDefault().Name;
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: ApplicationUsers/Delete/5
        public ActionResult Delete(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CambiarRol(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Roles = new SelectList(db.Roles, "Id", "Name");
            return View(user);
        }

        [HttpPost]
        public ActionResult CambiarRol(string Id, string ID_Roles)
        {
            ApplicationUser user = db.Users.Find(Id);
            var role = db.Roles.Find(ID_Roles);
            user.Roles.Clear();
            user.Roles.Add(new IdentityUserRole { RoleId = role.Id, UserId = user.Id });
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}