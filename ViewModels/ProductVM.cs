using Microsoft.AspNetCore.Mvc.Rendering;
using TokoSaya.Models;

namespace TokoSaya.ViewModels;

public class ProductVM
{
    public Product Product {get; set;} = new Product();
    public IEnumerable<SelectListItem> CategoryList {get; set;} = new List<SelectListItem>();
}