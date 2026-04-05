
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokoSaya.Data;
using TokoSaya.Models;

namespace TokoSaya.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpPost]
    public IActionResult AddToCart(int productId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Ambil userId yang sedang aktif
        if (userId == null)
        {
            return BadRequest();
        }

        // Cari apakah produk tersebut sudah ada di keranjang untuk user ini
        var cartFromDb = _context.ShoppingCarts
            .FirstOrDefault(c => c.ApplicationUserId == userId && c.ProductId == productId);

        if (cartFromDb != null)
        {
            // Jika produk sudah ada di keranjang, tambahkan kuantitasnya
            cartFromDb.Quantity += 1;
            _context.ShoppingCarts.Update(cartFromDb); // Update cart yang ada
        }
        else
        {
            // Jika produk belum ada di keranjang, buat entri baru
            var newCartItem = new ShoppingCart
            {
                ApplicationUserId = userId,
                ProductId = productId,
                Quantity = 1
            };

            _context.ShoppingCarts.Add(newCartItem); // Tambahkan item baru ke keranjang
        }

        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cartItems = _context.ShoppingCarts.Include(u => u.Product).Where(u => u.ApplicationUserId == userId).ToList();
        return View(cartItems);
    }
}