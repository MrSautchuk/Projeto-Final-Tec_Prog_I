using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using GestaoGaragem.Data;
using GestaoGaragem.Models;

namespace GestaoGaragem.Controllers
{
    [Authorize]
    public class VeiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VeiculosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Veiculos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Veiculos.ToListAsync());
        }

        // GET: Veiculos/Showroom
        public async Task<IActionResult> Showroom()
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.Status == "Disponivel" || v.Status == "Disponível")
                .ToListAsync();
            return View(veiculos);
        }

        // GET: Veiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // GET: Veiculos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Veiculos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Marca,Modelo,AnoFabricacao,AnoModelo,Placa,Valor,Status")] Veiculo veiculo, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null && foto.Length > 0)
                {
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        await foto.CopyToAsync(memoryStream);
                        veiculo.FotoBase64 = System.Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(veiculo);
        }

        // GET: Veiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }
            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Marca,Modelo,AnoFabricacao,AnoModelo,Placa,Valor,Status,FotoBase64")] Veiculo veiculo, IFormFile novaFoto)
        {
            if (id != veiculo.Id)
            {
                return NotFound();
            }

            ModelState.Remove("novaFoto");

            if (ModelState.IsValid)
            {
                try
                {
                    if (novaFoto != null && novaFoto.Length > 0)
                    {
                        using (var memoryStream = new System.IO.MemoryStream())
                        {
                            await novaFoto.CopyToAsync(memoryStream);
                            veiculo.FotoBase64 = System.Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }

                    _context.Update(veiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.Id))
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
            return View(veiculo);
        }

        // GET: Veiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // POST: Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo != null)
            {
                // Exclusão em cascata: Remove Vendas e Garantias associadas para evitar FK Error
                var vendas = await _context.Vendas.Where(v => v.VeiculoId == id).ToListAsync();
                foreach (var venda in vendas)
                {
                    var garantias = await _context.Garantias.Where(g => g.VendaId == venda.Id).ToListAsync();
                    if (garantias.Any())
                    {
                        _context.Garantias.RemoveRange(garantias);
                    }
                }
                
                if (vendas.Any())
                {
                    _context.Vendas.RemoveRange(vendas);
                }

                _context.Veiculos.Remove(veiculo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}