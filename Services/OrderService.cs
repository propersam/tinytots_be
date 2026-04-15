
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Tinytots.DbContext;



namespace Tinytots.Services;

public class OrderService
{
    private readonly TinytotsDbContext _context;

    public OrderService(TinytotsDbContext context)
    {
        _context = context;
    }

    // public async Task<Order> CreateOrder(decimal bill, List<OrderUpdateDTO> order)
    // {
    //     var invoice = new Invoice
    //     {
    //         Total = bill,
    //         Status = StatusEnum.Pending,
    //         InvCode = await UniqueInvoiceCode()
    //     };
    //
    //     _context.Order.Add(Order);
    //     await _context.SaveChangesAsync();
    //
    //     return Order;
    // }

    public async Task<string> UniqueOrderCode()
    {
        string code;
        bool exists;

        do
        {
            string today = DateTime.Now.ToString("yyyyMMdd");
            string randomNum = RandomNumberGenerator.GetInt32(100, 100000).ToString("D5");
            code = $"TTINV-{today}-{randomNum}";
            exists = await _context.Invoices.AnyAsync(i => i.InvCode == code);
        }
        while (exists);

        return code;
    }


}







