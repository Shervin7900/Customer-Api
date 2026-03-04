using Grpc.Core;
using Customer_Api.Data;
using Microsoft.EntityFrameworkCore;
using Customer_Api.Models;
using Customer_Api.Grpc;

namespace Customer_Api.Services;

public class CustomerGrpcService : CustomerService.CustomerServiceBase
{
    private readonly AppDbContext _db;

    public CustomerGrpcService(AppDbContext db)
    {
        _db = db;
    }

    public override async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email
        };

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();

        return new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        };
    }

    public override async Task<CustomerResponse> GetCustomer(GetCustomerRequest request, ServerCallContext context)
    {
        var customer = await _db.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id);

        if (customer is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Customer with ID {request.Id} not found"));
        }

        return new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        };
    }

    public override async Task<CustomerListResponse> GetAllCustomers(Customer_Api.Grpc.Empty request, ServerCallContext context)
    {
        var customers = await _db.Customers.AsNoTracking().ToListAsync();
        var response = new CustomerListResponse();
        
        response.Customers.AddRange(customers.Select(c => new CustomerResponse
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email
        }));

        return response;
    }

    public override async Task<Customer_Api.Grpc.Empty> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (customer is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Customer with ID {request.Id} not found"));
        }

        customer.Name = request.Name;
        customer.Email = request.Email;

        await _db.SaveChangesAsync();

        return new Customer_Api.Grpc.Empty();
    }

    public override async Task<Customer_Api.Grpc.Empty> DeleteCustomer(DeleteCustomerRequest request, ServerCallContext context)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (customer is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Customer with ID {request.Id} not found"));
        }

        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync();

        return new Customer_Api.Grpc.Empty();
    }
}
