using OrderApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () =>
{
    return "Hello World!";
});

app.MapGet("/order/{id}", ([FromRoute] int id) =>
{
    return Results.Ok(new Order(id, "Sample Item", 1));
});

app.MapPost("/order", ([FromBody] Order order) =>
{
    if (order.Quantity < 1)
    {
        return Results.BadRequest("Must provide a valid quantity");
    }

    return Results.Ok("Order received");
});

app.MapPut("/order", ([FromBody] Order order) =>
{
    return Results.Ok("Order has been updated");
});

app.MapDelete("/order", ([FromBody] Order order) => Results.NoContent());

app.Run();

namespace OrderApi
{
    public class Program;
}