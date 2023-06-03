using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Controllers
{
    public class VentasController : Controller
    {
        private readonly TallerContext _context;

        public VentasController(TallerContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index(int option, string filter)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            else
            {
                var tallerContext = _context.Ventas.Include(v => v.Cliente);
                if (!String.IsNullOrEmpty(filter))
                    return View(await _context.Ventas.Include(v => v.Cliente)
                        .Where(x => option == 1 ? x.Cliente.CedulaCliente == Convert.ToInt32(filter) : x.IdVenta == Convert.ToInt32(filter)).ToListAsync());

                return View(await tallerContext.ToListAsync());
            }
        }

        //GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (venta == null)
            {
                return NotFound();
            }
            venta.Facturas = await _context.Facturas
                .Where(x => x.VentaId == venta.IdVenta)
                .Include(v => v.Producto)
                .ToListAsync();
            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            var cosa = new SelectList(_context.Clientes, "IdCliente", "CedulaCliente", "NombreCliente");
            ViewData["ClienteId"] = cosa;
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenta,FechaVenta,ClienteId,TotalVenta")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", venta.ClienteId);
            return View(venta);
        }

        private bool VentaExists(int id)
        {
            return (_context.Ventas?.Any(e => e.IdVenta == id)).GetValueOrDefault();
        }
    }
}

