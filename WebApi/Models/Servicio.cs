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
    
    public partial class Servicio
    {
        public int IdServicio { get; set; }
        public int IdTipoServicio { get; set; }
        public string Nombre { get; set; }
        public Nullable<decimal> CostoBase { get; set; }
    }
}
