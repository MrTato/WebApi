using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCRUDapi.Models
{
    public class ClientesCLS
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
    }
}