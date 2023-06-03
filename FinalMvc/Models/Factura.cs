using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalMvc.Models
{
    public partial class Factura
    {
        [Key]
        public int IdFactura { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "IdVenta")]
        public int VentaId { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "IdProducto")]
        public int IdProductoF { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Valor")]
        public decimal Valor { get; set; }

        public virtual Producto? Producto { get; set; } = null!;
        public virtual Venta? Venta { get; set; } = null!;
    }
}
