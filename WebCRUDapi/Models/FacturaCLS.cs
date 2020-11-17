using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace WebCRUDapi.Models
{
    public class FacturaCLS
    {
        public int IdFactura { get; set; }
        public string Numero { get; set; }
        public string Fecha { get; set; }
        public int IdZonaCliente { get; set; }
        public string Total { get; set; }

    }
}