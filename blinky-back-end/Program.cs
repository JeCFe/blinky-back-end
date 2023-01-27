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
        builder.Services.AddDbContext<BookingDb>(opt => opt.UseInMemoryDatabase("DeskList"));
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
        var service = scope.ServiceProvider.GetService<BookingDb>();
        SeedInitaliseDeskData(service);
        Console.WriteLine(JsonSerializer.Serialize(service));

        app.UseCors();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint(
            "/swagger/v1/swagger.json", "v1"
        ));
        app.MapHealthChecks("/healthz");
        #endregion

        /*  app.MapPost("/AllDesks", async (BookingDb db, AllDesksRequest data) =>
         { */
        /* string key = "123";

        try
        {
            Console.WriteLine(JsonSerializer.Serialize(db.DateDesks));
            var period = await db.DateDesks.Include((x) => x.desks).FirstAsync((booking) => booking.Id == key);
            AllDesksResponse response = new AllDesksResponse();
            Console.WriteLine("here");

            Console.WriteLine(JsonSerializer.Serialize(period));
            BookingDate date = new BookingDate { day = period.day, month = period.month, year = period.year };
            response.date = date;
            //response.test = period.desks.Count();
            //Console.WriteLine(period.desks);
            response.desks = period.desks;
            await db.SaveChangesAsync();
            return Results.Ok(response);
        }
        catch (Exception error)
        {
            Console.WriteLine(error.Message);
            return Results.BadRequest();
            /* SeedInitaliseDeskData(db, data.date);
            var period = await db.DateDesks.AsNoTracking().FirstAsync((booking) => booking.BookingId == key);
            AllDesksResponse response = new AllDesksResponse();
            BookingDate date = new BookingDate { day = period.day, month = period.month, year = period.year };
            response.date = date;
            response.desks = period.desks;
            await db.SaveChangesAsync();
            return Results.Ok(response); */
        //}
        //});
        //.Produces<AllDesksResponse>(StatusCodes.Status200OK);

        /* app.MapPost("/BookDesk", async (BookingDb db, BookDeskRequest booking) =>
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

        app.MapPost("/AddDesk", async (Desk desk, BookingDb db) =>
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

        app.MapPost("/RemoveBooking", async (BookingDb db, Desk RequestDesk) =>
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
        .Produces(StatusCodes.Status400BadRequest); */

        app.Run();


        async void SeedInitaliseDeskData(BookingDb db)
        {

            /*             Room r = new Room();
                        r.Name = "Room1";

                        db.rooms.Add(r);
                        db.SaveChangesAsync(); */


            /* BookingLayout entry = new BookingLayout();
            entry.desks = new List<Desk>();
            if (date == null)
            {
                entry.day = "1";
                entry.month = "2";
                entry.year = "1999";
            }
            else
            {
                entry.day = date.day;
                entry.month = date.month;
                entry.year = date.year;
            }
            entry.Id = "123";

            for (int i = 0; i < 16; i++)
            {
                string dName = "desk" + i.ToString();
                entry.desks.Add(new Desk { Id = dName + entry.Id, DeskName = dName, AssignedName = "", IsAvailable = true });
            }
            //Console.WriteLine(entry);
            //Console.WriteLine(JsonSerializer.Serialize(entry));

            db.Add(entry);

            await db.SaveChangesAsync();
            //Console.WriteLine(JsonSerializer.Serialize(db.DateDesks));
            var period = await db.DateDesks.FindAsync("123");
            Console.WriteLine(JsonSerializer.Serialize(period)); */

        }
    }

}

