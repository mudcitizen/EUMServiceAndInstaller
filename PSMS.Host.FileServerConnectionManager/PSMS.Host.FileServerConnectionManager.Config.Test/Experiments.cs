using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Data.Sql;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSMS.Host.FileServerConnectionManager.Config.Test
{
    [TestClass]
    public class Experiments
    {
        [TestMethod]
        public void TestMessageBox()
        {
            DialogResult dr = MessageBox.Show("VFP Dataset?", "Caption", MessageBoxButtons.YesNoCancel);
            String txt;
            switch (dr)
            {
                case DialogResult.Yes:
                    {
                        txt = "Yes";
                        break;
                    }
                case DialogResult.No:
                    {
                        txt = "No";
                        break;
                    }
                default:
                    {
                        txt = "default / cancel";
                        break;
                    }
            }

            Debug.WriteLine(txt);
            Debug.WriteLine("Done");

        }

        [TestMethod]
        public void TestListSqlServers()
        {
            DataTable dt = SqlDataSourceEnumerator.Instance.GetDataSources();
            IList<SqlServer> servers = new List<SqlServer>();
            foreach (DataRow row in dt.Rows) {
                SqlServer ss = new SqlServer(
                    row[SqlServer.ServerNameColumnName].ToString(),
                    row[SqlServer.InstanceNameColumnName].ToString(),
                    row[SqlServer.IsClusteredColumnName].ToString(),
                    row[SqlServer.VersionColumnName].ToString()
                    );
                servers.Add(ss);
            }

            foreach (SqlServer ss in servers)
                Debug.WriteLine(ss);

            Debug.WriteLine("Done");

        }
    }

    class SqlServer
    {
        public const String ServerNameColumnName = "ServerName";
        public const String InstanceNameColumnName = "InstanceName";
        public const String VersionColumnName = "Version";
        public const String IsClusteredColumnName = "IsClustered";

        public String ServerName { get; }
        public String InstanceName { get; }
        public String Version { get; }
        public String IsClustered { get; }

        internal SqlServer(String serverName, String instanceName, String isClustered, String version)
        {
            ServerName = serverName;
            InstanceName = instanceName;
            IsClustered = isClustered;
            Version = version;
        }

        public override string ToString()
        {
            return String.Format("Server - {0} ; Instance - {1} ; Version {2} ; IsCluserted - {3}", ServerName, InstanceName, Version, IsClustered);
        }
    }
}
