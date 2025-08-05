using CommandLine;

namespace PartPriceChecker.Options;

public class CliOptions
{
    [Value(0, MetaName = "input", Required = true,
            HelpText = "Input CSV file containing part numbers")]
    public string InputFile { get; set; } = string.Empty;

    [Option('o', "output", Required = false,
            HelpText = "Output CSV file path (supresses console table)")]
    public string? OutputFile { get; set; }

    [Option('u', "url", Required = false,
            Default = "https://example.com/api/procurement/price",
            HelpText = "Url for API to be hit to receive part data")]
    public string Url { get; set; } = string.Empty;

    [Option('q', "quiet", Required = false, Default = false,
            HelpText = "Supress all console output except errors")]
    public bool Quiet { get; set; }

    [Option('m', "mock", Required = false, Default = false,
            HelpText = "Run the program using mock data")]
    public bool Mock { get; set; }
};
