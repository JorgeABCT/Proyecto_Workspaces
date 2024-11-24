using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_Workspaces.Models
{
    public class Salas_Reuniones
    {
        [Key]
        public int SalasId { get; set; }

        [Required]
        public string SalasNombre { get; set; }

        [Required]
        public TimeSpan Hora_Apertura { get; set; }

        [Required]
        public TimeSpan Hora_Clausura { get; set; }

        [Required]
        public string Ubicacion { get; set; }

        [Required]
        public bool DisponibilidadEquipo { get; set; } = false;

        public virtual ICollection<Equipo> Equipos { get; set; }
    }
}