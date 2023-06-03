using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalMvc.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Facturas = new HashSet<Factura>();
        }

        [Key]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "CodigoProducto")]
        public int CodigoProducto { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "NombreProducto")]
        public string NombreProducto { get; set; } = null!;

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "PrecioProducto")]
        public int PrecioProducto { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "CantidadProducto")]
        public int CantidadProducto { get; set; }

        public virtual ICollection<Factura> Facturas { get; set; }
    }
}
