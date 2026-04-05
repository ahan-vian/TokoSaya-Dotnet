using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TokoSaya.Models;

public class ShoppingCart
{
    public int Id {get; set;}
    public int Quantity {get; set;}
    public int ProductId {get; set;}
    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product {get;set;} = null!;

    public string ApplicationUserId {get; set;} = null!;
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser {get; set;} = null!;
}