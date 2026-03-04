# Customer-Api

A high-performance Customer Management Microservice built with .NET 10, featuring REST and gRPC interfaces, backed by SQL Server, and secured with Identity Server.

## 🚀 Features

- **Fast Endpoints**: Modern, high-performance REPR (Request-Endpoint-Response) pattern for RESTful APIs.
- **gRPC Service**: Efficient, contract-first communication for internal services.
- **Entity Framework Core**: Robust data access layer with SQL Server integration.
- **Duende IdentityServer**: Fully integrated authentication and authorization.
- **.NET 10**: Built on the latest cutting-edge .NET platform.

## 🏗️ Architecture

- **REST Layer**: Uses `FastEndpoints` for streamlined API development.
- **gRPC Layer**: Implements `CustomerService` defined in `customer.proto`.
- **Identity Layer**: Configures Duende IdentityServer with in-memory clients and scopes.
- **Data Layer**: SQL Server integration using `AppDbContext` and the `Customer` entity.

## 🛠️ Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server (LocalDB instance `(localdb)\mssqllocaldb` used by default)

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Shervin7900/Customer-Api.git
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

## 📖 API Documentation

### REST Endpoints

- `GET /api/customers` - List all customers
- `GET /api/customers/{id}` - Get a customer by ID
- `POST /api/customers` - Create a new customer
- `PUT /api/customers/{id}` - Update an existing customer
- `DELETE /api/customers/{id}` - Remove a customer

### gRPC Service

- Service Name: `customer.CustomerService`
- Definition: `Protos/customer.proto`

### Identity Server

- Discovery: `/.well-known/openid-configuration`
- Default Client: `client`
- Default Secret: `secret`
- Allowed Scopes: `customer_api`

## 📄 License

This project is licensed under the MIT License.
