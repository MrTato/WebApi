using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace WebCRUDapi.Models
{
    public class FacturasCLS
    {
        public int IdFactura { get; set; }
        public string Numero { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public int IdZonaCliente { get; set; }
        public SqlMoney Total { get; set; }

    }
}