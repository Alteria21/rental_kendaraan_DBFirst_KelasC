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
    public class JenisKendaraanController : Controller
    {
        private readonly rental_kendaraanContext _context;

        public JenisKendaraanController(rental_kendaraanContext context)
        {
            _context = context;
        }

        // GET: JenisKendaraan
        public async Task<IActionResult> Index(string Jenis, string searchString, string searchFil, string sortOrder, string currentFilter, int? pageNumber)
        {
            {
                var jenList = new List<String>();
                var jenQuery = from d in _context.JenisKendaraan orderby d.NamaJenisKendaraan select d.NamaJenisKendaraan;
                jenList.AddRange(jenQuery.Distinct());


                ViewBag.Jenis =  new SelectList(jenList);


                var menu = from m in _context.JenisKendaraan select m;
                if (!string.IsNullOrEmpty(Jenis))
                {
                    menu = menu.Where(x => x.NamaJenisKendaraan == Jenis);
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    menu = menu.Where(s => s.NamaJenisKendaraan.Contains(searchString));
                }
                ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

                switch (sortOrder)
                {
                    case "name_desc":
                        menu = menu.OrderByDescending(s => s.NamaJenisKendaraan);
                        break;
                    default: //name ascending
                        menu = menu.OrderBy(s => s.NamaJenisKendaraan);
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
                return View(await PaginatedList<JenisKendaraan>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
        }

        // GET: JenisKendaraan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jenisKendaraan = await _context.JenisKendaraan
                .FirstOrDefaultAsync(m => m.IdJenisKendaraan == id);
            if (jenisKendaraan == null)
            {
                return NotFound();
            }

            return View(jenisKendaraan);
        }

        // GET: JenisKendaraan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JenisKendaraan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdJenisKendaraan,NamaJenisKendaraan")] JenisKendaraan jenisKendaraan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jenisKendaraan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jenisKendaraan);
        }

        // GET: JenisKendaraan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jenisKendaraan = await _context.JenisKendaraan.FindAsync(id);
            if (jenisKendaraan == null)
            {
                return NotFound();
            }
            return View(jenisKendaraan);
        }

        // POST: JenisKendaraan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdJenisKendaraan,NamaJenisKendaraan")] JenisKendaraan jenisKendaraan)
        {
            if (id != jenisKendaraan.IdJenisKendaraan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jenisKendaraan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JenisKendaraanExists(jenisKendaraan.IdJenisKendaraan))
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
            return View(jenisKendaraan);
        }

        // GET: JenisKendaraan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jenisKendaraan = await _context.JenisKendaraan
                .FirstOrDefaultAsync(m => m.IdJenisKendaraan == id);
            if (jenisKendaraan == null)
            {
                return NotFound();
            }

            return View(jenisKendaraan);
        }

        // POST: JenisKendaraan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jenisKendaraan = await _context.JenisKendaraan.FindAsync(id);
            _context.JenisKendaraan.Remove(jenisKendaraan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JenisKendaraanExists(int id)
        {
            return _context.JenisKendaraan.Any(e => e.IdJenisKendaraan == id);
        }
    }
}
