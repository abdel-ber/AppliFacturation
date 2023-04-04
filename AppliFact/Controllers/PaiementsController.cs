using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppliFact.Data;
using AppliFact.Models;

namespace AppliFact.Controllers
{
    public class PaiementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaiementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Paiements
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Paiements.Include(p => p.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Paiements/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paiement = await _context.Paiements
                .Include(p => p.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paiement == null)
            {
                return NotFound();
            }

            return View(paiement);
        }

        // GET: Paiements/Create
        public IActionResult Create(Guid idClient)
        {
            var client = _context.Clients.Find(idClient);
            var paiement = new Paiement() { IdClient = idClient, Client = client };
            return View(paiement);
        }

        // POST: Paiements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Moyen,Montant,DateReception,DateBanque,IdClient")] Paiement paiement)
        {
            if (ModelState.IsValid)
            {
                paiement.Id = Guid.NewGuid();
                _context.Add(paiement);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Clients", new { id = paiement.IdClient });
            }
            paiement.Client = _context.Clients.Find(paiement.IdClient);
            return View(paiement);
        }

        // GET: Paiements/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paiement = await _context.Paiements.FindAsync(id);
            if (paiement == null)
            {
                return NotFound();
            }
            ViewData["IdClient"] = new SelectList(_context.Clients, "Id", "Adresse", paiement.IdClient);
            return View(paiement);
        }

        // POST: Paiements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Moyen,Montant,DateReception,DateBanque,IdClient")] Paiement paiement)
        {
            if (id != paiement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paiement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaiementExists(paiement.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Clients", new { id = paiement.IdClient });
            }
            
            return View(paiement);
        }

        // GET: Paiements/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paiement = await _context.Paiements
                .Include(p => p.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paiement == null)
            {
                return NotFound();
            }

            return View(paiement);
        }

        // POST: Paiements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var paiement = await _context.Paiements.FindAsync(id);
            _context.Paiements.Remove(paiement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaiementExists(Guid id)
        {
            return _context.Paiements.Any(e => e.Id == id);
        }
    }
}
