using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppliFact.Data;
using AppliFact.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;



namespace AppliFact.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context
        
        )
        
        {
            _context = context;
        }

        // GET: Clients
        [Authorize]
        public async Task<IActionResult> Index()
        {
             var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _context.Clients.Where(c => c.OwnerID == UserID).ToListAsync());
        }

        // GET: Clients/Details/5
        [Authorize]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c=>c.Factures)
                .Include(c=>c.Paiements)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client.OwnerID != this.User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return RedirectToAction("Index", "Client");
            }
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        [Authorize]
        public IActionResult Create(string UserID)
        {

            
            UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Find(UserID);
            var client = new Client()
            {
                OwnerID = UserID,
                

            };
            return View(client);
        }

        // POST: Factures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public class CreatModel : DI_BasePageModel
        //{
        //    public CreatModel(
        //ApplicationDbContext context,
        //IAuthorizationService authorizationService,
        //UserManager<IdentityUser> userManager)
        //: base(context, authorizationService, userManager)

        //    {

        //    }
        public async Task<IActionResult> Create([Bind("OwnerID,Id,RaisonSociale,Adresse,CodePostal,Ville,Pays")] Client client)/* ([Bind("OwnerID,Id,RaisonSociale,Adresse,CodePostal,Ville,Pays")] Client client)*/
            {
                if (ModelState.IsValid)
                {
                    client.OwnerID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    client.Id = Guid.NewGuid();
                    _context.Clients.Add(client);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Create", "Factures", new { idClient = client.Id});
                    
                }
            client = _context.Clients.Find(client.OwnerID);
            return View(client);
        }

        // GET: Clients/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client.OwnerID != this.User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return RedirectToAction("Index", "Client");
            }
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OwnerID,Id,RaisonSociale,Adresse,CodePostal,Ville,Pays")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client.OwnerID != this.User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return RedirectToAction("Index", "Client");
            }
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(Guid id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
