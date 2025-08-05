using System.Net;
using System.Text.Json;
using PartPriceChecker.Models;

namespace PartPriceChecker.Services;


public class HttpApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://example.com/api/procurement/price";

    public HttpApiService(HttpClient httpClient, string url)
    {
        _httpClient = httpClient;
        _baseUrl = url;
    }

    public async Task<PartApiResponse> GetPartPriceAsync(string partNumber)
    {
        try
        {
            var url = $"{_baseUrl}?partNumber={Uri.EscapeDataString(partNumber)}";
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new PartApiResponse { PartNumber = partNumber };
            }

            response.EnsureSuccessStatusCode();
            var jsonContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PartApiResponse>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ??
            new PartApiResponse
            {
                PartNumber = partNumber,
                Availability = "error"
            };
        }
        catch (HttpRequestException ex)
        {
            // TODO: Find a better way of managing errors within this function
            Console.WriteLine($"Error fetching price for {partNumber}: {ex.Message}");
            return new PartApiResponse
            {
                PartNumber = partNumber,
                Availability = "error"
            };
        }
    }
}
