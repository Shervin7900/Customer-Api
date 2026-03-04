using FastEndpoints;
using Customer_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Customer_Api.Endpoints.Customers;

public class GetRequest
{
    public int Id { get; set; }
}

public class GetResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class GetCustomerEndpoint : Endpoint<GetRequest, GetResponse>
{
    private readonly AppDbContext _db;

    public GetCustomerEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/api/customers/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRequest req, CancellationToken ct)
    {
        var customer = await _db.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (customer is null)
        {
            await HttpContext.Response.SendNotFoundAsync(ct);
            return;
        }

        await HttpContext.Response.SendAsync(new GetResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        }, 200, null, ct);
    }
}
