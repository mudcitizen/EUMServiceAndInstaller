using System;
using PSMS.Host.FileServerConnectionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceProcess;

namespace PSMS.Host.FileServerConnectionManager.Tests
{
    [TestClass]
    public class TestService
    {
        [TestMethod]
        public void TestCTOR()
        {
            ServiceBase s = new PSMS.Host.FileServerConnectionManager.Service();
        }
      
    }
}
