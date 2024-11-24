using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_Workspaces.Models
{
    public class Estado
    {
        [Key]
        public int EstadoID { get; set; }

        [Required]
        public string EstadoNombre { get; set; }
    }
}