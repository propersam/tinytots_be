using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Tinytots.Enums;

namespace Tinytots.Models;

public class Invoice
{ 
    public int Id { get; set; }
    
    [MaxLength(Int32.MaxValue)]
    public string? Code { get; set; }
    
    [Precision(9,2)] 
    public decimal Price { get; set; } 
    public StatusEnum Status { get; set; }
    
    public DateTimeOffset CreatedAt { get; init; }
    
}