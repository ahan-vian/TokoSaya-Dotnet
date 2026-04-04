

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TokoSaya.Data;
using TokoSaya.Models;

namespace TokoSaya.Controllers;

public class CategoryController : Controller
{
    public readonly ApplicationDbContext _context;
    public CategoryController(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var objCategoryList = _context.Categories.ToList();
        return View(objCategoryList);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if (!ModelState.IsValid)
        {
            return View(obj);
        }
        _context.Categories.Add(obj);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var categoryDb = _context.Categories.Find(id);
        if (categoryDb == null)
        {
            return NotFound();
        }
        return View(categoryDb);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (!ModelState.IsValid)
        {
            return View(obj);
        }
        _context.Categories.Update(obj);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var categoryDb = _context.Categories.Find(id);
        if (categoryDb == null)
        {
            return NotFound();
        }
        return View(categoryDb);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        
        var obj = _context.Categories.Find(id);
        if (obj == null)
        {
            return NotFound();
        }
        _context.Categories.Remove(obj);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}