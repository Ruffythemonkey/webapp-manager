using System.Diagnostics;
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
}
