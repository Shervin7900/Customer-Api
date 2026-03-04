using FastEndpoints;
using Customer_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Customer_Api.Endpoints.Customers;

public class DeleteRequest
{
    public int Id { get; set; }
}

public class DeleteCustomerEndpoint : Endpoint<DeleteRequest>
{
    private readonly AppDbContext _db;

    public DeleteCustomerEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Delete("/api/customers/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (customer is null)
        {
            await HttpContext.Response.SendNotFoundAsync(ct);
            return;
        }

        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync(ct);

        await HttpContext.Response.SendNoContentAsync(ct);
    }
}
