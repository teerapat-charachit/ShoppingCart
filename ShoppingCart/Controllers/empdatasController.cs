using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class empdatasController : Controller
    {
        private readonly DataContext _context;

        public empdatasController(DataContext context)
        {
            _context = context;
        }

        // GET: empdatas
        public async Task<IActionResult> Index()
        {
              return View(await _context.empdata.ToListAsync());
        }

        // GET: empdatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.empdata == null)
            {
                return NotFound();
            }

            var empdata = await _context.empdata
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empdata == null)
            {
                return NotFound();
            }

            return View(empdata);
        }

        // GET: empdatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: empdatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,lastname,nickname,gen,religion,birthday,idcard,phone,Email,address")] empdata empdata)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empdata);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empdata);
        }

        // GET: empdatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.empdata == null)
            {
                return NotFound();
            }

            var empdata = await _context.empdata.FindAsync(id);
            if (empdata == null)
            {
                return NotFound();
            }
            return View(empdata);
        }

        // POST: empdatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,lastname,nickname,gen,religion,birthday,idcard,phone,Email,address")] empdata empdata)
        {
            if (id != empdata.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empdata);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!empdataExists(empdata.Id))
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
            return View(empdata);
        }

        // GET: empdatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.empdata == null)
            {
                return NotFound();
            }

            var empdata = await _context.empdata
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empdata == null)
            {
                return NotFound();
            }

            return View(empdata);
        }

        // POST: empdatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.empdata == null)
            {
                return Problem("Entity set 'DataContext.empdata'  is null.");
            }
            var empdata = await _context.empdata.FindAsync(id);
            if (empdata != null)
            {
                _context.empdata.Remove(empdata);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool empdataExists(int id)
        {
          return _context.empdata.Any(e => e.Id == id);
        }
    }
}
