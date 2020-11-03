using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace WebCRUDapi.Models
{
    public class ServiciosCLS
    {
        public int IdServicio { get; set; }
        public int IdTipoServicio { get; set; }
        public string Nombre { get; set; }
        public SqlMoney CostoBase { get; set; }

    }
}