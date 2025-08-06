# Part Price Checker

Queries a simple API based on a parts.csv file and outputs a CSV fetching the pricing and availability of the parts. Includes also a mock web-server for testing the application as well as full Unit Tests.

## Usage

```
  input (pos. 0)    Required. Input CSV file containing part numbers
  -o, --output      Output CSV file path (supresses console table)

  -u, --url         (Default: http://localhost:5050) Url for API to be hit to receive part
                    data

  -q, --quiet       (Default: false) Supress all console output except errors

  -m, --mock        (Default: false) Run the program using mock data

  --help            Display this help screen.

  --version         Display version information.

```
## Spec

Task: Build a Console App to Query Part Prices from Sample API

Goal: Create a simple .NET console app that reads a CSV file containing part numbers, calls a REST API to fetch pricing and availability, and outputs the results in either the console (table format) or a new CSV file.
Sample Input File: parts.csv

```
PartNumber
W10295370
DC97-17345B
5304511966
W10165295RP
WE03X29897
WPW10196405
MEE61841401
134937300
ADQ73334008
WB06X10943
```

Expected Output Format (Console or CSV):

PartNumber     | KnownPartNumber | Provider  | Price   | Currency | Availability
---------------|------------------|-----------|---------|----------|--------------
W10295370      | W10295370        | Marcone   | 43.25   | USD      | in_stock
DC97-17345B    | DC97-17345B      | Encompass | 38.99   | USD      | limited
5304511966     |                  | N/A       | N/A     | N/A      | out_of_stock
...

API Endpoint:

GET https://example.com/api/procurement/price

Query Parameter:

- `partNumber` (string, required) – The part number to query

Response (200):
```
{
  "partNumber": "W10295370",
  "knownPartNumber": "W10295370",
  "provider": "Marcone",
  "price": 43.25,
  "currency": "USD",
  "availability": "in_stock"
}
```

Error responses:

- 400 Bad Request
- 404 Part not found

Deliverables

- .NET 7+ console app in C#
- Reads from parts.csv
- Outputs results to console table or output.csv
- Includes basic error handling for failed lookups


