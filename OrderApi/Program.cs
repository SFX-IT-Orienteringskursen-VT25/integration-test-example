using OrderApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<Database>();

var app = builder.Build();

app.MapGet("/", () =>
{
    return "Hello World!";
});

app.MapGet("/order/{id}", ([FromRoute] int id, [FromServices] Database db) =>
{
    var order = db.Select(id);
    if (order is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(order);
});

app.MapPost("/order", ([FromBody] Order order, [FromServices] Database db) =>
{
    if (order.Quantity < 1)
    {
        return Results.BadRequest("Must provide a valid quantity");
    }

    var id = db.InsertOrder(order.Item, order.Quantity);

    return Results.Ok(db.Select(id));
});

app.Run();

namespace OrderApi
{
    public class Program;
}