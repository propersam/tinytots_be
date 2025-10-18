using Tinytots.Models;

namespace Tinytots.DTO;

public class OrderCreateDTO
{
    public int ProductId { get; set; } = 0;
    public int Quantity { get; set; }
}
