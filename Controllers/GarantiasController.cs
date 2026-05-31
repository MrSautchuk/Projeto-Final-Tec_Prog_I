using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GestaoGaragem.Data;
using GestaoGaragem.Models;

namespace GestaoGaragem.Controllers
{
    [Authorize(Roles = "Gerente,Admin")]
    public class GarantiasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GarantiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Garantias
        public async Task<IActionResult> Index()
        {
            // Inclui os dados da Venda relacionada para exibição
            var garantias = _context.Garantias.Include(g => g.Venda);
            return View(await garantias.ToListAsync());
        }

        // GET: Garantias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garantia = await _context.Garantias
                .Include(g => g.Venda)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (garantia == null)
            {
                return NotFound();
            }

            return View(garantia);
        }

        // GET: Garantias/Create
        public IActionResult Create()
        {
            ViewData["VendaId"] = new SelectList(_context.Vendas, "Id", "Id");
            return View();
        }

        // POST: Garantias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VendaId,Descricao,DataVencimento")] Garantia garantia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(garantia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["VendaId"] = new SelectList(_context.Vendas, "Id", "Id", garantia.VendaId);
            return View(garantia);
        }

        // GET: Garantias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garantia = await _context.Garantias.FindAsync(id);
            if (garantia == null)
            {
                return NotFound();
            }
            
            ViewData["VendaId"] = new SelectList(_context.Vendas, "Id", "Id", garantia.VendaId);
            return View(garantia);
        }

        // POST: Garantias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VendaId,Descricao,DataVencimento")] Garantia garantia)
        {
            if (id != garantia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(garantia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GarantiaExists(garantia.Id))
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
            
            ViewData["VendaId"] = new SelectList(_context.Vendas, "Id", "Id", garantia.VendaId);
            return View(garantia);
        }

        // GET: Garantias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garantia = await _context.Garantias
                .Include(g => g.Venda)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (garantia == null)
            {
                return NotFound();
            }

            return View(garantia);
        }

        // POST: Garantias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var garantia = await _context.Garantias.FindAsync(id);
            if (garantia != null)
            {
                // Remove o registro de garantia do contexto de forma limpa
                _context.Garantias.Remove(garantia);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Garantias/Estender/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Estender(int id, int dias)
        {
            var garantia = await _context.Garantias.FindAsync(id);
            if (garantia != null)
            {
                garantia.DataFim = garantia.DataFim.AddDays(dias);
                garantia.Status = "Ativa"; // Reativa a garantia caso estivesse expirada
                _context.Update(garantia);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GarantiaExists(int id)
        {
            return _context.Garantias.Any(e => e.Id == id);
        }
    }
}