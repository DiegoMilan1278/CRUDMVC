using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalMvc.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Venta = new HashSet<Venta>();
        }

        [Key]
        [Required]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "CedulaCliente")]
        public int CedulaCliente { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "NombreCliente")]
        public string NombreCliente { get; set; } = null!;

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "ApellidoCliente")]
        public string ApellidoCliente { get; set; } = null!;

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "DireccionCliente")]
        public string DireccionCliente { get; set; } = null!;

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "TelefonoCliente")]
        public string TelefonoCliente { get; set; } = null!;

        public virtual ICollection<Venta> Venta { get; set; }
    }
}
