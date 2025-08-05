using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using PartPriceChecker.Services;
using Xunit;

namespace PartPriceChecker.Tests.Services;

using MockServerFactory = WebApplicationFactory<PartPriceChecker.MockServer.Program>;

public class HttpApiServiceTests : IClassFixture<WebApplicationFactory<PartPriceChecker.MockServer.Program>>
{

    private readonly MockServerFactory _factory;
    private readonly HttpClient _client;

    public HttpApiServiceTests(MockServerFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetPartPriceAsync_ValidPartNumber_ReturnsCorrectData()
    {
        var service = new HttpApiService(_client, "");

        var result = await service.GetPartPriceAsync("W10295370");

        Assert.NotNull(result);
        Assert.Equal("W10295370", result.PartNumber);
        Assert.Equal("W10295370", result.KnownPartNumber);
        Assert.Equal("Marcone", result.Provider);
        Assert.Equal(43.25m, result.Price);
        Assert.Equal("USD", result.Currency);
        Assert.Equal("in_stock", result.Availability);
    }

    [Fact]
    public async Task GetPartPriceAsync_AnotherValidPart_ReturnsCorrectData()
    {
        var service = new HttpApiService(_client, "");

        var result = await service.GetPartPriceAsync("DC97-17345B");

        Assert.NotNull(result);
        Assert.Equal("DC97-17345B", result.PartNumber);
        Assert.Equal("Encompass", result.Provider);
        Assert.Equal(38.99m, result.Price);
        Assert.Equal("limited", result.Availability);
    }

    [Fact]
    public async Task GetPartPriceAsync_NotFoundPart_ReturnsPartNumberOnly()
    {
        var service = new HttpApiService(_client, "");

        var result = await service.GetPartPriceAsync("NOTFOUND123");

        Assert.NotNull(result);
        Assert.Equal("NOTFOUND123", result.PartNumber);
    }

    [Fact]
    public async Task GetPartPriceAsync_ServerError_ReturnsErrorResponse()
    {
        var service = new HttpApiService(_client, "");

        var result = await service.GetPartPriceAsync("TEST_500_ERROR");

        Assert.NotNull(result);
        Assert.Equal("TEST_500_ERROR", result.PartNumber);
        Assert.Equal("error", result.Availability);
    }

    [Fact]
    public async Task GetPartPriceAsync_EmptyPartNumber_ReturnsErrorResponse()
    {
        var service = new HttpApiService(_client, "");

        var result = await service.GetPartPriceAsync("");

        Assert.NotNull(result);
        Assert.Equal("", result.PartNumber);
        Assert.Equal("error", result.Availability);
    }
}
