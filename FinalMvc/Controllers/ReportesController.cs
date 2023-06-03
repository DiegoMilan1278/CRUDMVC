using FinalMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalMvc.Controllers
{
    public class ReportesController : Controller
    {
        private readonly TallerContext _context;

        public ReportesController(TallerContext context)
        {
            _context = context;
        }

        //INDEX PARA MOSTRAR LOS METODOS DE REPORTES
        public IActionResult IndexR()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            else
            {
                return View();
            }
        }

        //REPORTE DE CLIENTES
        public IActionResult ReporteC()
        {
            IEnumerable<Cliente> ListClientes = _context.Clientes;
            return View(ListClientes);
        }

        //REPORTE DE PRODUCTOS
        public IActionResult ReporteP()
        {
            IEnumerable<Producto> ListProductos = _context.Productos;
            return View(ListProductos);
        }

        //REPORTE DE VENTAS
        public IActionResult ReporteV()
        {
            IEnumerable<Venta> ListVentas = _context.Ventas;
            return View(ListVentas);
        }
    }
}
