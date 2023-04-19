using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Finances.Data;
using Finances.Models;

namespace Finances.Controllers
{
    public class ExpansesController : Controller
    {
        private readonly FinancesContext _context;

        public ExpansesController(FinancesContext context)
        {
            _context = context;
        }

        // GET: Expanses
        public async Task<IActionResult> Index()
        {
              return _context.Expanse != null ? 
                          View(await _context.Expanse.ToListAsync()) :
                          Problem("Entity set 'FinancesContext.Expanse'  is null.");
        }

        // GET: Expanses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Expanse == null)
            {
                return NotFound();
            }

            var expanse = await _context.Expanse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expanse == null)
            {
                return NotFound();
            }

            return View(expanse);
        }

        // GET: Expanses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Expanses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Value,Category,Created,Description")] Expanse expanse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expanse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expanse);
        }

        // GET: Expanses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Expanse == null)
            {
                return NotFound();
            }

            var expanse = await _context.Expanse.FindAsync(id);
            if (expanse == null)
            {
                return NotFound();
            }
            return View(expanse);
        }

        // POST: Expanses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Value,Category,Created,Description")] Expanse expanse)
        {
            if (id != expanse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expanse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpanseExists(expanse.Id))
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
            return View(expanse);
        }

        // GET: Expanses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Expanse == null)
            {
                return NotFound();
            }

            var expanse = await _context.Expanse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expanse == null)
            {
                return NotFound();
            }

            return View(expanse);
        }

        // POST: Expanses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Expanse == null)
            {
                return Problem("Entity set 'FinancesContext.Expanse'  is null.");
            }
            var expanse = await _context.Expanse.FindAsync(id);
            if (expanse != null)
            {
                _context.Expanse.Remove(expanse);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpanseExists(int id)
        {
          return (_context.Expanse?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
