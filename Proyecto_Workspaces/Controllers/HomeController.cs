using Proyecto_Workspaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Workspaces.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles="Administrador")]
        public ActionResult Estadisticas()
        {
            ViewBag.HorasMasComunes = db.Reservas.GroupBy(x => x.FechaReservacion).OrderByDescending(x => x.Count()).FirstOrDefault().Key;
            ViewBag.SalasMasReservadaID = db.Reservas.GroupBy(x => x.Sala.SalasId).OrderByDescending(x => x.Count()).FirstOrDefault().Key;
            ViewBag.SalasMasReservadaNombre = db.Reservas.GroupBy(x => x.Sala.SalasNombre).OrderByDescending(x => x.Count()).FirstOrDefault().Key;
            ViewBag.UsuariosMasReservaciones = db.Reservas.GroupBy(x => x.User.UserName).OrderByDescending(x => x.Count()).FirstOrDefault().Key;
            ViewBag.ReservasPorDia = db.Reservas.OrderByDescending(x => x.FechaReservacion).Take(5).ToList();
            return View();
        }
    }
}