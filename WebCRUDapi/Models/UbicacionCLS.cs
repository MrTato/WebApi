using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCRUDapi.Models
{
    public class UbicacionCLS
    {
        public int IdUbicacion { get; set; }
        public int IdMaestro { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    }
}