//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DetalleFactura
    {
        public int IdDetalleFactura { get; set; }
        public int Cantidad { get; set; }
    
        public virtual Factura Factura { get; set; }
        public virtual Servicio Servicio { get; set; }
    }
}
