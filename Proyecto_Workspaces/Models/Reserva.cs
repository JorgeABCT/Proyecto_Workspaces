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
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int SalaId { get; set; }
        [ForeignKey("SalaId")]
        public Salas_Reuniones Sala { get; set; }

        [Required]
        public int EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public Estado Estado { get; set; }

        public DateTime FechaReservacion { get; set; }
        public DateTime FechaFinalizacion { get; set; }
    }
}
