using PartPriceChecker.Models;
using PartPriceChecker.Services;
using Xunit;

namespace PartPriceChecker.Tests.Services;

public class CsvServiceTests
{
    [Fact]
    public void ReadPartsFromCsv_ValidFile_ReturnsCorrectData()
    {
        var testFile = Path.GetTempFileName();
        var csvContent = "PartNumber\nTEST123\nTEST456";
        File.WriteAllText(testFile, csvContent);
        try
        {
            var csvService = new CsvService();
            var parts = csvService.ReadPartsFromCsv(testFile);

            Assert.Equal(2, parts.Count);
            Assert.Equal("TEST123", parts[0].PartNumber);
            Assert.Equal("TEST456", parts[1].PartNumber);
        }
        finally
        {
            File.Delete(testFile);
        }
    }

    [Fact]
    public void WriteResultsToCsv_ValidData_CreatesCorrectFile()
    {
        var testFile = Path.GetTempFileName();
        var results = new List<PartResponse>
            {
                new() {
                    PartNumber = "TEST123",
                    Provider = "TestProvider",
                    Price = 10.50m
                }
            };
        var csvService = new CsvService();
        csvService.WriteResultsToCsv(results, testFile);

        var content = File.ReadAllText(testFile);
        Assert.Contains("TEST123", content);
        Assert.Contains("TestProvider", content);

        File.Delete(testFile);
    }
}
