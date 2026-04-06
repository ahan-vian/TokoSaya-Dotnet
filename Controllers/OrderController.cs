using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokoSaya.Data;
using TokoSaya.ViewModels;

namespace TokoSaya.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null || userId == "0")
        {
            return BadRequest();
        }
        var orderHeaders = _context.OrderHeaders.Where(u => u.ApplicationUserId == userId).ToList();
        return View(orderHeaders);
    }

    // [HttpGet]
    // public IActionResult Details(int id)
    // {
    //     OrderVM orderVM = new OrderVM()
    //     {
    //         OrderHeader = _context.OrderHeaders.FirstOrDefault(u => u.Id == id)!,
    //         OrderDetail = _context.OrderDetails.Include(u => u.Product).Where(u => u.OrderHeaderId == id).ToList()
    //     };
    //     if (orderVM.OrderHeader == null)
    //     {
    //         return NotFound();
    //     }
    //     return View(orderVM);
    // }
    [HttpGet]
    public IActionResult Details(int id)
    {
        OrderVM orderVM = new OrderVM()
        {
            OrderHeader = _context.OrderHeaders.FirstOrDefault(u => u.Id == id)!,
            OrderDetail = _context.OrderDetails.Include(u => u.Product).Where(u => u.OrderHeaderId == id).ToList()
        };

        if (orderVM.OrderHeader == null)
        {
            return NotFound();
        }

        return View(orderVM);
    }
}