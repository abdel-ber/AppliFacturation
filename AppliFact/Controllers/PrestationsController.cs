using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppliFact.Data;
using AppliFact.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AppliFact.Controllers
{
    public class PrestationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrestationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prestations
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext =_context.Prestations.Include(p => p.Facture);
            var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var applicationDbContext = _context.Factures.Include(f => f.Client);
            return View(await _context.Prestations.Where(c => c.Facture.Client.OwnerID == UserID).ToListAsync());
        }

        // GET: Prestations/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var prestation = await _context.Prestations
        //        .Include(p => p.Facture)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (prestation == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(prestation);
        //}

        // GET: Prestations/Create
        [Authorize]
        public IActionResult Create(Guid idFacture)
        {
            var facture = _context.Factures.Find(idFacture);
            //ViewBag.Title = $"Prestation sur Facture {facture.NumeroFacture} pour {facture.Client.RaisonSociale}";
            var prestation = new Prestation { IdFacture = idFacture };

            return View(prestation);
        }

        // POST: Prestations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MontantHorsTax,TauxTva,Description,IdFacture")] Prestation prestation)
        {
            var facture = _context.Factures.Find(prestation.IdFacture);
            //ViewBag.Title = $"Prestation sur Facture {facture.NumeroFacture} pour {facture.Client.RaisonSociale}";
            if (ModelState.IsValid)
            {
                prestation.Id = Guid.NewGuid();
                _context.Add(prestation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Factures", new { id = prestation.IdFacture });
            }
            
            return View(prestation);
        }

        // GET: Prestations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestation = await _context.Prestations.FindAsync(id);
            if (prestation == null)
            {
                return NotFound();
            }
            var facture = _context.Factures.Find(prestation.IdFacture);
           // ViewBag.Title = $"Prestation sur Facture {facture.NumeroFacture} pour {facture.Client.RaisonSociale}";
            return View(prestation);
        }

        // POST: Prestations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,MontantHorsTax,TauxTva,Description,IdFacture")] Prestation prestation)
        {
            if (id != prestation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestationExists(prestation.Id))
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
            var facture = _context.Factures.Find(prestation.IdFacture);
            ViewBag.Title = $"Prestation sur Facture {facture.NumeroFacture} pour {facture.Client.RaisonSociale}";
            return View(prestation);
        }

        // GET: Prestations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestation = await _context.Prestations
                .Include(p => p.Facture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestation == null)
            {
                return NotFound();
            }

            return View(prestation);
        }

        // POST: Prestations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var prestation = await _context.Prestations.FindAsync(id);
            _context.Prestations.Remove(prestation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestationExists(Guid id)
        {
            return _context.Prestations.Any(e => e.Id == id);
        }
    }
}
