using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Model;
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
        var res = await client.PostAsJsonAsync("/BookDesk?deskId=desk7&assignedName=Jessica", new { });
        Assert.AreEqual(HttpStatusCode.Accepted, res.StatusCode);
    }


}
