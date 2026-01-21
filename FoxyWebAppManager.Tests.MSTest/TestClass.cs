using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using FoxyWebAppManager.Helpers;
using Microsoft.UI.Xaml.Controls;

namespace FoxyWebAppManager.Tests.MSTest;

// TODO: Write unit tests.
// https://docs.microsoft.com/visualstudio/test/getting-started-with-unit-testing
// https://docs.microsoft.com/visualstudio/test/using-microsoft-visualstudio-testtools-unittesting-members-in-unit-tests
// https://docs.microsoft.com/visualstudio/test/run-unit-tests-with-test-explorer

[TestClass]
public class TestClass
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        Debug.WriteLine("ClassInitialize");
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Debug.WriteLine("ClassCleanup");
    }

    [TestInitialize]
    public void TestInitialize()
    {
        Debug.WriteLine("TestInitialize");
    }

    [TestCleanup]
    public void TestCleanup()
    {
        Debug.WriteLine("TestCleanup");
    }

    [TestMethod]
    public void TestMethod()
    {

        var profiles = Helpers.FireFoxIniParser.LoadProfilesFromInstalledFF();

        profiles.RemoveAt(1);

        profiles.AttachProfilesToIniFile();


        Assert.IsTrue(true);
    }

    [UITestMethod]
    public void UITestMethod()
    {
        Assert.AreEqual(0, new Grid().ActualWidth);
    }

    [TestMethod]
    public async Task WebTestMethod()
    {
        var uri = new Uri("https://www.crunchyroll.com");
        
        using var client = new HttpClient();
	    client.DefaultRequestHeaders.Clear();
        client.DefaultRequestVersion = HttpVersion.Version20;
        client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
	
		client.DefaultRequestHeaders.Add("User-Agent", "FoxyWebAppManager-TestClient");
        client.DefaultRequestHeaders.Add("Accept", "*/*");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

		var req = await client.GetAsync(uri,HttpCompletionOption.ResponseContentRead);
        req.EnsureSuccessStatusCode();  


		//Assert.AreEqual();
	}
}
