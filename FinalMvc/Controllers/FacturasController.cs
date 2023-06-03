using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalMvc.Models;

namespace FinalMvc.Controllers
{
    public class FacturasController : Controller
    {
        private readonly TallerContext _context;

        public FacturasController(TallerContext context)
        {
            _context = context;
        }

        // GET: DetallesVentas
        public async Task<IActionResult> Index(string filter)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            else
            {
                var tallerContext = _context.Facturas.Include(v => v.Producto).Include(d => d.Venta).OrderBy(o => o.VentaId);
                if (!String.IsNullOrEmpty(filter))
                    return View(await _context.Facturas.Include(v => v.Producto).Include(d => d.Venta)
                        .Where(x => x.VentaId == Convert.ToInt32(filter)).OrderBy(o => o.VentaId).ToListAsync());

                return View(await tallerContext.ToListAsync());
            }
        }

        // GET: DetallesVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var detallesVenta = await _context.Facturas
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .FirstOrDefaultAsync(m => m.IdFactura == id);
            if (detallesVenta == null)
            {
                return NotFound();
            }

            return View(detallesVenta);
        }





        // GET: DetallesVentas/Create
        public IActionResult Create()
        {
            ViewData["IdProductoF"] = new SelectList(_context.Productos, "IdProducto", "NombreProducto");
            ViewData["VentaId"] = new SelectList(_context.Ventas, "IdVenta", "IdVenta");
            ViewData["Mensaje"] = "";
            return View();
        }

        // POST: DetallesVentas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFactura,VentaId,IdProductoF,Cantidad,Valor")] Factura detallesVenta)
        {
            if (ModelState.IsValid)
            {
                // Se valida que no se este registrando mas articulos de los que hay en el inventario
                var producto = await _context.Productos.FindAsync(detallesVenta.IdProductoF);
                detallesVenta.Valor = producto.PrecioProducto * detallesVenta.Cantidad;
                if (producto.CantidadProducto >= detallesVenta.Cantidad)
                {
                    // guardar el detalle
                    _context.Add(detallesVenta);
                    await _context.SaveChangesAsync();

                    // Actualizar existencias del inventario
                    producto.CantidadProducto -= detallesVenta.Cantidad;
                    _context.Productos.Update(producto);

                    // Se actualiza la venta
                    var venta = await _context.Ventas.FindAsync(detallesVenta.VentaId);
                    venta.TotalVenta = 0;
                    foreach (var item in await _context.Facturas.Where(p => p.VentaId == detallesVenta.VentaId).ToListAsync())
                    {
                        venta.TotalVenta += item.Valor;
                    }
                    _context.Ventas.Update(venta);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Mensaje"] = "La cantidad de productos es mayor a los existentes, cuentas con " + producto.CantidadProducto + "de" + producto.NombreProducto;
                }
            }
            ViewData["IdProductoF"] = new SelectList(_context.Productos, "IdProducto", "NombreProducto", detallesVenta.IdProductoF);
            ViewData["VentaId"] = new SelectList(_context.Ventas, "IdVenta", "IdVenta", detallesVenta.VentaId);
            return View(detallesVenta);
        }





        // GET: DetallesVentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var detallesVenta = await _context.Facturas.FindAsync(id);
            if (detallesVenta == null)
            {
                return NotFound();
            }
            ViewData["IdProductoF"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detallesVenta.IdProductoF);
            ViewData["VentaId"] = new SelectList(_context.Ventas, "IdVenta", "IdVenta", detallesVenta.VentaId);
            return View(detallesVenta);
        }

        // POST: DetallesVentas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFactura,VentaId,IdProductoF,Cantidad,Valor")] Factura factura)
        {
            if (id != factura.IdFactura)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(factura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetallesVentaExists(factura.IdFactura))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProductoF"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", factura.IdProductoF);
            ViewData["VentaId"] = new SelectList(_context.Ventas, "IdVenta", "IdVenta", factura.VentaId);
            return View(factura);
        }

        // GET: DetallesVentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var detallesVenta = await _context.Facturas
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .FirstOrDefaultAsync(m => m.IdFactura == id);
            if (detallesVenta == null)
            {
                return NotFound();
            }

            return View(detallesVenta);
        }

        // POST: DetallesVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Facturas == null)
            {
                return Problem("Entity set 'TallerContext.DetallesVentas'  is null.");
            }
            var detallesVenta = await _context.Facturas.FindAsync(id);
            if (detallesVenta != null)
            {
                _context.Facturas.Remove(detallesVenta);
                await _context.SaveChangesAsync();

                // Actualizar existencias del inventario
                var producto = await _context.Productos.FindAsync(detallesVenta.IdProductoF);
                producto.CantidadProducto += detallesVenta.Cantidad;
                _context.Productos.Update(producto);

                // Se actualiza la venta
                var venta = await _context.Ventas.FindAsync(detallesVenta.VentaId);
                venta.TotalVenta = 0;
                foreach (var item in await _context.Facturas.Where(p => p.VentaId == detallesVenta.VentaId).ToListAsync())
                {
                    venta.TotalVenta += item.Valor * item.Cantidad;
                }
                _context.Ventas.Update(venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetallesVentaExists(int id)
        {
            return (_context.Facturas?.Any(e => e.IdFactura == id)).GetValueOrDefault();
        }
    }
}
