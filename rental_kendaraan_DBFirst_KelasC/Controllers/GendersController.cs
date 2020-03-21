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
    public class GendersController : Controller
    {
        private readonly rental_kendaraanContext _context;

        public GendersController(rental_kendaraanContext context)
        {
            _context = context;
        }

        // GET: Genders
        public async Task<IActionResult> Index(string gender, string searchString, string searchFil, string sortOrder, string currentFilter, int? pageNumber)
        {
            //buat list menyimpan ktersediaan
            {
                var genList = new List<String>();
                var genQuery = from d in _context.Gender orderby d.NamaGender select d.NamaGender;
                genList.AddRange(genQuery.Distinct());

                //menampilkan kedalam dropdown ListView di view
                ViewBag.gender = new SelectList(genList);


                var menu = from m in _context.Gender select m;
                if (!string.IsNullOrEmpty(gender))
                {
                    menu = menu.Where(x => x.NamaGender == gender);
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    menu = menu.Where(s => s.NamaGender.Contains(searchString));
                }
                ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

                switch (sortOrder)
                {
                    case "name_desc":
                        menu = menu.OrderByDescending(s => s.NamaGender);
                        break;
                    default: //name ascending
                        menu = menu.OrderBy(s => s.NamaGender);
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
                return View(await PaginatedList<Gender>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
        }

        // GET: Genders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = await _context.Gender
                .FirstOrDefaultAsync(m => m.IdGender == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }

        // GET: Genders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGender,NamaGender")] Gender gender)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gender);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gender);
        }

        // GET: Genders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = await _context.Gender.FindAsync(id);
            if (gender == null)
            {
                return NotFound();
            }
            return View(gender);
        }

        // POST: Genders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGender,NamaGender")] Gender gender)
        {
            if (id != gender.IdGender)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gender);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenderExists(gender.IdGender))
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
            return View(gender);
        }

        // GET: Genders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = await _context.Gender
                .FirstOrDefaultAsync(m => m.IdGender == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }

        // POST: Genders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gender = await _context.Gender.FindAsync(id);
            _context.Gender.Remove(gender);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenderExists(int id)
        {
            return _context.Gender.Any(e => e.IdGender == id);
        }
    }
}
