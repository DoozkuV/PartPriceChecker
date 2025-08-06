using System.Net;
using System.Text.Json;
using PartPriceChecker.Models;

namespace PartPriceChecker.Services;


public class HttpApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public HttpApiService(HttpClient httpClient, string url)
    {
        _httpClient = httpClient;
        _baseUrl = url;
    }


    public async Task<PartResponse> GetPartPriceAsync(string partNumber)
    {
        var url = $"{_baseUrl}/api/procurement/price?partNumber={Uri.EscapeDataString(partNumber)}";
        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new PartResponse { PartNumber = partNumber };
        }
        else if (!response.IsSuccessStatusCode)
        {
            return new PartResponse
            {
                PartNumber = partNumber,
                Availability = "error"
            };

        }

        response.EnsureSuccessStatusCode();
        var jsonContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PartResponse>(jsonContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ??
        new PartResponse
        {
            PartNumber = partNumber,
            Availability = "error"
        };
    }
}
