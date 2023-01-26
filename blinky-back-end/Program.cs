using Microsoft.AspNetCore.Mvc;
using Blinky_Back_End.Model;
using Microsoft.EntityFrameworkCore;

using Blinky_Back_End;
public class Program
{
    public static void Main(string[] args)
    {
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

        var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider.GetService<DeskDb>();
        SeedInitaliseDeskData(service);

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

        app.MapPost("/BookDesk", async (DeskDb db, string DeskId, string AssignedName) =>
        {
            try
            {
                var desk = await db.desks.FindAsync(DeskId);
                if (desk.AssignedName == null) { return Results.Conflict(); }
                desk.AssignedName = AssignedName;
                desk.IsAvailable = false;
                await db.SaveChangesAsync();
                return Results.Ok();
            }
            catch
            {
                return Results.BadRequest();
            }
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status400BadRequest);

        app.MapPost("/AddDesk", async (Desk desk, DeskDb db) =>
        {
            try
            {
                db.desks.Add(desk);
                await db.SaveChangesAsync();
                return Results.Created($"/AddDesk", desk);
            }
            catch
            {
                return Results.BadRequest();
            }
        })
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        app.MapPost("/RemoveBooking", async (DeskDb db, Desk RequestDesk) =>
        {
            try
            {
                var desk = await db.desks.FindAsync(RequestDesk.DeskId);
                desk.AssignedName = null;
                await db.SaveChangesAsync();
                return Results.Ok();
            }
            catch
            {
                return Results.BadRequest();
            }
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        app.Run();


        async void SeedInitaliseDeskData(DeskDb db)
        {
            for (int i = 0; i < 16; i++)
            {
                db.Add(new Desk { DeskId = "desk" + i.ToString(), AssignedName = "", IsAvailable = true });
            }
            await db.SaveChangesAsync();
        }
    }

}

