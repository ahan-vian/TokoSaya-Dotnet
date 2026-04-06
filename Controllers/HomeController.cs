using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TokoSaya.Data;
using TokoSaya.Models;
using Microsoft.EntityFrameworkCore;
namespace TokoSaya.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var productList = _context.Products.Include(u => u.Category).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                productList = productList.Where(p =>
                    p.Name.Contains(searchString) ||
                    (p.Category != null && p.Category.Name.Contains(searchString))
                );
            }
            return View(productList.ToList());
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