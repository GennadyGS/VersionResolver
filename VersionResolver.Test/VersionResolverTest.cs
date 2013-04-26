using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VersionResolver.Test
{
  [TestClass]
  public class VersionResolverTest
  {
    [TestMethod]
    public void TestEmpty()
    {
      Assert.AreEqual("", VersionResolver.Versions["UnexistingProperty"]);
    }

    [TestMethod]
    public void TestVersionsEmpty()
    {
      VersionResolver.LoadFiles();
      Assert.AreEqual("", VersionResolver.Versions["UnexistingProperty"]);
    }

    [TestMethod]
    public void TestVersionsUnexisting()
    {
      VersionResolver.LoadFiles("UnexistingFile");
      Assert.AreEqual("", VersionResolver.Versions["UnexistingProperty"]);
    }

    [TestMethod]
    public void TestVersionsDefault()
    {
      const string defaultValue = "Default";
      VersionResolver.DefaultValue = defaultValue;
      Assert.AreEqual(defaultValue, VersionResolver.Versions["UnexistingProperty"]);
    }

    [TestMethod]
    public void TestVersions()
    {
      VersionResolver.LoadFiles(@"..\..\..\TestData\versions.xml", @"..\..\..\TestData\revisions.xml");
      Assert.AreEqual("1", VersionResolver.Versions["MajorVersion"]);
      Assert.AreEqual("2", VersionResolver.Versions["MinorVersion"]);
      Assert.AreEqual("3", VersionResolver.Versions["Release"]);
      Assert.AreEqual("5", VersionResolver.Versions["Revision"]);
      Assert.AreEqual("abcdef", VersionResolver.Versions["RevisionDetailed"]);
    }

    [TestMethod]
    public void TestBuildAssemblyVersion()
    {
      VersionResolver.LoadFiles(@"..\..\..\TestData\versions.xml", @"..\..\..\TestData\revisions.xml");
      Assert.AreEqual("1.2.3.5", VersionResolver.BuildAssemblyVersion("MajorVersion", "MinorVersion", "Release", "Revision"));
    }

  }
}
