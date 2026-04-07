
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokoSaya.Data;
using TokoSaya.Models;
using TokoSaya.ViewModels;
using TokoSaya.Utility;

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ProceedToCheckout(List<int> selectedCartIds, Dictionary<int, int> quantities)
    {
        if (selectedCartIds == null || selectedCartIds.Count == 0)
        {
            TempData["Error"] = "Pilih minimal satu barang untuk dicheckout!";
            return RedirectToAction(nameof(Index));
        }
        foreach (var cartId in selectedCartIds)
        {
            var cartFromDb = _context.ShoppingCarts.FirstOrDefault(c => c.Id == cartId);
            if (cartFromDb != null && quantities.ContainsKey(cartId))
            {
                cartFromDb.Quantity = quantities[cartId];
            }
        }
        _context.SaveChanges();
        HttpContext.Session.SetString("SelectedCartIds", System.Text.Json.JsonSerializer.Serialize(selectedCartIds));
        return RedirectToAction("Summary");
    }

    [HttpGet]
    public IActionResult Summary()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var selectedIdsJson = HttpContext.Session.GetString("SelectedCartIds");
        if (string.IsNullOrEmpty(selectedIdsJson))
        {
            return RedirectToAction(nameof(Index));
        }
        var selectedCartIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(selectedIdsJson) ?? new List<int>();

        var shoppingCartVM = new ShoppingCartVM()
        {
            ListCart = _context.ShoppingCarts.Include(u => u.Product)
                        .Where(u => u.ApplicationUserId == userId && selectedCartIds.Contains(u.Id)).ToList(),
            OrderHeader = new OrderHeader()
        };

        decimal orderTotal = 0;
        foreach (var item in shoppingCartVM.ListCart)
        {
            orderTotal += item.Quantity * item.Product.Price;
        }
        shoppingCartVM.OrderHeader.OrderTotal = orderTotal;

        return View(shoppingCartVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Summary(ShoppingCartVM shoppingCartVM)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return BadRequest();
        }
        var selectedIdsJson = HttpContext.Session.GetString("SelectedCartIds");
        if (string.IsNullOrEmpty(selectedIdsJson)) return RedirectToAction(nameof(Index));
        var selectedCartIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(selectedIdsJson) ?? new List<int>();
        var cartItems = _context.ShoppingCarts.Include(u => u.Product)
                        .Where(u => u.ApplicationUserId == userId && selectedCartIds.Contains(u.Id)).ToList();

        shoppingCartVM.OrderHeader.ApplicationUserId = userId;
        shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
        shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
        shoppingCartVM.OrderHeader.PaymentStatus = SD.StatusPending;

        decimal orderTotal = 0;
        foreach (var item in cartItems)
        {
            orderTotal += (item.Quantity * item.Product.Price);
        }
        shoppingCartVM.OrderHeader.OrderTotal = orderTotal;

        _context.OrderHeaders.Add(shoppingCartVM.OrderHeader);
        _context.SaveChanges();

        foreach (var item in cartItems)
        {
            var orderDetail = new OrderDetail()
            {
                ProductId = item.ProductId,
                OrderHeaderId = shoppingCartVM.OrderHeader.Id,
                Price = item.Product.Price,
                Count = item.Quantity,
            };
            _context.OrderDetails.Add(orderDetail);
        }
        _context.SaveChanges();
        _context.ShoppingCarts.RemoveRange(cartItems);
        _context.SaveChanges();
        HttpContext.Session.Remove("SelectedCartIds");

        return RedirectToAction("Payment", new { id = shoppingCartVM.OrderHeader.Id });
    }

    [HttpGet]
    public IActionResult OrderConfirmation(int id)
    {
        var orderHeader = _context.OrderHeaders
            .FirstOrDefault(o => o.Id == id);
        if (orderHeader == null)
        {
            return NotFound();
        }
        return View(orderHeader);
    }

    [HttpGet]
    public IActionResult Payment(int id)
    {
        // Cari pesanan berdasarkan ID yang dilempar dari Summary
        var orderHeader = _context.OrderHeaders.FirstOrDefault(u => u.Id == id);
        if (orderHeader == null)
        {
            return NotFound();
        }
        return View(orderHeader);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Payment(int id, IFormFile file, [FromServices] IWebHostEnvironment webHostEnvironment)
    {
        var orderHeader = _context.OrderHeaders.FirstOrDefault(u => u.Id == id);
        if (orderHeader == null) return NotFound();
        if (file != null && file.Length > 0)
        {
            string wwwRootPath = webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string receiptPath = Path.Combine(wwwRootPath, @"images\receipts");

            using (var fileStream = new FileStream(Path.Combine(receiptPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            orderHeader.PaymentReceiptUrl = @"\images\receipts\" + fileName;
            orderHeader.PaymentStatus = "Menunggu Konfirmasi Admin";

            _context.SaveChanges();
        }
        return RedirectToAction("OrderConfirmation", new { id = id });
    }
}