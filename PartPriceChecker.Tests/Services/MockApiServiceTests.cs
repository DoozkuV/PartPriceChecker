using PartPriceChecker.Services;
using Xunit;

namespace PartPriceChecker.Tests.Services;

public class MockApiServiceTests
{
    [Fact]
    public async Task GetPartPriceAsync_KnownPart_ReturnsCorrectData()
    {
        var service = new MockApiService();

        var result = await service.GetPartPriceAsync("W10295370");

        Assert.NotNull(result);
        Assert.Equal("W10295370", result.PartNumber);
        Assert.Equal("Marcone", result.Provider);
        Assert.Equal(43.25m, result.Price);
    }

    [Fact]
    public async Task GetPartPriceAsync_UnknownPart_ReturnsOutOfStock()
    {
        var service = new MockApiService();

        var result = await service.GetPartPriceAsync("UNKNOWN123");

        Assert.NotNull(result);
        Assert.Equal("UNKNOWN123", result.PartNumber);
        Assert.Equal("N/A", result.Provider);
        Assert.Equal("out_of_stock", result.Availability);
    }
}
