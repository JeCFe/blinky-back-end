using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Blinky_Back_End.DbModels;
using Tests.Model;

namespace APIUnitTests;

[TestClass]
public class UnitTest
{
    private TestContext testContextInstance;
    private WebApplicationFactory<Program> app = new WebApplicationFactory<Program>();


    /// <summary>
    /// Gets or sets the test context which provides
    /// information about and functionality for the current test run.
    /// </summary>
    public TestContext TestContext
    {
        get { return testContextInstance; }
        set { testContextInstance = value; }
    }

    [TestMethod]
    public async Task AddRoomsOK()
    {
        using var client = app.CreateClient();
        var res = await client.PostAsJsonAsync("/GenerateRoom?RoomName=London&AmountOfDesks=20", new { });
        Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
    }
    [TestMethod]
    public async Task AddRoomsBadRequest()
    {
        using var client = app.CreateClient();
        var res = await client.PostAsJsonAsync("/GenerateRoom", new { });
        Assert.AreEqual(HttpStatusCode.BadRequest, res.StatusCode);
    }
    [TestMethod]
    public async Task ViewRooms()
    {
        using var client = app.CreateClient();
        await client.PostAsJsonAsync("/GenerateRoom?RoomName=London&AmountOfDesks=20", new { });
        var res = await client.GetAsync("/Rooms");
        RoomsResponse data = await res.Content.ReadFromJsonAsync<RoomsResponse>();
        Assert.IsNotNull(data);
        Assert.AreEqual(data.Rooms.Count(), 2);
        Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
    }

    [TestMethod]
    public async Task ViewRoomWithIdandDate()
    {
        using var client = app.CreateClient();
        await client.PostAsJsonAsync("/GenerateRoom?RoomName=London&AmountOfDesks=20", new { });
        var rooms = await client.GetAsync("/Rooms");
        RoomsResponse roomsData = await rooms.Content.ReadFromJsonAsync<RoomsResponse>();

        string req = "/Rooms/" + roomsData.Rooms.First().Id;

        var singleRoom = await client.GetAsync(req);
        TestViewDeskResponse singleRoomData = await singleRoom.Content.ReadFromJsonAsync<TestViewDeskResponse>();

        Assert.AreEqual(HttpStatusCode.OK, singleRoom.StatusCode);
        Assert.AreEqual(singleRoomData.DesksAvailability.Count(), 20);
    }

    [TestMethod]
    public async Task ViewRoomWithIdandDateSadPath()
    {
        using var client = app.CreateClient();
        var singleRoom = await client.GetAsync("/Rooms/NonExistingRoom");
        Assert.AreEqual(HttpStatusCode.BadRequest, singleRoom.StatusCode);
    }

    [TestMethod]
    public async Task BookDeskHappyPath()
    {
        using var client = app.CreateClient();
        await client.PostAsJsonAsync("/GenerateRoom?RoomName=London&AmountOfDesks=20", new { });
        var rooms = await client.GetAsync("/Rooms");
        RoomsResponse roomsData = await rooms.Content.ReadFromJsonAsync<RoomsResponse>();
        string req = "/Rooms/" + roomsData.Rooms.First().Id;
        var singleRoom = await client.GetAsync(req);
        TestViewDeskResponse singleRoomData = await singleRoom.Content.ReadFromJsonAsync<TestViewDeskResponse>();
        req = "/book?deskId=" + singleRoomData.DesksAvailability[0].Desk.Id + "&userName=TestJessica&date=2023-10-10";
        var bookData = await client.PostAsJsonAsync(req, new { });
        Assert.AreEqual(HttpStatusCode.OK, bookData.StatusCode);
    }
    [TestMethod]
    public async Task BookDeskSadPath()
    {
        using var client = app.CreateClient();
        await client.PostAsJsonAsync("/GenerateRoom?RoomName=London&AmountOfDesks=20", new { });
        var rooms = await client.GetAsync("/Rooms");
        RoomsResponse roomsData = await rooms.Content.ReadFromJsonAsync<RoomsResponse>();
        string req = "/Rooms/" + roomsData.Rooms.First().Id;
        var singleRoom = await client.GetAsync(req);
        TestViewDeskResponse singleRoomData = await singleRoom.Content.ReadFromJsonAsync<TestViewDeskResponse>();
        req = "/book?";
        var bookData = await client.PostAsJsonAsync(req, new { });
        Assert.AreEqual(HttpStatusCode.BadRequest, bookData.StatusCode);
    }

    [TestMethod]
    public async Task UpdatePosition()
    {
        using var client = app.CreateClient();
        await client.PostAsJsonAsync("/GenerateRoom?RoomName=London&AmountOfDesks=20", new { });
        var rooms = await client.GetAsync("/Rooms");
        RoomsResponse roomsData = await rooms.Content.ReadFromJsonAsync<RoomsResponse>();

        string req = "/Rooms/" + roomsData.Rooms.First().Id;

        var singleRoom = await client.GetAsync(req);
        TestViewDeskResponse singleRoomData = await singleRoom.Content.ReadFromJsonAsync<TestViewDeskResponse>();

        req = "/updatePosition?deskId=" + singleRoomData.DesksAvailability[0].Desk.Id + "&x=200&y=200";
        var updatingPosition = await client.PostAsJsonAsync(req, new { });
        Assert.AreEqual(HttpStatusCode.OK, updatingPosition.StatusCode);
    }

}