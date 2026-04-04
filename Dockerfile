# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first for better layer caching
COPY *.csproj ./
COPY modules/Base-Api/BaseApi/BaseApi.csproj ./modules/Base-Api/BaseApi/

# Restore dependencies
RUN dotnet restore "Customer-Api.csproj"

# Copy all remaining source
COPY . .

# Build and publish
RUN dotnet publish "Customer-Api.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Use built-in non-root user for security
USER app

# Copy artifacts
COPY --from=build /app/publish .

# Environment variables
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "Customer-Api.dll"]
