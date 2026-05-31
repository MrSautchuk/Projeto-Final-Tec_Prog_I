using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoGaragem.Data;
using GestaoGaragem.Models;

namespace GestaoGaragem.Controllers
{
    [Authorize]
    public class VendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index()
        {
            var vendas = _context.Vendas
                .Include(v => v.Veiculo)
                .Include(v => v.Usuario);
            
            return View(await vendas.ToListAsync());
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Veiculo)
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Vendas/Create
        [Authorize(Roles = "Vendedor,Gerente")]
        public IActionResult Create()
        {
            // Carrega apenas os veículos disponíveis
            var veiculosDisponiveis = _context.Veiculos
                .Where(v => v.Status == "Disponivel" || v.Status == "Disponível")
                .ToList();

            ViewData["VeiculoId"] = new SelectList(veiculosDisponiveis, "Id", "Modelo");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome");
            
            return View();
        }

        // POST: Vendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Vendedor,Gerente")]
        public async Task<IActionResult> Create([Bind("Id,DataVenda,ValorFinal,VeiculoId,UsuarioId,NomeCliente,CpfCliente")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                var veiculo = await _context.Veiculos.FindAsync(venda.VeiculoId);
                if (veiculo != null)
                {
                    venda.ValorFinal = veiculo.Valor;
                    veiculo.Status = "Vendido";
                    _context.Update(veiculo);
                }

                _context.Add(venda);
                await _context.SaveChangesAsync();

                var garantia = new Garantia
                {
                    VendaId = venda.Id,
                    DataInicio = venda.DataVenda,
                    DataFim = venda.DataVenda.AddDays(90),
                    Status = "Ativa"
                };
                _context.Add(garantia);
                await _context.SaveChangesAsync();

                return RedirectToAction("Showroom", "Veiculos");
            }
            
            var veiculosDisponiveis = _context.Veiculos
                .Where(v => v.Status == "Disponivel" || v.Status == "Disponível")
                .ToList();
                
            ViewData["VeiculoId"] = new SelectList(veiculosDisponiveis, "Id", "Modelo", venda.VeiculoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", venda.UsuarioId);
            
            return View(venda);
        }

        // GET: Vendas/Edit/5
        [Authorize(Roles = "Vendedor,Gerente")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }
            
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Modelo", venda.VeiculoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", venda.UsuarioId);
            
            return View(venda);
        }

        // POST: Vendas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Vendedor,Gerente")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataVenda,Valor,VeiculoId,UsuarioId")] Venda venda)
        {
            if (id != venda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.Id))
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
            
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Modelo", venda.VeiculoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", venda.UsuarioId);
            
            return View(venda);
        }

        // GET: Vendas/Delete/5
        [Authorize(Roles = "Vendedor,Gerente")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Veiculo)
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Vendedor,Gerente")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda != null)
            {
                var veiculo = await _context.Veiculos.FindAsync(venda.VeiculoId);
                if (veiculo != null)
                {
                    veiculo.Status = "Disponivel";
                    _context.Update(veiculo);
                }

                // Exclusão em cascata: Limpa a garantia vinculada antes de excluir a venda
                var garantias = await _context.Garantias.Where(g => g.VendaId == id).ToListAsync();
                if (garantias.Any())
                {
                    _context.Garantias.RemoveRange(garantias);
                }

                _context.Vendas.Remove(venda);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.Id == id);
        }
    }
}