using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSMS.Host.FileServerConnectionManager.Config;
using System.Data.Common;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using PSMS.Host.FileServerConnectionManager.Config;


namespace PSMS.Host.FileServerConnectionManager.Config.Test
{
    [TestClass]
    public class TestConnectionStringValidator
    {

        DbConnectionStringBuilder _vfpConnStrBldr;
        DbConnectionStringBuilder _sqlConnStrBldr;
        String _testedConnectionString;

        Properties.Settings _settings = Properties.Settings.Default;

        const String _dataSourceLiteral = "Data Source";

        [TestInitialize]
        public void Setup()
        {
            _vfpConnStrBldr = new OleDbConnectionStringBuilder();
            _sqlConnStrBldr = new SqlConnectionStringBuilder();
            _testedConnectionString = null;
                 
        }

        [TestMethod]
        public void TestEmpty()
        {
            _sqlConnStrBldr.ConnectionString = String.Empty;
            String s = Validate(_sqlConnStrBldr);
            Assert.AreEqual(s, _settings.ConnectionStringCannotBeEmpty);

        }

        [TestMethod]
        public void TestValidVfpConnectionString()
        {
            const String folder = @"D:\vhost\";
            Assert.IsTrue(Directory.Exists(folder));
            const string dbf = "syeumreq.dbf";
            Assert.IsTrue(File.Exists(Path.Combine(folder, dbf)));

            //_vfpConnStrBldr[_dataSourceLiteral] = @"C:\" + Guid.NewGuid().ToString();
            _vfpConnStrBldr.ConnectionString = String.Format("Provider = VFPOLEDB.1; Data Source = {0}; Collating Sequence = general;", folder);
            String s = Validate(_vfpConnStrBldr);
            Assert.IsTrue(String.IsNullOrEmpty(s));

        }

        [TestMethod]
        public void TestValidSqlConnectionString()
        {
            const String connStr = "Data Source = LANEK-LT; Initial Catalog = LAPTOP_21_000825;Integrated Security=True";
            _sqlConnStrBldr.ConnectionString = connStr;
            String s = Validate(_sqlConnStrBldr);
            Assert.IsTrue(String.IsNullOrEmpty(s));
        }

        [TestMethod]
        public void TestInvalidSqlConnectionStringNoSyeumreq()
        {
            const String connStr = "Data Source = LANEK-LT; Initial Catalog = ReportServer;Integrated Security=True";
            _sqlConnStrBldr.ConnectionString = connStr;
            String s = Validate(_sqlConnStrBldr);
            Assert.IsFalse(String.IsNullOrEmpty(s));
            Assert.IsTrue(s.ToUpper().Contains("syeumreq".ToUpper()));
        }


        String Validate(DbConnectionStringBuilder connStrBldr) {

            IConnectionStringValidator csv = new ConnectionStringValidator();
            _testedConnectionString = connStrBldr.ConnectionString;
            return csv.Validate(_testedConnectionString);
        }
    }
}
