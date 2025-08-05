using PartPriceChecker.Models;

namespace PartPriceChecker.Services;

public interface IApiService
{
    Task<PartApiResponse> GetPartPriceAsync(string partNumber);
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

    private bool _quiet;

    public ApplicationService(CsvService csvService, OutputService outputService,
            IApiService apiService)
    {
        _csvService = csvService;
        _outputService = outputService;
        _apiService = apiService;
    }

    public async Task<int> RunAsync(string inputFile, string? outputFile, bool quiet)
    {
        _quiet = quiet;
        if (!File.Exists(inputFile))
        {

            QuietWriteLine($"Input file '{inputFile}' not found.");
            return 1;
        }

        QuietWriteLine("Reading parts from CSV...");
        var parts = _csvService.ReadPartsFromCsv(inputFile);
        QuietWriteLine($"Found {parts.Count} parts to process.");

        var results = new List<PartApiResponse>();

        QuietWriteLine("Fetching prices...");
        foreach (var part in parts)
        {
            QuietWriteLine($"Processing {part.PartNumber}...");

            var result = await _apiService.GetPartPriceAsync(part.PartNumber);
            results.Add(result);
        }

        if (outputFile != null)
        {
            QuietWriteLine($"\nSaving results to {outputFile}...");
            _csvService.WriteResultsToCsv(results, outputFile);
            QuietWriteLine("Done!");
        }
        else if (!_quiet)
        {
            QuietWriteLine("\nResults:");
            _outputService.DisplayResultsInConsole(results);
        }
        return 0;
    }

    private void QuietWriteLine(string? value)
    {
        if (!_quiet) Console.WriteLine(value);
    }
}
