using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_Workspaces.Models
{
    public class Equipo
    {
        [Key]
        public int EquipoId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Serie { get; set; }

        public virtual ICollection<Salas_Reuniones> Salas { get; set; }
    }
}