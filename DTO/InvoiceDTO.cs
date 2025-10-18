using Tinytots.Models;

namespace Tinytots.DTO;

public class InvoiceDTO
{
    public List<OrderCreateDTO> Items { get; set; } = new();
}