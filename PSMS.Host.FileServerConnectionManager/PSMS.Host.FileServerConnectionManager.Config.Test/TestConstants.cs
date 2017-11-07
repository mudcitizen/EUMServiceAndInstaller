using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSMS.Host.FileServerConnectionManager.Config.Test
{
    [TestClass]
    public class TestConstants
    {
        [TestMethod]
        public void TestValues()
        {
            Debug.WriteLine("TestValues()");
            Assert.AreEqual(Constants.ConfigTokenNames.DbType, "{DbType}");
            Assert.AreEqual(Constants.ConfigTokenNames.DbName, "{DbName}");
            Assert.AreEqual(Constants.ConfigTokenValues.DbTypeVfp, "VFP");
            Assert.AreEqual(Constants.ConfigTokenValues.DbTypeSql, "SQL");
            Assert.AreEqual(Constants.ConfigTokenValues.DbNameVfp, "HostDb");
            Assert.AreEqual(Constants.ConfigTokenValues.DbNameSql, "HostSql");
        }

    }
}
