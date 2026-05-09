using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Gerenciador_de_Garagem.Models;

namespace Gerenciador_de_Garagem.Controllers;

public class HomeController : Controller
{
    // public IActionResult Index()
    // {
    //     return View();
    // }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    static List<Veiculo> model = new List<Veiculo>();
    // http://localhost:1234/todo/index
    public ActionResult Index()
    {
        return View(model);
    }
}
