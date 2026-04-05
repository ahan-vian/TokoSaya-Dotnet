using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TokoSaya.Data;
using TokoSaya.Models;
using TokoSaya.ViewModels;

namespace TokoSaya.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Product> objProductList = _context.Products.Include(u => u.Category).ToList();
        return View(objProductList);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ProductVM productVM = new ProductVM
        {
            CategoryList = _context.Categories.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            Product = new Product()

        };
        return View(productVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProductVM productVM, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            if (file != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, "images","products");

                if (Directory.Exists(productPath))
                {
                    Directory.CreateDirectory(productPath);
                }

                using (var filestream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }
                productVM.Product.ImageUrl = "/images/products" + filename;
            }
            _context.Products.Add(productVM.Product);
            _context.SaveChanges();
        }
        productVM.CategoryList = _context.Categories.Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString()
        });

        return View(productVM);
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var productDb = _context.Products.Find(id);
        if (productDb == null)
        {
            return NotFound();
        }
        ProductVM productVM = new ProductVM
        {
            CategoryList = _context.Categories.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            Product = new Product()

        };
        return View(productVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public IActionResult Edit(ProductVM productVM, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            var productFromDb = _context.Products.AsNoTracking()
                .FirstOrDefault(u => u.Id == productVM.Product.Id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            if (file != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, "images", "product");
                if (!Directory.Exists(productPath))
                {
                    Directory.CreateDirectory(productPath);
                }
                if (!string.IsNullOrEmpty(productFromDb.ImageUrl))
                {
                    string oldImagePath = Path.Combine(
                        wwwRootPath,
                        productFromDb.ImageUrl.TrimStart('\\', '/').Replace("/", Path.DirectorySeparatorChar.ToString())
                    );
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                productVM.Product.ImageUrl = "/images/product/" + fileName;
            }
            else
            {
                productVM.Product.ImageUrl = productFromDb.ImageUrl;
            }

            _context.Products.Update(productVM.Product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        productVM.CategoryList = _context.Categories.Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString()
        });
        return View(productVM);
    }
    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var productFromDb = _context.Products
            .Include(u => u.Category)
            .FirstOrDefault(u => u.Id == id);

        if (productFromDb == null)
        {
            return NotFound();
        }

        ProductVM productVM = new ProductVM
        {
            Product = productFromDb,
            CategoryList = _context.Categories.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            })
        };

        return View(productVM);
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

        var productFromDb = _context.Products.Find(id);

        if (productFromDb == null)
        {
            return NotFound();
        }
        if (!string.IsNullOrEmpty(productFromDb.ImageUrl))
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(
                wwwRootPath,
                productFromDb.ImageUrl.TrimStart('\\', '/').Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        _context.Products.Remove(productFromDb);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}