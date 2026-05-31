using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using GestaoGaragem.Data;
using GestaoGaragem.Models;

namespace GestaoGaragem.Controllers
{
    [Authorize (Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios/Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Usuarios/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string nomeUsuario, string senha)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NomeUsuario == nomeUsuario && u.Senha == senha);

            if (usuario != null)
            {
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.NomeUsuario),
                    new Claim(ClaimTypes.Role, usuario.Perfil)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Redirecionamento condicional com base no Perfil
                if (usuario.Perfil == "Admin")
                {
                    return RedirectToAction("PainelAdmin", "Usuarios");
                }
                else
                {
                    return RedirectToAction("Showroom", "Veiculos");
                }
            }

            ViewBag.Erro = "Usuário ou senha inválidos.";
            return View();
        }

        // GET/POST: Usuarios/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuarios");
        }

        // GET: Usuarios/PainelAdmin
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> PainelAdmin()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        // POST: Usuarios/CadastrarUsuario
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastrarUsuario([Bind("NomeUsuario,Senha,Perfil")] Usuario usuario, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null && foto.Length > 0)
                {
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        await foto.CopyToAsync(memoryStream);
                        usuario.FotoBase64 = System.Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(PainelAdmin));
        }

        // POST: Usuarios/ExcluirUsuario
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(PainelAdmin));
        }
    }
}