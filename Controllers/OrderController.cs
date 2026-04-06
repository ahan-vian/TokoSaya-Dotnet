using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokoSaya.Data;

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
        if(userId == null || userId == "0")
        {
            return BadRequest();
        }
        var orderHeaders =_context.OrderHeaders.Where(u=> u.ApplicationUserId == userId).ToList();
        return View(orderHeaders);
    }
}