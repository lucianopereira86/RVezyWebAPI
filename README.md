# RVezyWebAPI

## Tech Stack
- Visual Studio 2019
- .NET Core 5.0
- EF Core 6
- SQl Server
- AutoMapper
- xUnit
- Serilog
- CsvHelper
- Swashbuckle
- FluentAssertions
- Moq

## Solution
![solution](/docs/solution.JPG)

The solution contains 5 projects:
- *0 - Common/RVezy.Common.DI:* dependency injection.
- *1 - Presentation/RVezy.Presentation.WebAPI:* controllers.
- *2 - Domain/RVezy.Domain.Domain:* intermediate entities, interfaces and models.
- *3 - Infra/RVezy.Infra.Infra:* database entities and repositories.
- *4 - Tests/RVezy.Tests:* unity tests and CSV files.

## Web API
![swagger](/docs/swagger.JPG)

- In order to run the web API, a SQL Server connection string must be set in the _appsettings.json_ file:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server={SERVER},{PORT};Database={DATABASE};User Id={USER_ID};Password={PASSWORD};"
  },
  ...
}
```

- The database will be created through migration once the web API runs, but the data will be inserted only after the CSV files be uploaded on the Swagger page.
- The CSV files are located at: "4 - Tests\RVezy.Tests\Files".
- Each file has its own endpoint:
    - "listings.csv": POST ​/Listing​/upload
    - "calendars.csv": POST ​/Calendar/upload
    - "reviews.csv": POST ​/Review/upload
- The "listings.csv" file must be the first one to be uploaded.
- The data extracted from the files will be displayed through the GET endpoints.

## Unit Tests
![tests](/docs/tests.JPG)

- No configuration is needed before running the tests (neither a SQL Server connection).
- There are 3 scenarios tested for each entity:
    - GET endpoints from controllers with mocked repositories; 
    - Queries from repositories with In-Memory Database;
    - POST endpoints from controllers with CSV files uploaded dynamically.
