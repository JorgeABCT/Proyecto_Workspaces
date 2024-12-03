using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto_Workspaces.Models;

namespace Proyecto_Workspaces.Controllers
{
    public class EquiposController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var equipos = context.Equipos.ToList();
            return View(equipos);
        }

        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = context.Equipos.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "EquipoId,Nombre,Serie")] Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                context.Equipos.Add(equipo);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(equipo);
        }

        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = context.Equipos.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "EquipoId,Nombre,Serie")] Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                context.Entry(equipo).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(equipo);
        }

        [HttpGet]
        public ActionResult Agregar(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var equipo = context.Equipos.Find(id);
            if (equipo == null) return HttpNotFound();

            ViewBag.ID_Salas = new SelectList(context.SalasReuniones, "SalasId", "SalasNombre");
            return View(equipo);
        }

        [HttpPost]
        public ActionResult Agregar(int? ID_Salas, int? EquipoId)
        {
            if (ID_Salas == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var sala = context.SalasReuniones.Find(ID_Salas);
            if (sala == null) return HttpNotFound();

            if (EquipoId == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var equipo = context.Equipos.Find(EquipoId);
            if (equipo == null) return HttpNotFound();

            equipo.Salas.Add(sala);
            sala.Equipos.Add(equipo);

            sala.DisponibilidadEquipo = true;

            context.Entry(equipo).State = System.Data.Entity.EntityState.Modified;
            context.Entry(sala).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Quitar(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var equipo = context.Equipos.Find(id);
            if (equipo == null) return HttpNotFound();
            ViewBag.ID_Salas = new SelectList(equipo.Salas, "SalasId", "SalasNombre");
            return View(equipo);
        }

        [HttpPost]
        public ActionResult Quitar(int? ID_Salas, int? EquipoId)
        {
            if (ID_Salas == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var sala = context.SalasReuniones.Find(ID_Salas);
            if (sala == null) return HttpNotFound();

            if (EquipoId == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var equipo = context.Equipos.Find(EquipoId);
            if (equipo == null) return HttpNotFound();

            equipo.Salas.Remove(sala);
            sala.Equipos.Remove(equipo);

            if(sala.Equipos.Count == 0)
            {
                sala.DisponibilidadEquipo = false;
            }

            context.Entry(equipo).State = System.Data.Entity.EntityState.Modified;
            context.Entry(sala).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
