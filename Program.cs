using FastEndpoints;
using Customer_Api.Data;
using Microsoft.EntityFrameworkCore;
using Customer_Api.Services;
using Customer_Api.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Server=(localdb)\\mssqllocaldb;Database=CustomerApiDb;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Fast Endpoints
builder.Services.AddFastEndpoints();

// gRPC
builder.Services.AddGrpc();

// Identity Server
builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(Customer_Api.Identity.Config.IdentityResources)
    .AddInMemoryApiScopes(Customer_Api.Identity.Config.ApiScopes)
    .AddInMemoryClients(Customer_Api.Identity.Config.Clients)
    .AddDeveloperSigningCredential();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:5000"; // Assuming the API runs on 5000
        options.RequireHttpsMetadata = false;
        options.Audience = "customer_api";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

// Register gRPC services
app.MapGrpcService<CustomerGrpcService>();

app.MapGet("/", () => "Customer API is running. Access /api/customers for REST or use gRPC.");

// Ensure database is created (simple approach for demo/new project)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
