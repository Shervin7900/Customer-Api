using FastEndpoints;
using Customer_Api.Data;
using Customer_Api.Models;

namespace Customer_Api.Endpoints.Customers;

public class CreateRequest
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class CreateResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class CreateCustomerEndpoint : Endpoint<CreateRequest, CreateResponse>
{
    private readonly AppDbContext _db;

    public CreateCustomerEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Post("/api/customers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
    {
        var customer = new Customer
        {
            Name = req.Name,
            Email = req.Email
        };

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync(ct);

        await HttpContext.Response.SendAsync(new CreateResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        }, 200, null, ct);
    }
}
