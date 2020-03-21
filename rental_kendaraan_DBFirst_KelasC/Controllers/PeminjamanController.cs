using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rental_kendaraan_DBFirst_KelasC.Models;
using Microsoft.AspNetCore.Authorization;

namespace rental_kendaraan_DBFirst_KelasC.Controllers
{
    public class PeminjamanController : Controller
    {
        private readonly rental_kendaraanContext _context;

        public PeminjamanController(rental_kendaraanContext context)
        {
            _context = context;
        }

        // GET: Peminjaman
        [Authorize(Policy = "readonlypolicy")]
        public async Task<IActionResult> Index(string tgl, string searchString, string searchFil, string sortOrder, string currentFilter, int? pageNumber
)
        {
            var tglList = new List<DateTime?>();

            //query untuk mengambil data
            var tglQuery = from d in _context.Peminjaman orderby d.TglPeminjaman select d.TglPeminjaman;

            tglList.AddRange(tglQuery.Distinct());

            //Menampilkan kedalam dropdown list di view
            ViewBag.tgl = new SelectList(tglList);

            //mengambil semua data //cari  foreign key
            var menu = from m in _context.Peminjaman
                .Include(p => p.IdCustomerNavigation)
                .Include(p => p.IdJaminanNavigation)
                .Include(p => p.IdKendaraanNavigation)
                       select m;

            // dropdown list atau filter data
            if (!string.IsNullOrEmpty(tgl))
            {
                menu = menu.Where(x => x.TglPeminjaman == Convert.ToDateTime(tgl));
            }


            //Search data
            if (!string.IsNullOrEmpty(searchFil))
            {
                menu = menu.Where(s => s.IdCustomerNavigation.NamaCustomer.Contains(searchFil) || s.IdJaminanNavigation.NamaJaminan.Contains(searchFil) || s.IdKendaraanNavigation.NamaKendaraan.Contains(searchFil));
            }

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.IdCustomerNavigation.NamaCustomer);
                    break;
                case "Date":
                    menu = menu.OrderBy(s => s.TglPeminjaman);
                    break;
                case "date_desc":
                    menu = menu.OrderByDescending(s => s.TglPeminjaman);
                    break;
                default: //name ascending
                    menu = menu.OrderBy(s => s.IdCustomerNavigation.NamaCustomer);
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
            return View(await PaginatedList<Peminjaman>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
        }



        // GET: Peminjaman/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjaman
                .Include(p => p.IdCustomerNavigation)
                .Include(p => p.IdJaminanNavigation)
                .Include(p => p.IdKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdPeminjaman == id);
            if (peminjaman == null)
            {
                return NotFound();
            }

            return View(peminjaman);
        }

        // GET: Peminjaman/Create
        [Authorize(Policy = "writepolicy")]
        public IActionResult Create()
        {
            ViewData["IdCustomer"] = new SelectList(_context.Customer, "IdCustomer", "NamaCustomer");
            ViewData["IdJaminan"] = new SelectList(_context.Jaminan, "IdJaminan", "IdJaminan");
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraan, "IdKendaraan", "IdKendaraan");
            return View();
        }

        // POST: Peminjaman/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPeminjaman,TglPeminjaman,IdKendaraan,IdCustomer,IdJaminan,Biaya")] Peminjaman peminjaman)
        {
            if (ModelState.IsValid)
            {
                _context.Add(peminjaman);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCustomer"] = new SelectList(_context.Customer, "IdCustomer", "NamaCustomer", peminjaman.IdCustomer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminan, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraan, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        // GET: Peminjaman/Edit/5
        [Authorize(Policy = "editpolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjaman.FindAsync(id);
            if (peminjaman == null)
            {
                return NotFound();
            }
            ViewData["IdCustomer"] = new SelectList(_context.Customer, "IdCustomer", "NamaCustomer", peminjaman.IdCustomer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminan, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraan, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        // POST: Peminjaman/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPeminjaman,TglPeminjaman,IdKendaraan,IdCustomer,IdJaminan,Biaya")] Peminjaman peminjaman)
        {
            if (id != peminjaman.IdPeminjaman)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(peminjaman);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeminjamanExists(peminjaman.IdPeminjaman))
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
            ViewData["IdCustomer"] = new SelectList(_context.Customer, "IdCustomer", "NamaCustomer", peminjaman.IdCustomer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminan, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraan, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        // GET: Peminjaman/Delete/5
        [Authorize(Policy = "deletepolicy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjaman
                .Include(p => p.IdCustomerNavigation)
                .Include(p => p.IdJaminanNavigation)
                .Include(p => p.IdKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdPeminjaman == id);
            if (peminjaman == null)
            {
                return NotFound();
            }

            return View(peminjaman);
        }

        // POST: Peminjaman/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peminjaman = await _context.Peminjaman.FindAsync(id);
            _context.Peminjaman.Remove(peminjaman);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeminjamanExists(int id)
        {
            return _context.Peminjaman.Any(e => e.IdPeminjaman == id);
        }
    }
}
