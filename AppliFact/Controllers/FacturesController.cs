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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AppliFact.Controllers
{
    public class FacturesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public FacturesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Factures
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var applicationDbContext = _context.Factures.Include(f => f.Client);
            return View(await _context.Factures.Where(c=>c.Client.OwnerID== UserID).ToListAsync());
        }
        [Authorize]
        public async Task<IActionResult> Valider(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facture = await _context.Factures.FindAsync(id);
            
            if (facture == null)
            {
                return NotFound();
            }
            return View(facture);
            
        }
        // POST: Factures/Valider
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Valider(Guid id,[Bind("Id,DateEdition,NumeroFacture,IdClient")]Facture facture)
        {            
            if (id != facture.Id)
                {
                    return NotFound();
                }

            if (ModelState.IsValid)
            {
                
                    facture.DateEdition = DateTime.Now;
                    var annee = DateTime.Now.Year;
                    var lastFacture = _context.Factures.Where(c => c.NumeroFacture
                   .StartsWith(annee.ToString())).OrderBy(c => c.NumeroFacture).LastOrDefault();
                    var nextNumero = annee.ToString() + "-0001";
                if (lastFacture != null)
                {
                    nextNumero = annee + "-" + (int.Parse(lastFacture.NumeroFacture
                        .Substring(5, 4)) + 1).ToString("0000");

                }
                
                    facture.NumeroFacture = nextNumero;
                    try
                    {

                        _context.Update(facture);
                        await _context.SaveChangesAsync();
                    }


                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FactureExists(facture.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Details", new { id = facture.Id });
                
            }
            facture.Client = _context.Clients.Find(facture.IdClient);
            return View(facture);
        }
        // GET: Factures/Details/5
        [Authorize]
        public async Task<IActionResult> Details(Guid? id, Guid? selectedPrestation = null)
        {
                if (id == null)
                {
                    return NotFound();
                }
                Facture facture = await _context.Factures
                    .Include(f => f.Client)
                    .Include(f => f.Prestations)
                    .Include(f => f.ApplicationUser)
                    .FirstOrDefaultAsync(m => m.Id == id);
            if (facture.Client.OwnerID != this.User.FindFirstValue(ClaimTypes.NameIdentifier))
            { 
                 return RedirectToAction("Index","Facture");
            }
                if (facture == null)
                {
                    return NotFound();
                }
                if (selectedPrestation != null)
                {
                    ViewBag.SelectedPrestation = facture.Prestations.FirstOrDefault(c => c.Id == selectedPrestation);
                }
                return View(facture);
            

            
        }

        // GET: Factures/Create
        [Authorize]
        public async Task<IActionResult> CreateAsync( Guid idClient)
        {           
            var client = _context.Clients.Find(idClient);
            var facture = new Facture() { IdClient = idClient, Client = client };
            return View(facture);
        }

        // POST: Factures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroFactureFacture,DateCreation,DateEdition,DateEcheance,IdClient")] Facture facture)
        {
            var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {               
                _context.Add(facture);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Prestations", new { idFacture = facture.Id });
            }
            facture.Client = _context.Clients.Find(facture.IdClient);
            return View(facture);
        }

        // GET: Factures/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facture = await _context.Factures.FindAsync(id);

            if (facture == null)
            {
                return NotFound();
            }
            if (facture.Client.OwnerID != this.User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return RedirectToAction("Index", "Facture");
            }
            ViewData["IdClient"] = new SelectList(_context.Clients, "Id", "Adresse", facture.IdClient);
            return View(facture);
        }

        // POST: Factures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,NumeroFactureFacture,DateCreation,DateEdition,DateEcheance,IdClient")] Facture facture)
        {
            if (id != facture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactureExists(facture.Id))
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
            facture.Client = _context.Clients.Find(facture.IdClient);
            return View(facture);
        }

        // GET: Factures/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facture = await _context.Factures
                .Include(f => f.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facture == null)
            {
                return NotFound();
            }
            if (facture.Client.OwnerID != this.User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return RedirectToAction("Index", "Facture");
            }
            return View(facture);
        }

        // POST: Factures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var facture = await _context.Factures.FindAsync(id);
            _context.Factures.Remove(facture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FactureExists(Guid id)
        {
            return _context.Factures.Any(e => e.Id == id);
        }
    }
}
