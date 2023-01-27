using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blinky_Back_End.DbModels;
using Blinky_Back_End;
public class Program
{
    public static void Main(string[] args)
    {
        #region BuilderConfig
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("db");
        builder.Services.AddDbContext<BookingDb>(opt => opt.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString)
        ));
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
        builder.Services.Configure<JsonOptions>(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
        #endregion

        #region appSetup

        var app = builder.Build();
        var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider.GetService<BookingDb>();
        //SeedInitaliseDeskData(service);

        app.UseCors();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint(
            "/swagger/v1/swagger.json", "v1"
        ));
        app.MapHealthChecks("/healthz");
        #endregion

        app.MapGet("/Rooms", async (BookingDb db) =>
        {
            var rooms = await db.rooms.ToListAsync();
            return Results.Ok(new RoomsResponse { Rooms = rooms });
        })
        .Produces<RoomsResponse>(StatusCodes.Status200OK);

        app.MapGet("/Rooms/{roomId}", async (BookingDb db, Guid roomId, DateOnly? date) =>
        {
            List<Booking> bookedDesks = new List<Booking>();
            var searchDate = date ?? DateOnly.FromDateTime(DateTime.Now);
            var allDesks = await db.desks.Where((x) => x.Room.Id == roomId).ToListAsync();
            bookedDesks = await db.bookings.Where((x) => x.Desk.Room.Id == roomId && x.Date == searchDate).ToListAsync();
            ViewDesksResponse response = new ViewDesksResponse(bookedDesks, allDesks);
            return Results.Ok(response);
        })
        .Produces<ViewDesksResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        app.MapPost("/book", async (BookingDb db, Guid deskId, string userName, DateOnly date) =>
        {
            var desk = await db.desks.SingleOrDefaultAsync<Desk>(d => d.Id == deskId);
            if (desk == null) { return Results.BadRequest(); }
            Booking booking = new()
            {
                Id = Guid.NewGuid(),
                Desk = desk,
                AssignedName = userName,
                Date = date
            };
            db.bookings.Add(booking);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Results.Conflict();
            }
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status409Conflict);

        app.Run();

        async void SeedInitaliseDeskData(BookingDb db)
        {
            Room r = new Room();
            r.Name = "TestRoom";
            db.rooms.Add(r);
            for (int i = 0; i < 9; i++)
            {
                db.desks.Add(new Desk { Name = "test" + i.ToString(), Room = r });
            }
            await db.SaveChangesAsync();
        }
    }

}

