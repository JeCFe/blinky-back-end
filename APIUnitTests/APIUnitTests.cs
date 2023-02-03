using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

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
    public async Task AddRooms()
    {
        using var client = app.CreateClient();
        var res = await client.PostAsJsonAsync("/GenerateRoom?RoomName=London&AmountOfDesks=20", new { });
        Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
    }
}