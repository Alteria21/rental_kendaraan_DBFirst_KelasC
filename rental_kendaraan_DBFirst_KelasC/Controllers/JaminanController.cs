using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rental_kendaraan_DBFirst_KelasC.Models;

namespace rental_kendaraan_DBFirst_KelasC.Controllers
{
    public class JaminanController : Controller
    {
        private readonly rental_kendaraanContext _context;

        public JaminanController(rental_kendaraanContext context)
        {
            _context = context;
        }

        // GET: Jaminan
        public async Task<IActionResult> Index(string Jm, string searchString, string searchFil, string sortOrder, string currentFilter, int? pageNumber)
        {

            {
                var jmnList = new List<String>();
                var jmnQuery = from d in _context.Jaminan orderby d.NamaJaminan select d.NamaJaminan;
                jmnList.AddRange(jmnQuery.Distinct());


                ViewBag.Jm = new SelectList(jmnList);


                var menu = from m in _context.Jaminan select m;
                if (!string.IsNullOrEmpty(Jm))
                {
                    menu = menu.Where(x => x.NamaJaminan == Jm);
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    menu = menu.Where(s => s.NamaJaminan.Contains(searchString));
                }
                ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

                switch (sortOrder)
                {
                    case "name_desc":
                        menu = menu.OrderByDescending(s => s.NamaJaminan);
                        break;
                    default: //name ascending
                        menu = menu.OrderBy(s => s.NamaJaminan);
                        break;
                }
                ViewData["CurrentSort"] = sortOrder;

                if (searchString != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewData["CurrentFilter"] = searchString;

                // return View(await menu.ToListAsync());
                int pageSize = 5;
                return View(await PaginatedList<Jaminan>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
        }

        // GET: Jaminan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jaminan = await _context.Jaminan
                .FirstOrDefaultAsync(m => m.IdJaminan == id);
            if (jaminan == null)
            {
                return NotFound();
            }

            return View(jaminan);
        }

        // GET: Jaminan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jaminan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdJaminan,NamaJaminan")] Jaminan jaminan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jaminan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jaminan);
        }

        // GET: Jaminan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jaminan = await _context.Jaminan.FindAsync(id);
            if (jaminan == null)
            {
                return NotFound();
            }
            return View(jaminan);
        }

        // POST: Jaminan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdJaminan,NamaJaminan")] Jaminan jaminan)
        {
            if (id != jaminan.IdJaminan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jaminan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JaminanExists(jaminan.IdJaminan))
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
            return View(jaminan);
        }

        // GET: Jaminan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jaminan = await _context.Jaminan
                .FirstOrDefaultAsync(m => m.IdJaminan == id);
            if (jaminan == null)
            {
                return NotFound();
            }

            return View(jaminan);
        }

        // POST: Jaminan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jaminan = await _context.Jaminan.FindAsync(id);
            _context.Jaminan.Remove(jaminan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JaminanExists(int id)
        {
            return _context.Jaminan.Any(e => e.IdJaminan == id);
        }
    }
}
