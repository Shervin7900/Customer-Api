using FastEndpoints;
using Customer_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Customer_Api.Endpoints.Customers;

public class GetAllResponse
{
    public IEnumerable<CustomerResponse> Customers { get; set; } = default!;
}

public class CustomerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class GetAllCustomersEndpoint : EndpointWithoutRequest<GetAllResponse>
{
    private readonly AppDbContext _db;

    public GetAllCustomersEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/api/customers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var customers = await _db.Customers
            .AsNoTracking()
            .Select(c => new CustomerResponse
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email
            })
            .ToListAsync(ct);

        await HttpContext.Response.SendAsync(new GetAllResponse { Customers = customers }, 200, null, ct);
    }
}
