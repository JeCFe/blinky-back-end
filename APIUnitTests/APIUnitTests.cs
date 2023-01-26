using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net.Http.Json;
using Blinky_Back_End.Model;

namespace APIUnitTests;

[TestClass]
public class UnitTest
{
    private WebApplicationFactory<Program> app = new WebApplicationFactory<Program>();

    [TestMethod]
    public async Task GetAllDesksTest()
    {
        using var client = app.CreateClient();
        var res = await client.GetAsync("/AllDesks");
        string data = await res.Content.ReadAsStringAsync();
        AllDesksResponse response = JsonSerializer.Deserialize<AllDesksResponse>(data);
        Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
        Assert.AreEqual(response.desks.Count(), 16);
    }
    [TestMethod]
    public async Task BookADesk()
    {
        using var client = app.CreateClient();
        var res = await client.PostAsJsonAsync("/BookDesk?deskId=Desk7&AssignedName=Jessica", new { });
        Assert.AreEqual(HttpStatusCode.Accepted, res.StatusCode);
    }
}
