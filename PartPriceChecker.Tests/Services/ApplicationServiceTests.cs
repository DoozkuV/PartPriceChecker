using Moq;
using PartPriceChecker.Models;
using PartPriceChecker.Services;
using Xunit;

namespace PartPriceChecker.Tests.Services;

public class ApplicationServiceTests
{
    [Fact]
    public async Task RunAsync_WithNonExistentFile_ReturnsErrorCode()
    {
        var csvService = new Mock<CsvService>();
        var outputService = new Mock<OutputService>();
        var apiService = new Mock<IApiService>();

        var appService = new ApplicationService(csvService.Object, outputService.Object, apiService.Object);

        var result = await appService.RunAsync("nonexistent.csv", null, true);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task RunAsync_WithValidInput_ReturnsSuccess()
    {
        var testFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(testFile, "PartNumber\nTEST123");

        var csvService = new CsvService();
        var outputService = new OutputService();
        var apiService = new Mock<IApiService>();

        apiService.Setup(x => x.GetPartPriceAsync("TEST123"))
            .ReturnsAsync(new PartResponse
            {
                PartNumber = "TEST123",
                Provider = "TestProvider"
            });

        var appService = new ApplicationService(csvService, outputService, apiService.Object);

        var result = await appService.RunAsync(testFile, null, true);

        Assert.Equal(0, result);
        File.Delete(testFile);
    }
}
