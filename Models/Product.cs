using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TokoSaya.Models;

public class Product
{
    public int Id {get; set;}
    public string Name {get; set;} = null!;
    public string Description {get; set;} = null!;
    public decimal Price{get; set;}
    [ValidateNever]
    public string ImageUrl {get; set;} = null!;
    public int CategoryId{get; set;}

    [ValidateNever]
    public Category Category {get; set;} = null!;
}