﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FogachoReveloProyecto.Models;

namespace FogachoReveloProyecto.Controllers
{
    public class GastosController : Controller
    {
        private readonly FogachoReveloDataBase _context;

        public GastosController(FogachoReveloDataBase context)
        {
            _context = context;
        }

        // GET: Gasto
        // GET: Gasto
        public async Task<IActionResult> PaginaInicial(string categoria)
        {
            var gastos = from g in _context.Gasto select g;

            // Si la categoría no es nula o vacía, filtra los gastos por la categoría seleccionada
            if (!string.IsNullOrEmpty(categoria))
            {
                Categoria categoriaEnum;
                if (Enum.TryParse(categoria, out categoriaEnum))
                {
                    gastos = gastos.Where(g => g.Categorias == categoriaEnum);
                }
            }

            // Pasa el total a la vista
            var subtotalGastos = await gastos.SumAsync(g => g.Valor);
            var subtotalValorPagado = await gastos.SumAsync(g => g.ValorPagado);

            // Calcular el total (diferencia entre subtotal de gastos y subtotal de valor pagado)
            var total = subtotalGastos - subtotalValorPagado;

            // Pasar los subtotales y el total a la vista
            ViewBag.SubtotalGastos = subtotalGastos;
            ViewBag.SubtotalValorPagado = subtotalValorPagado;
            ViewBag.Total = total;


            return View(await gastos.ToListAsync());
        }

        public IActionResult CrearGasto()
        {
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> CrearGasto(Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                gasto.ValidarValor();
                _context.Add(gasto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PaginaInicial)); // Redirige a la lista de gastos después de crear
            }
            return View(gasto); // Si hay errores, vuelve a la vista con el gasto
        }

        // GET: Gasto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gasto
                .FirstOrDefaultAsync(m => m.IdGasto == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }


        // POST: Gasto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        // GET: Gasto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gasto.FindAsync(id);
            if (gasto == null)
            {
                return NotFound();
            }
            return View(gasto);
        }

        // POST: Gasto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGasto,FechaRegristo,FechaFinal,Categorias,Descripcion,Valor,ValorPagado,Estados")] Gasto gasto)
        {
            if (id != gasto.IdGasto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Llamar a ActualizacionPagos antes de actualizar el gasto
                    gasto.ValidarValor(); // Actualizamos el estado
                    _context.Update(gasto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoExists(gasto.IdGasto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(PaginaInicial));
            }
            return View(gasto);
        }

        // GET: Gasto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gasto
                .FirstOrDefaultAsync(m => m.IdGasto == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // POST: Gasto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gasto = await _context.Gasto.FindAsync(id);
            if (gasto != null)
            {
                _context.Gasto.Remove(gasto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PaginaInicial));
        }

        private bool GastoExists(int id)
        {
            return _context.Gasto.Any(e => e.IdGasto == id);
        }
    }
}
