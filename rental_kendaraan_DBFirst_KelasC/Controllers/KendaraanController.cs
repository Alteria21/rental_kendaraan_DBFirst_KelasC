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
    public class KendaraanController : Controller
    {
        private readonly rental_kendaraanContext _context;

        public KendaraanController(rental_kendaraanContext context)
        {
            _context = context;
        }

        // GET: Kendaraan
        public async Task<IActionResult> Index(string ktsd, string searchString, string searchFil, string sortOrder, string currentFilter, int? pageNumber)

            //buat list menyimpan ktersediaan
        {
            var ktsdList = new List<String>();
            var ktsdQuery = from d in _context.Kendaraan orderby d.Ketersediaan select d.Ketersediaan;
            ktsdList.AddRange(ktsdQuery.Distinct());

            //menampilkan kedalam dropdown ListView di view
            ViewBag.ktsd = new SelectList(ktsdList);


            var menu = from m in _context.Kendaraan.Include(k => k.IdJenisKendaraanNavigation) select m;
            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.Ketersediaan == ktsd);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.NoPolisi.Contains(searchString));
            }

            //var rental_kendaraanContext = _context.Kendaraan.Include(k => k.IdJenisKendaraanNavigation);
            //return View(await rental_kendaraanContext.ToListAsync());

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.NamaKendaraan);
                    break;
                case "Date":
                    menu = menu.OrderBy(s => s.NoPolisi);
                    break;
                default: //name ascending
                    menu = menu.OrderBy(s => s.NamaKendaraan);
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
            return View(await PaginatedList<Kendaraan>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Kendaraan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kendaraan = await _context.Kendaraan
                .Include(k => k.IdJenisKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdKendaraan == id);
            if (kendaraan == null)
            {
                return NotFound();
            }

            return View(kendaraan);
        }

        // GET: Kendaraan/Create
        public IActionResult Create()
        {
            ViewData["IdJenisKendaraan"] = new SelectList(_context.JenisKendaraan, "IdJenisKendaraan", "IdJenisKendaraan");
            return View();
        }

        // POST: Kendaraan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKendaraan,NamaKendaraan,NoPolisi,NoStnk,IdJenisKendaraan,Ketersediaan")] Kendaraan kendaraan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kendaraan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdJenisKendaraan"] = new SelectList(_context.JenisKendaraan, "IdJenisKendaraan", "IdJenisKendaraan", kendaraan.IdJenisKendaraan);
            return View(kendaraan);
        }

        // GET: Kendaraan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kendaraan = await _context.Kendaraan.FindAsync(id);
            if (kendaraan == null)
            {
                return NotFound();
            }
            ViewData["IdJenisKendaraan"] = new SelectList(_context.JenisKendaraan, "IdJenisKendaraan", "IdJenisKendaraan", kendaraan.IdJenisKendaraan);
            return View(kendaraan);
        }

        // POST: Kendaraan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKendaraan,NamaKendaraan,NoPolisi,NoStnk,IdJenisKendaraan,Ketersediaan")] Kendaraan kendaraan)
        {
            if (id != kendaraan.IdKendaraan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kendaraan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KendaraanExists(kendaraan.IdKendaraan))
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
            ViewData["IdJenisKendaraan"] = new SelectList(_context.JenisKendaraan, "IdJenisKendaraan", "IdJenisKendaraan", kendaraan.IdJenisKendaraan);
            return View(kendaraan);
        }

        // GET: Kendaraan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kendaraan = await _context.Kendaraan
                .Include(k => k.IdJenisKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdKendaraan == id);
            if (kendaraan == null)
            {
                return NotFound();
            }

            return View(kendaraan);
        }

        // POST: Kendaraan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kendaraan = await _context.Kendaraan.FindAsync(id);
            _context.Kendaraan.Remove(kendaraan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KendaraanExists(int id)
        {
            return _context.Kendaraan.Any(e => e.IdKendaraan == id);
        }
    }
}
