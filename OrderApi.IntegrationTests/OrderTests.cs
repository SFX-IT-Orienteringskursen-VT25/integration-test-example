using System.Net;
using System.Net.Http.Json;

namespace OrderApi.IntegrationTests;

public class OrderTests(IntegrationTestFixture fixture) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task GivenOrderExists_WhenGettingOrder_ThenOrderShouldBeReturnedSuccessfully()
    {
        // Arrange
        var client = fixture.WebApplicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/order/1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GivenOrderHasInvalidQuantity_WhenPlacingOrder_ThenBadRequestShouldBeReturned()
    {
        // Arrange
        int invalidQuantity = -1;

        // Act
        var client = fixture.WebApplicationFactory.CreateClient();
        var response = await client.PostAsJsonAsync("/order", new Order(1, "Apple iPhone", invalidQuantity));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}