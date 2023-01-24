using Microsoft.AspNetCore.Mvc;
using Model;
using Microsoft.EntityFrameworkCore;

#region BuilderConfig
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DeskDb>(opt => opt.UseInMemoryDatabase("DeskList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin();
        }
    );
});
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Blinky-Backend", Version = "v1" });
});
#endregion

#region appSetup

var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint(
    "/swagger/v1/swagger.json", "v1"
));
app.MapHealthChecks("/healthz");
#endregion

app.MapGet("/AllDesks", async (DeskDb db) =>
{
    AllDesksResponse response = new AllDesksResponse();
    response.desks = await db.desks.ToListAsync();
    return Results.Ok(response);
})
.Produces<AllDesksResponse>(StatusCodes.Status200OK);

app.MapPost("/BookDesk", async (DeskDb db, Desk requestDesk) =>
{
    var desk = await db.desks.FindAsync(requestDesk.deskId);
    if (desk.isAvailable == false) { return Results.Conflict(); }
    desk.assignedName = requestDesk.assignedName;
    desk.isAvailable = false;
    await db.SaveChangesAsync();
    return Results.Accepted();
})
.Produces(StatusCodes.Status202Accepted)
.Produces(StatusCodes.Status409Conflict);

app.MapPut("/AddDesk", async (Desk desk, DeskDb db) =>
{
    db.desks.Add(desk);
    await db.SaveChangesAsync();
    return Results.Created($"/AddDesk", desk);
})
.Produces(StatusCodes.Status201Created);

app.Run();
