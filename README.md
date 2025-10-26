run by typing

1. dotnet restore // this will download dependencies
2. dotnet build // builds/compiles the application
3. dotnet test // executes the test suite

Should result in something like

>dotnet test

Restore complete (0,3s)
  OrderApi succeeded (0,2s) → OrderApi\bin\Debug\net9.0\OrderApi.dll
  OrderApi.IntegrationTests succeeded (0,1s) → OrderApi.IntegrationTests\bin\Debug\net9.0\OrderApi.IntegrationTests.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.8.2+699d445a1a (64-bit .NET 9.0.10)
[xUnit.net 00:00:00.54]   Discovering: OrderApi.IntegrationTests
[xUnit.net 00:00:00.56]   Discovered:  OrderApi.IntegrationTests
[xUnit.net 00:00:00.57]   Starting:    OrderApi.IntegrationTests
[xUnit.net 00:00:07.48]   Finished:    OrderApi.IntegrationTests
  OrderApi.IntegrationTests test succeeded (8,1s)

Test summary: total: 3; failed: 0; succeeded: 3; skipped: 0; duration: 8,1s
Build succeeded in 8,9s
