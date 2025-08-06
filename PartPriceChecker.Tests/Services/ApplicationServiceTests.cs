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

        try
        {
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
        }
        finally
        {
            File.Delete(testFile);
        }
    }

    [Fact]
    public async Task RunAsync_WithValidInputAndQuiet_HasNoOutput()
    {

        var stringWriter = new StringWriter();
        var originalConsoleOut = Console.Out;
        var testFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(testFile, "PartNumber\nTEST123");

        try
        {
            // Capture Console.Out
            Console.SetOut(stringWriter);

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
            Assert.Empty(stringWriter.ToString());
        }
        finally
        {
            // Restore console state
            Console.SetOut(originalConsoleOut);
            File.Delete(testFile);
        }

    }

    [Fact]
    public async Task RunAsync_WithInvalidHttpAddress_ReturnsErrorCode()
    {
        var testFile = Path.GetTempFileName();
        var csvContent = "PartNumber\nTEST123\nTEST456";
        File.WriteAllText(testFile, csvContent);

        try
        {
            var csvService = new CsvService();
            var outputService = new Mock<OutputService>();
            var apiService = new HttpApiService(new HttpClient(), "nonsense");

            var appService = new ApplicationService(csvService, outputService.Object, apiService);

            var result = await appService.RunAsync(testFile, null, true);

            Assert.Equal(1, result);
        }
        finally
        {
            File.Delete(testFile);
        }
    }

    [Fact]
    public async Task RunAsync_WithNonExistantHttpAddress_ReturnsErrorCode()
    {
        var testFile = Path.GetTempFileName();
        var csvContent = "PartNumber\nTEST123\nTEST456";
        File.WriteAllText(testFile, csvContent);

        try
        {
            var csvService = new CsvService();
            var outputService = new Mock<OutputService>();
            var apiService = new HttpApiService(new HttpClient(), "http://localhost:9999");

            var appService = new ApplicationService(csvService, outputService.Object, apiService);

            var result = await appService.RunAsync(testFile, null, true);

            Assert.Equal(1, result);
        }
        finally
        {
            File.Delete(testFile);
        }
    }
}
