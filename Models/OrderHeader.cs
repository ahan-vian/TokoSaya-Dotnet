using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TokoSaya.Models;

public class OrderHeader
{
    public int Id{get; set;}
    public string ApplicationUserId{get; set;} = null!;
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser{get; set;} = null!;
    public DateTime OrderDate {get; set;}
    public decimal OrderTotal{get; set;}
    public string OrderStatus {get; set;} = null!;
    public string PaymentStatus {get; set;} = null!;
    
    [Required]
    public string Name{get; set;} = null!;
    [Required]
    public string PhoneNumber {get; set;} = null!;
    [Required]
    public string StreetAddress {get; set;} = null!;
    [Required]
    public string City {get; set;} = null!;
    [Required]
    public string State {get; set;} = null!;
    [Required]
    public string PostalCode {get; set;} = null!;


}