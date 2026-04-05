using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TokoSaya.Data;
using TokoSaya.Models;

namespace TokoSaya.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Ambil semua produk dari database
            var products = _context.Products.ToList();

            // Kirim data produk ke view
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}