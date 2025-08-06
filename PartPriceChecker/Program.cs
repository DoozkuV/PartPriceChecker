using CommandLine;
using PartPriceChecker.Options;
using PartPriceChecker.Services;

return await Parser.Default.ParseArguments<CliOptions>(args)
    .MapResult(
            async options => await RunWithOptions(options),
            errors => Task.FromResult(1)
        );

static async Task<int> RunWithOptions(CliOptions options)
{
    var csvService = new CsvService();
    var outputService = new OutputService();
    IApiService apiService = options.Mock ?
        new MockApiService() :
        new HttpApiService(new HttpClient(), options.Url);

    var appService = new ApplicationService(csvService, outputService, apiService);

    return await appService.RunAsync(
            options.InputFile,
            options.OutputFile,
            options.Quiet
        );
}
