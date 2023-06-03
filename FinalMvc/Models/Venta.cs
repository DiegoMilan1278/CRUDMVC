using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalMvc.Models
{
    public partial class Venta
    {
        public Venta()
        {
            Facturas = new HashSet<Factura>();
        }

        [Key]
        public int IdVenta { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [DataType(DataType.Date)]
        public DateTime FechaVenta { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "IdCliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "TotalVenta")]
        public decimal TotalVenta { get; set; }

        public virtual Cliente? Cliente { get; set; } = null!;
        public virtual ICollection<Factura> Facturas { get; set; }
    }
}
