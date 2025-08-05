namespace PartPriceChecker.Models;

public class PartApiResponse
{
    public string PartNumber { get; set; } = string.Empty;
    public string KnownPartNumber { get; set; } = "";
    public string Provider { get; set; } = "N/A";
    public decimal? Price { get; set; } = null;
    public string Currency { get; set; } = "N/A";
    public string Availability { get; set; } = "out_of_stock";
}

public class PartInput
{
    public string PartNumber { get; set; } = string.Empty;
}
