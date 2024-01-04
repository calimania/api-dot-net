using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CalimaDB>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", async (CalimaDB db) =>
    await db.Calimas.ToListAsync());

app.MapGet("/calimas", async (CalimaDB db) =>
    await db.Calimas.ToListAsync());

app.MapGet("/calimas/complete", async (CalimaDB db) =>
    await db.Calimas.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/calima/{id}", async (int id, CalimaDB db) =>
    await db.Calimas.FindAsync(id)
        is Calima calima
            ? Results.Ok(calima)
            : Results.NotFound());

app.MapPost("/calima", async (Calima calima, CalimaDB db) =>
{
  db.Calimas.Add(calima);
  await db.SaveChangesAsync();

  return Results.Created($"/calima/{calima.Id}", calima);
});

app.MapPut("/calima/{id}", async (int id, Calima calimaInput, CalimaDB db) =>
{
  var calima = await db.Calimas.FindAsync(id);

  if (calima is null) return Results.NotFound();

  calima.Name = calimaInput.Name;
  calima.IsComplete = calimaInput.IsComplete;

  await db.SaveChangesAsync();

  return Results.NoContent();
});

app.MapDelete("/calima/{id}", async (int id, CalimaDB db) =>
{
  if (await db.Calimas.FindAsync(id) is Calima calima)
  {
    db.Calimas.Remove(calima);
    await db.SaveChangesAsync();
    return Results.NoContent();
  }

  return Results.NotFound();
});

app.Run();
