using PartPriceChecker.Models;

namespace PartPriceChecker.Services;

public class MockApiService : IApiService
{
    private readonly Dictionary<string, PartApiResponse> _mockData;

    public MockApiService()
    {
        _mockData = new Dictionary<string, PartApiResponse>
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
            }
        };
    }

    public async Task<PartApiResponse> GetPartPriceAsync(string partNumber)
    {
        await Task.Delay(100); // Simulate API delay

        if (_mockData.TryGetValue(partNumber, out var response))
        {
            return response;
        }

        return new PartApiResponse
        {
            PartNumber = partNumber,
        };
    }
}
