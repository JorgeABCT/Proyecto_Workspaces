using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //LIST
        //Index = Para ver las salas disponibles, para poder hacer una reserva 
        public ActionResult Index()
        {
            //Solo pasan los que estan activos
            var reserva = context.SalasReuniones.Where(x => x.Activo == true).ToList(); 
            return View(reserva);
        }

        public ActionResult Detalle(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var sala = context.SalasReuniones.FirstOrDefault(x => x.SalasId == id);
            if (sala == null) return HttpNotFound();
            return View(sala);
        }

        //LIST
        //Ver reservas Admin y usuario
        [Authorize]
        public ActionResult VerReservas()
        {
            //UsuarioId
            var userId = User.Identity.GetUserId();
            var user = context.Users.Find(userId);

            var idUsuario = User.Identity.GetUserId();
            if (User.Identity.IsAuthenticated)
            {
                //Si el usuario es admin
                if (User.IsInRole("Administrador"))
                {
                    //Listado de todas las reservas realizadas
                    var reserva = context.Reservas.Include(x => x.Sala).Include(x => x.Estado).ToList();
                    return View(reserva);
                }
                //Reservas de Solo el Usuario
                else if (User.IsInRole("Usuario"))
                {
                    //Solo muestra las del usuario / Pasadas Futuras
                    var reserva = context.Reservas.Include(x => x.Sala).Include(x => x.Estado).Where(x => x.User.Id == user.Id).ToList();
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
        [Authorize]
        public ActionResult Reservar(int SalasId)
        {
            //UsuarioId
            var userId = User.Identity.GetUserId();
            var user = context.Users.Find(userId);

            //Esta obteniendo la sala por el id de la sala 
            var sala = context.SalasReuniones.FirstOrDefault(s => s.SalasId == SalasId);

            //Un objeto para meter toda la infomacion de la sala dentro del modelo de Reserva 
            var modeloReserva = new Reserva
            {
                Sala = sala,
                User = user
            };

            //Obtener las reservas ligadas a la sala especifica
            var reservasSala = context.Reservas.Include(x => x.User).Where(r => r.Sala.SalasId == SalasId).ToList();

            //Manda todas las Reservas de la sala, a la vista 
            ViewBag.ReservasSala = reservasSala;

            //Retorna el objeto de Reserva 
            return View(modeloReserva);
        }



        [HttpPost]
        [Authorize]
        public ActionResult Reservar(Reserva model)
        {
            try
            {
                // Obtén el id del usuario
                var idUsuario = User.Identity.GetUserId();
                var usuario = context.Users.FirstOrDefault(u => u.Id == idUsuario);
                model.User = usuario;

                // Obtener sala basada en el ID proporcionado
                var sala = context.SalasReuniones.FirstOrDefault(u => u.SalasId == model.Sala.SalasId);
                if (sala == null)
                {
                    ModelState.AddModelError("", "La sala seleccionada no existe.");
                    return View(model);
                }

                var estado = context.Estados.FirstOrDefault(u => u.EstadoID == 1);

                // Combina la fecha y la hora de inicio y finalización en DateTime
                DateTime fechaInicio = model.Fecha.Date + model.FechaReservacion;
                DateTime fechaFin = model.Fecha.Date + model.FechaFinalizacion;

                // Convierte las horas de apertura y clausura en DateTime
                DateTime horaApertura = model.Fecha.Date + sala.Hora_Apertura;
                DateTime horaClausura = model.Fecha.Date + sala.Hora_Clausura;

                // Validar rango de horarios permitidos
                if (fechaInicio < horaApertura || fechaInicio > horaClausura)
                {
                    ModelState.AddModelError("", $"El horario permitido para esta sala es entre {horaApertura:hh\\:mm tt} y {horaClausura:hh\\:mm tt}.");
                    return View(model);
                }

                if (fechaFin <= fechaInicio)
                {
                    ModelState.AddModelError("", "La hora de finalización debe ser posterior a la hora de inicio.");
                    return View(model);
                }

                //UsuarioId
                var userId = User.Identity.GetUserId();
                var user = context.Users.Find(userId);

                // Crea el objeto de reserva
                var reserva = new Reserva
                {
                    User = user,
                    Sala = sala,
                    Estado = estado,
                    Fecha = model.Fecha,
                    FechaReservacion = model.FechaReservacion,
                    FechaFinalizacion = model.FechaFinalizacion,
                    Modificada = false
                };

                var reservasDentroDelRango = ObtenerReservasPorRango(reserva.Fecha, reserva.FechaReservacion, reserva.FechaFinalizacion);
                var cantidadDeReservasRango = reservasDentroDelRango.Count();

                if (cantidadDeReservasRango == 0)
                {
                    // Insertar en la base de datos
                    context.Reservas.Add(reserva);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }

                context.Reservas.Add(reserva);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return View("Error"); 
            }
        }


        // GET: Reserva/Edit/5
        [Authorize(Roles="Administrador")]
        public ActionResult ModificarReserva(int? id)
        {
            if(id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var reserva = context.Reservas.FirstOrDefault(x => x.ReservaId == id);
            if (reserva == null) return HttpNotFound();
            //Retorna el objeto de Reserva 
            return View(reserva);
        }

        // POST: Reserva/Edit/5
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public ActionResult ModificarReserva(Reserva reserva)
        {
            var idReserva = reserva.ReservaId;
            var reservaEditar = context.Reservas.Find(idReserva);
            var estado = context.Estados.FirstOrDefault(u => u.EstadoID == 3);
            try
            {
                reservaEditar.Estado = estado;
                reservaEditar.Sala = reservaEditar.Sala;
                reservaEditar.Fecha = reserva.Fecha;
                reservaEditar.FechaReservacion = reserva.FechaReservacion;
                reservaEditar.FechaFinalizacion = reserva.FechaFinalizacion;
                reservaEditar.Modificada = true;
                reservaEditar.User = reservaEditar.User;
                context.Entry(reservaEditar).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("VerReservas");
            }
            catch
            {
                var reservaOriginal = context.Reservas.Find(idReserva);
                return View(reservaOriginal);
            }
        }



        // POST: Conferencias/RegistrarAsistente
        [HttpPost]
        public JsonResult CancelarReserva(int ReservaId)
        {
            var reserva = context.Reservas.Find(ReservaId);
            if (reserva.ReservaId != 0)
            {
                var idEstado = 2;
                var estado = context.Estados.Find(idEstado);
                reserva.Estado = estado;

                reserva.Modificada = true;
                reserva.Sala = reserva.Sala;
                reserva.FechaReservacion = reserva.FechaReservacion;
                reserva.FechaFinalizacion = reserva.FechaFinalizacion;
                reserva.Fecha = reserva.Fecha;  
                reserva.User = reserva.User;
                context.Entry(reserva).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return Json(new { success = true, message = "Reserva cancelada correctamente!" });
            }
            return Json(new { success = false, message = "No se encontró la reservacion." });
        }
        
        /* [HttpPost]
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
         }*/


        public List<Reserva> ObtenerReservasPorRango(DateTime fechaUsuario, TimeSpan horaInicioUsuario, TimeSpan horaFinUsuario)
        {
            var reservas = context.Reservas.Where(r =>
                r.Fecha == fechaUsuario.Date && 
                (
                    (r.FechaReservacion >= horaInicioUsuario && r.FechaReservacion < horaFinUsuario) ||
                    (r.FechaFinalizacion > horaInicioUsuario && r.FechaFinalizacion <= horaFinUsuario) ||
                    (horaInicioUsuario >= r.FechaReservacion && horaFinUsuario <= r.FechaFinalizacion) ||
                    (horaInicioUsuario <= r.FechaReservacion && horaFinUsuario >= r.FechaFinalizacion)
                )
            ).ToList();

            return reservas;
        }



    }
}
