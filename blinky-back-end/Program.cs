using Microsoft.AspNetCore.Mvc;
using Models;
using RoomController;

#region BuilderConfig
var builder = WebApplication.CreateBuilder(args);
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

//Create dummy room 

Room room = new Room();
room.RoomId = "RoomA";
room.desks = new Desk[] { new Desk { deskId = "D1", assignedTo = "Bob" }, new Desk { deskId = "D2" }, new Desk { deskId = "D3", assignedTo = "Sarah" } };

Room[] rooms = new Room[] { room };

RController roomController = new RController(rooms);

app.MapPost("/GetAllRoomIds", () =>
{
    return roomController.GetAllRoomIds();
})
.Produces<GetRoomIdsResponse>(StatusCodes.Status200OK);

app.MapPost("/GetRoomData", (GetRoomRequest request) =>
{
    GetRoomDataResponse? response = roomController.GetRoomData(request);
    return response switch
    {
        null => Results.NotFound(),
        _ => TypedResults.Ok(response)
    };
})
.Produces<GetRoomDataResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.MapPost("/BookDesk", (BookDeskRequest request) =>
{
    return roomController.BookDesk(request) switch
    {
        true => Results.Accepted(),
        false => Results.Conflict()
    };
})
.Produces(StatusCodes.Status202Accepted)
.Produces(StatusCodes.Status409Conflict);

app.MapPost("/DeleteAllBookingsInRoom", (GetRoomRequest request) =>
{
    return roomController.DeleteAllBookingsInRoom(request) switch
    {
        true => Results.Accepted(),
        false => Results.NotFound()
    };
})
.Produces(StatusCodes.Status202Accepted)
.Produces(StatusCodes.Status404NotFound);



app.Run();
