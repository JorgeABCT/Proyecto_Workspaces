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
    public class SalasController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            var salas = context.SalasReuniones.ToList();
            return View(salas);
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Salas_Reuniones sala)
        {
            if (ModelState.IsValid)
            {
                context.SalasReuniones.Add(sala);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sala);
        }

        public ActionResult Editar(int? id)
        {
            if(id ==  null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var sala = context.SalasReuniones.FirstOrDefault(x => x.SalasId == id);
            if (sala == null) return HttpNotFound();
            return View(sala);
        }

        [HttpPost]
        public ActionResult Editar(Salas_Reuniones sala)
        {
            if (ModelState.IsValid)
            {
                context.Entry(sala).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sala);
        }

        public ActionResult Detalle(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var sala = context.SalasReuniones.FirstOrDefault(x => x.SalasId == id);
            if (sala == null) return HttpNotFound();
            return View(sala);
        }
        
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salas_Reuniones sala = context.SalasReuniones.Find(id);
            if (sala == null)
            {
                return HttpNotFound();
            }
            if(sala.Activo) sala.Activo = false;
            else sala.Activo = true;
            context.Entry(sala).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}