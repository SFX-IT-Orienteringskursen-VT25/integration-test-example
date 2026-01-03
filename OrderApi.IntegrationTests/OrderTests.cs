using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace OrderApi.IntegrationTests;

public class OrderTests(IntegrationTestFixture fixture) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task GivenNonExistentOrder_WhenGettingOrder_ThenNotFound()
    {
        // Arrange
        fixture.GetDatabase().DeleteAll();
        var client = fixture.WebApplicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/order/1");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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

    [Fact]
    public async Task GivenExistingOrder_WhenGettingOrder_ThenOrderShouldBeReturnedSuccessfully()
    {
        // Arrange
        var item = "Apple iPhone";
        var quantity = 2;
        var client = fixture.WebApplicationFactory.CreateClient();
        var id = fixture.GetDatabase().InsertOrder(item, quantity);

        // Act
        var order = await client.GetFromJsonAsync<Order>($"/order/{id}");

        // Assert
        Assert.Equal(new Order(id, item, quantity), order);
    }
}