using FastEndpoints;
using Customer_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Customer_Api.Endpoints.Customers;

public class UpdateRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class UpdateCustomerEndpoint : Endpoint<UpdateRequest>
{
    private readonly AppDbContext _db;

    public UpdateCustomerEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Put("/api/customers/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateRequest req, CancellationToken ct)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (customer is null)
        {
            await HttpContext.Response.SendNotFoundAsync(ct);
            return;
        }

        customer.Name = req.Name;
        customer.Email = req.Email;

        await _db.SaveChangesAsync(ct);

        await HttpContext.Response.SendOkAsync(ct);
    }
}
