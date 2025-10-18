using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Tinytots.Enums;
using Tinytots.Services;

namespace Tinytots.Models;

public class Invoice
{ 
    [Key] public int InvId { get; init; }

    [MaxLength(20)] public string InvCode { get; set; } = null!;
    
    [Precision(20,2)] 
    public decimal InvoiceBill { get; init; } 
    public StatusEnum Status { get; set; }
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public DateTimeOffset CreatedAt  = DateTimeOffset.UtcNow;
}