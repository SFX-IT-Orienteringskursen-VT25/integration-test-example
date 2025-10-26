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
    var order = Database.Select(id);
    if (order is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(order);
});

app.MapPost("/order", ([FromBody] Order order) =>
{
    if (order.Quantity < 1)
    {
        return Results.BadRequest("Must provide a valid quantity");
    }

    var id = Database.InsertOrder(order.Item, order.Quantity);

    return Results.Ok(Database.Select(id));
});

app.Run();

namespace OrderApi
{
    public class Program;
}