using Microsoft.EntityFrameworkCore;
using GestaoGaragem.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login"; // Redireciona para cá caso não esteja logado
        options.AccessDeniedPath = "/Usuarios/Login"; // se não tiver permissão
    });

var app = builder.Build();

// http request
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Login}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Verifica se já existe algum usuário para não duplicar
    if (!context.Usuarios.Any())
    {
        context.Usuarios.Add(new GestaoGaragem.Models.Usuario
        {
            NomeUsuario = "admin",
            Senha = "123",
            Perfil = "Admin"
        });
        context.SaveChanges();
    }
}

app.Run();
