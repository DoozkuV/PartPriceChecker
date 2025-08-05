using ConsoleTables;
using PartPriceChecker.Models;

namespace PartPriceChecker.Services;

public class OutputService
{
    public void DisplayResultsInConsole(List<PartResponse> results)
    {
        var table = new ConsoleTable(
                "PartNumber",
                "KnownPartNumber",
                "Provider",
                "Price",
                "Currency",
                "Availability"
            );


        foreach (var result in results)
        {
            table.AddRow(
                result.PartNumber,
                result.KnownPartNumber,
                result.Provider,
                result.Price?.ToString("F2") ?? "N/A",
                result.Currency,
                result.Availability
            );
        }

        table.Write();
    }
}
