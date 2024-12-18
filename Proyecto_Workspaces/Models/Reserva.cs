using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proyecto_Workspaces.Models
{
    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public Salas_Reuniones Sala { get; set; }

        [Required]
        public Estado Estado { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        //Solo va a guardar la fecha, aunque se le pase la hora.
        //Ex. 2024-12-04 12:00:00 -> 2024-12-04

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Hora Inicio de Reservacion")]
        public TimeSpan FechaReservacion { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Hora Finalizacion de Reservacion")]
        public TimeSpan FechaFinalizacion { get; set; }
        [Required]
        public bool Modificada { get; set; } = false;
    }
}
