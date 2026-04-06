
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokoSaya.Data;
using TokoSaya.Models;
using TokoSaya.ViewModels;

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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
        if (userId == null)
        {
            return BadRequest();
        }
        var cartFromDb = _context.ShoppingCarts
            .FirstOrDefault(c => c.ApplicationUserId == userId && c.ProductId == productId);

        if (cartFromDb != null)
        {
            cartFromDb.Quantity += 1;
            _context.ShoppingCarts.Update(cartFromDb);
        }
        else
        {
            var newCartItem = new ShoppingCart
            {
                ApplicationUserId = userId,
                ProductId = productId,
                Quantity = 1
            };

            _context.ShoppingCarts.Add(newCartItem);
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

    [HttpGet]
    public IActionResult Summary()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var shoppingCartVM = new ShoppingCartVM()
        {
          ListCart = _context.ShoppingCarts.Include(u=> u.Product).Where(u=> u.ApplicationUserId == userId).ToList(),
          OrderHeader = new OrderHeader()  
        };
        decimal orderTotal = 0;
        foreach(var item in shoppingCartVM.ListCart)
        {
            orderTotal += item.Quantity * item.Product.Price;
        }
        shoppingCartVM.OrderHeader.OrderTotal = orderTotal;
        return View(shoppingCartVM);
    }
}