using PartPriceChecker.Models;

namespace PartPriceChecker.Services;

public interface IApiService
{
    Task<PartResponse> GetPartPriceAsync(string partNumber);
}

public interface IApplicationService
{
    Task<int> RunAsync(string inputFile, string? outputFile, bool quiet);
}

public class ApplicationService : IApplicationService
{
    private readonly CsvService _csvService;
    private readonly OutputService _outputService;
    private readonly IApiService _apiService;

    public ApplicationService(CsvService csvService, OutputService outputService,
            IApiService apiService)
    {
        _csvService = csvService;
        _outputService = outputService;
        _apiService = apiService;
    }

    public async Task<int> RunAsync(string inputFile, string? outputFile, bool quiet)
    {
        Action<String> print = quiet ? _ => { } : Console.WriteLine;

        if (!File.Exists(inputFile))
        {
            print($"Input file '{inputFile}' not found.");
            return 1;
        }

        print("Reading parts from CSV...");
        var parts = _csvService.ReadPartsFromCsv(inputFile);
        print($"Found {parts.Count} parts to process.");

        var results = new List<PartResponse>();

        print("Fetching prices...");
        try
        {
            foreach (var part in parts)
            {
                print($"Processing {part.PartNumber}...");

                var result = await _apiService.GetPartPriceAsync(part.PartNumber);
                results.Add(result);
            }
        }
        catch (InvalidOperationException)
        {
            print("Error: Invalid Address");
            return 1;
        }
        catch (HttpRequestException ex)
        {
            print($"Failed to query address: {ex.Message}");
            return 1;
        }

        if (outputFile != null)
        {
            print($"\nSaving results to {outputFile}...");
            _csvService.WriteResultsToCsv(results, outputFile);
            print("Done!");
        }
        else if (!quiet)
        {
            print("\nResults:");
            _outputService.DisplayResultsInConsole(results);
        }
        return 0;
    }
}
