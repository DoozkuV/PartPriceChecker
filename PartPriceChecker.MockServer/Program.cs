using PartPriceChecker.Models;

namespace PartPriceChecker.MockServer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = CreateWebApplication(args);
        var app = builder.Build();

        ConfigureApp(app);

        var port = args.Length > 0 && int.TryParse(args[0], out var p) ? p : 5050;
        app.Urls.Add($"http://localhost:{port}");

        Console.WriteLine($"Mock API Server starting on http://localhost:{port}");
        Console.WriteLine("Available endpoints:");
        Console.WriteLine($"  GET  http://localhost:{port}/health");
        Console.WriteLine($"  GET  http://localhost:{port}/api/procurement/price?partNumber=<part>");
        Console.WriteLine($"  GET  http://localhost:{port}/api/procurement/parts");
        Console.WriteLine($"  POST http://localhost:{port}/api/procurement/parts");
        Console.WriteLine();
        Console.WriteLine("Test part numbers: W10295370, DC97-17345B, WE03X29897");
        Console.WriteLine("Special test cases: TEST_ERROR, TEST_500_ERROR");

        app.Run();
    }

    public static WebApplicationBuilder CreateWebApplication(string[]? args = null)
    {
        var builder = WebApplication.CreateBuilder(args ?? Array.Empty<string>());
        builder.Services.AddCors();
        return builder;
    }

    public static void ConfigureApp(WebApplication app)
    {
        app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        var mockData = GetMockData();

        app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

        app.MapGet("/api/procurement/price", (string partNumber) =>
        {
            if (string.IsNullOrWhiteSpace(partNumber))
            {
                return Results.BadRequest(new { error = "partNumber is required" });
            }

            if (partNumber == "TEST_500_ERROR")
            {
                return Results.Problem("Internal server error", statusCode: 500);
            }

            if (mockData.TryGetValue(partNumber, out var part))
            {
                return Results.Ok(part);
            }

            return Results.NotFound(new { error = "Part not found", partNumber });
        });

        app.MapPost("/api/procurement/parts", () =>
        {
            var allParts = mockData.Values.ToList();
            return Results.Ok(allParts);
        });

        app.MapGet("/api/procurement/parts", () =>
        {
            var partNumbers = mockData.Keys.ToArray();
            return Results.Ok(new { availableParts = partNumbers, count = partNumbers.Length });
        });
    }

    private static Dictionary<string, PartResponse> GetMockData() =>
        new Dictionary<string, PartResponse>
        {
            ["W10295370"] = new()
            {
                PartNumber = "W10295370",
                KnownPartNumber = "W10295370",
                Provider = "Marcone",
                Price = 43.25m,
                Currency = "USD",
                Availability = "in_stock"
            },
            ["DC97-17345B"] = new()
            {
                PartNumber = "DC97-17345B",
                KnownPartNumber = "DC97-17345B",
                Provider = "Encompass",
                Price = 38.99m,
                Currency = "USD",
                Availability = "limited"
            },
            ["WE03X29897"] = new()
            {
                PartNumber = "WE03X29897",
                KnownPartNumber = "WE03X29897",
                Provider = "GE Parts",
                Price = 24.50m,
                Currency = "USD",
                Availability = "in_stock"
            },
            ["TEST_ERROR"] = new()
            {
                PartNumber = "TEST_ERROR",
                KnownPartNumber = "",
                Provider = "ErrorProvider",
                Price = null,
                Currency = "USD",
                Availability = "error"
            }
        };
}
