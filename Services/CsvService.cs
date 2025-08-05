using System.Globalization;
using CsvHelper;
using PartPriceChecker.Models;

namespace PartPriceChecker.Services;

public class CsvService
{
    public List<PartInput> ReadPartsFromCsv(string filePath)
    {
        using var reader = new StringReader(File.ReadAllText(filePath));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<PartInput>().ToList();
    }

    public void WriteResultsToCsv(List<PartApiResponse> results, string filePath)
    {
        using var writer = new StringWriter();
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteRecords(results);
        File.WriteAllText(filePath, writer.ToString());
    }
}
