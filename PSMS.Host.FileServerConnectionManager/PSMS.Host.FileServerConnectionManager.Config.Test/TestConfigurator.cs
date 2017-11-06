using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using PSMS.Host.FileServerConnectionManager.Config;
using Sms.ConnectionUI.Client;
using Sms.ConnectionUI.VfpDataProvider;


using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSMS.Host.FileServerConnectionManager.Config.Test
{
    [TestClass]
    public class TestConfigurator
    {

        const String _configTestFileName = @"C:\temp\app.config";
        String _appConfigFileName;
        String _CurrentDirectory;

        public static String VfpFolderName = @"D:\VHOST\";
        public static String SqlServerName = "LANEK-LT";
        public static String SqlDbName = "LAPTOP_21_000511";


        [TestInitialize]
        public void Setup()
        {
            Assert.IsTrue(File.Exists(_configTestFileName));
            _CurrentDirectory = Directory.GetCurrentDirectory();
            _appConfigFileName = Path.Combine(_CurrentDirectory, "app.config");
            if (File.Exists(_appConfigFileName))
                File.Delete(_appConfigFileName);
            File.Copy(_configTestFileName, _appConfigFileName);
            ValidateInputConfigFile();
        }

        [TestMethod]
        public void TestVfpConfig()
        {
            IConnectionStringValidator csv = new TestCsv(DbType.Vfp);
            IConnectionStringProvider csp = new TestVfpConnectionStringProvider();

            Configurator c = new Configurator(csp, csv);
            c.Configure(_CurrentDirectory);

            IList<KeyValuePair<String, String>> terms = new List<KeyValuePair<String, String>>();
            terms.Add(new KeyValuePair<String, String>(StripBraces(Constants.ConfigTokenNames.DbType), Constants.ConfigTokenValues.DbTypeVfp));     // DbType = VFP
            terms.Add(new KeyValuePair<String, String>(StripBraces(Constants.ConfigTokenNames.DbName), Constants.ConfigTokenValues.DbNameVfp));     // DbName = HostDb
            terms.Add(new KeyValuePair<String, String>(StripBraces(Constants.ConfigTokenValues.DbNameVfp), TestConfigurator.VfpFolderName));     // Connection element - HostDb & D:\Vhost\

            IEnumerable<String> lines = File.ReadLines(_appConfigFileName).Where(line => !line.Trim().StartsWith("<!--"));

            foreach (KeyValuePair<String,String> kvp in terms)
            {
                IEnumerable<String> selectedLines = lines.Where(line => line.Contains(kvp.Value) && line.Contains(kvp.Key));
                Assert.AreEqual(1, selectedLines.Count());
                Debug.WriteLine(selectedLines.Take(1).ToList()[0]);
            }
        }

        [TestMethod]
        public void TestSqlConfig()
        {
            IConnectionStringValidator csv = new TestCsv(DbType.Sql);
            IConnectionStringProvider csp = new TestSqlConnectionStringProvider();

            Configurator c = new Configurator(csp, csv);
            c.Configure(_CurrentDirectory);

            IList<KeyValuePair<String, String>> terms = new List<KeyValuePair<String, String>>();
            terms.Add(new KeyValuePair<String, String>(StripBraces(Constants.ConfigTokenNames.DbType), Constants.ConfigTokenValues.DbTypeSql));     // DbType = VFP
            terms.Add(new KeyValuePair<String, String>(StripBraces(Constants.ConfigTokenNames.DbName), Constants.ConfigTokenValues.DbNameSql));     // DbName = HostDb
            terms.Add(new KeyValuePair<String, String>(StripBraces(Constants.ConfigTokenValues.DbNameSql), TestConfigurator.SqlServerName));     // Connection element - HosSql & ServerName
            terms.Add(new KeyValuePair<String, String>(StripBraces(Constants.ConfigTokenValues.DbNameSql), TestConfigurator.SqlDbName));     // Connection element - HosSql & ServerName

            IEnumerable<String> lines = File.ReadLines(_appConfigFileName).Where(line => !line.Trim().StartsWith("<!--"));

            foreach (KeyValuePair<String, String> kvp in terms)
            {
                IEnumerable<String> selectedLines = lines.Where(line => line.Contains(kvp.Value) && line.Contains(kvp.Key));
                Assert.AreEqual(1, selectedLines.Count());
                Debug.WriteLine(selectedLines.Take(1).ToList()[0]);
            }

            // One line with HostSql SqlServer and SqlDb
            Assert.AreEqual(1,
                lines.Where(line => line.Contains(Constants.ConfigTokenValues.DbNameSql) && line.Contains(TestConfigurator.SqlDbName) && line.Contains(TestConfigurator.SqlServerName)).Count()
                );
        }

        void ValidateInputConfigFile()
        {
            IEnumerable<String> lines = File.ReadLines(_appConfigFileName);

            IList<String> terms = new List<String>() { Constants.ConfigTokenNames.DbType, Constants.ConfigTokenNames.DbName };
            // Needs lines with DbName/{DbName} and DbType/{DbType}
            foreach (String term in terms)
                Assert.AreEqual(lines
                    .Where(line => line.Contains(term) && line.Contains(StripBraces(term)))
                    .Count(),1);


            String element = Constants.ConfigTokenValues.DbNameVfp;
            IEnumerable<String> debugLines = lines.Where(line => line.Contains(Constants.ConfigTokenNames.HostDirectory) && line.Contains(element)); 

            Assert.AreEqual(1, lines
                .Where(line => line.Contains(element) && line.Contains(Constants.ConfigTokenNames.HostDirectory)).Count());

            element = Constants.ConfigTokenValues.DbNameSql;
            Assert.AreEqual(1, lines
                .Where(line => line.Contains(element) && line.Contains(Constants.ConfigTokenNames.SqlServerName) && line.Contains(Constants.ConfigTokenNames.SqlDbName)).Count());

        }


        String WrapInQuotes(String term)
        {
            String dblQuote = "\"";
            return dblQuote + term + dblQuote;
        }

        String WrapInBraces(String term)
        {
            return "{" + term + "}";
        }

        String StripBraces(String term)
        {
            return term.Replace("{", "").Replace("}", "");
        }

    }

    #region TestInfrastructureClasses
    class TestCsv : IConnectionStringValidator
    {
        DbType _DbType;

        public TestCsv(DbType dbType)
        {
            _DbType = dbType;
        }

        public DbType GetDbType(string connectionString)
        {
            return _DbType;
        }

        public string Validate(string connectionString)
        {
            return String.Empty;
        }
    }

    class TestVfpConnectionStringProvider : IConnectionStringProvider
    {
        public string GetConnectionString()
        {
            return String.Format("Provider=VFPOLEDB.1;Data Source={0};Collating Sequence=general;", TestConfigurator.VfpFolderName);
        }
    }
    class TestSqlConnectionStringProvider : IConnectionStringProvider
    {
        public string GetConnectionString()
        {
            return String.Format("Data Source={0};Initial Catalog={1};User ID=PSMSUsr;Password=$1koj1@VGR2", TestConfigurator.SqlServerName, TestConfigurator.SqlDbName);
        }
    }
    #endregion

}
