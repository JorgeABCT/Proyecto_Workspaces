using Microsoft.AspNet.Identity;
using Proyecto_Workspaces.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Proyecto_Workspaces.Controllers
{
    public class ReservaController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();


        //LIST
        //Index = Para ver las salas disponibles, para poder hacer una reserva 
        public ActionResult Index()
        {
            //Solo pasan los que estan activos
            var reserva = context.SalasReuniones.Where(x => x.Activo == true).ToList(); 
            return View(reserva);
        }

        //LIST
        //Ver reservas Admin y usuario
        public ActionResult VerReservas()
        {
            var idUsuario = User.Identity.GetUserId();
            if (User.Identity.IsAuthenticated)
            {
                //Si el usuario es admin
                if (User.IsInRole("Administrador"))
                {
                    //Listado de todas las reservas realizadas
                    var reserva = context.Reservas.ToList();
                    return View(reserva);
                }
                //Reservas de Solo el Usuario
                else if (User.IsInRole("Usuario"))
                {
                    //Solo muestra las del usuario / Pasadas Futuras
                    var reserva = context.Reservas.Where(x => x.User.Id == idUsuario).ToList();
                    return View(reserva);
                }
                else
                {
                    //Error - El usuario no esta ligado a ningun rol de admin o usuario
                    return View();
                }
            }
            else {
                //Error - Que lo mande al inicio de sesion
                return View();
            }
            
            
        }

        [HttpGet]
        public ActionResult Reservar(int SalasId)
        {
            //Esta obteniendo la sala por el id de la sala 
            var sala = context.SalasReuniones.FirstOrDefault(s => s.SalasId == SalasId);

            //Un objeto para meter toda la infomacion de la sala dentro del modelo de Reserva 
            var modeloReserva = new Reserva
            {
                Sala = sala
            };

            //Obtener las reservas ligadas a la sala especifica
            var reservasSala = context.Reservas.Where(r => r.Sala.SalasId == SalasId).ToList();

            //Manda todas las Reservas de la sala, a la vista 
            ViewBag.ReservasSala = reservasSala;

            //Retorna el objeto de Reserva 
            return View(modeloReserva);

        }


        [HttpPost]
        public ActionResult Reservar(Reserva model)
        {
            try
            {
                //Obtenemos el id del usuario
                var idUsuario = User.Identity.GetUserId();
                //Filtramos la tabla users por medio del id 
                var usuario = context.Users.FirstOrDefault(u => u.Id == idUsuario);
                //Filtramos la tabla SalaReuniones por medio del id 
                var sala = context.SalasReuniones.FirstOrDefault(u => u.SalasId == model.Sala.SalasId);

                //Filtramos el estado por medio del id (1 = Activo)
                var estado = context.Estados.FirstOrDefault(u => u.EstadoID == 1);

                //Obj de reserva que guarde toda la informacion ingresada por el usuario
                var reserva = new Reserva
                {
                    User = usuario,
                    Sala = sala,
                    Estado = estado,
                    Fecha = model.Fecha,
                    FechaReservacion = model.FechaReservacion,
                    FechaFinalizacion = model.FechaReservacion,
                    Modificada = false
                };

                //Insert
                context.Reservas.Add(reserva);
                //Commit (Se guarde en la base de datos)
                context.SaveChanges();
                //Lo devolvemos a una pantalla (Index)
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reserva/Edit/5
        public ActionResult ModificarReserva(int id)
        {
            return View();
        }

        // POST: Reserva/Edit/5
        [HttpPost]
        public ActionResult ModificarReserva(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult CancelarReserva(int reservaId)
        {

            if (reservaId != null)
            {
                //Obj de reserva que guarde toda la informacion ingresada por el usuario
                var reserva = new Reserva
                {
                    User = User,
                    Sala = sala,
                    Estado = estado,
                    Fecha = model.Fecha,
                    FechaReservacion = model.FechaReservacion,
                    FechaFinalizacion = model.FechaReservacion,
                    Modificada = false
                };

                context.Entry(sala).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();


                return Json(new { success = true });

            }
            return Json(new { success = false, message = "No se encontró la reserva." });
        }

    }
}
