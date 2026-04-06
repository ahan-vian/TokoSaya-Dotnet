using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TokoSaya.Models;

public class OrderDetail
{
    public int Id {get; set;}
    public int OrderHeaderId {get; set;}
    [ForeignKey("OrderHeaderId")]
    [ValidateNever]
    public OrderHeader OrderHeader{get; set;}= null!;
    public int ProductId {get; set;}
    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product{get; set;}= null!;

    public int Count {get; set;}
    public decimal Price {get; set;}
    
}