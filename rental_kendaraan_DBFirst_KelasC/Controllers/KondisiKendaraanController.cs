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
    public class KondisiKendaraanController : Controller
    {
        private readonly rental_kendaraanContext _context;

        public KondisiKendaraanController(rental_kendaraanContext context)
        {
            _context = context;
        }

        // GET: KondisiKendaraan
        public async Task<IActionResult> Index(string Kondisi,string searchString, string searchFil, string sortOrder, string currentFilter, int? pageNumber)
        {
            {
                var jenList = new List<String>();
                var jenQuery = from d in _context.KondisiKendaraan orderby d.NamaKondisi select d.NamaKondisi;
                jenList.AddRange(jenQuery.Distinct());


                ViewBag.Kondisi = new SelectList(jenList);


                var menu = from m in _context.KondisiKendaraan select m;
                if (!string.IsNullOrEmpty(Kondisi))
                {
                    menu = menu.Where(x => x.NamaKondisi == Kondisi);
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    menu = menu.Where(s => s.NamaKondisi.Contains(searchString));
                }

                ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_kondisi" : "";

                switch (sortOrder)
                {
                    case "name_kondisi":
                        menu = menu.OrderByDescending(s => s.NamaKondisi);
                        break;
                    default: //name ascending
                        menu = menu.OrderBy(s => s.NamaKondisi);
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


                int pageSize = 5;
                return View(await PaginatedList<KondisiKendaraan>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
        }

        // GET: KondisiKendaraan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kondisiKendaraan = await _context.KondisiKendaraan
                .FirstOrDefaultAsync(m => m.IdKondisi == id);
            if (kondisiKendaraan == null)
            {
                return NotFound();
            }

            return View(kondisiKendaraan);
        }

        // GET: KondisiKendaraan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KondisiKendaraan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKondisi,NamaKondisi")] KondisiKendaraan kondisiKendaraan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kondisiKendaraan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kondisiKendaraan);
        }

        // GET: KondisiKendaraan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kondisiKendaraan = await _context.KondisiKendaraan.FindAsync(id);
            if (kondisiKendaraan == null)
            {
                return NotFound();
            }
            return View(kondisiKendaraan);
        }

        // POST: KondisiKendaraan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKondisi,NamaKondisi")] KondisiKendaraan kondisiKendaraan)
        {
            if (id != kondisiKendaraan.IdKondisi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kondisiKendaraan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KondisiKendaraanExists(kondisiKendaraan.IdKondisi))
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
            return View(kondisiKendaraan);
        }

        // GET: KondisiKendaraan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kondisiKendaraan = await _context.KondisiKendaraan
                .FirstOrDefaultAsync(m => m.IdKondisi == id);
            if (kondisiKendaraan == null)
            {
                return NotFound();
            }

            return View(kondisiKendaraan);
        }

        // POST: KondisiKendaraan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kondisiKendaraan = await _context.KondisiKendaraan.FindAsync(id);
            _context.KondisiKendaraan.Remove(kondisiKendaraan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KondisiKendaraanExists(int id)
        {
            return _context.KondisiKendaraan.Any(e => e.IdKondisi == id);
        }
    }
}
