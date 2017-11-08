using Sms.ConnectionUI.Client;
using Sms.ConnectionUI.VfpDataProvider;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSMS.Host.FileServerConnectionManager.Config
{
    public class Configurator
    {

        IConnectionStringProvider _ConnectionStringProvider;
        IConnectionStringValidator _ConnectionStringValidator; 

        String _Slash = Path.DirectorySeparatorChar.ToString();

        public Configurator() : this(null, null) { }

        public Configurator(IConnectionStringProvider connectionStringProvider, IConnectionStringValidator connectionStringValidator)
        {
            _ConnectionStringProvider = connectionStringProvider ?? new EumInstallClient(); 
            _ConnectionStringValidator = connectionStringValidator ?? new ConnectionStringValidator();
        }

        public void Configure(String targetDir)
        {

            String errMsg = "Not empty";

            while (!String.IsNullOrEmpty(errMsg))
            {

                String connStr = _ConnectionStringProvider.GetConnectionString();
                errMsg = _ConnectionStringValidator.Validate(connStr);

                if (String.IsNullOrEmpty(errMsg))
                {
                    String[] configFiles = Directory.GetFiles(targetDir, "*.config");
                    IConfigurationTextUpdater configUpdater = new ConfigurationTextUpdater();

                    DbType dbType = _ConnectionStringValidator.GetDbType(connStr);
                    IDictionary<String, String> configTerms = new Dictionary<String, String>();

                    String dbTypeString, dbNameString;
                    DbConnectionStringBuilder connStrBldr;

                    if (dbType == DbType.Vfp)
                    {
                        connStrBldr = new OleDbConnectionStringBuilder(connStr);
                        configTerms.Add(new KeyValuePair<String, String>(Constants.ConfigTokenNames.HostDirectory,connStrBldr[Constants.ConnectionStringDetails.DataSourcePropertyName].ToString()));

                        dbTypeString = Constants.ConfigTokenValues.DbTypeVfp;
                        dbNameString = Constants.ConfigTokenValues.DbNameVfp;
                    }
                    else
                    {
                        connStrBldr = new SqlConnectionStringBuilder(connStr);
                        configTerms.Add(new KeyValuePair<String, String>(Constants.ConfigTokenNames.SqlServerName, connStrBldr[Constants.ConnectionStringDetails.DataSourcePropertyName].ToString()));
                        configTerms.Add(new KeyValuePair<String, String>(Constants.ConfigTokenNames.SqlDbName, connStrBldr[Constants.ConnectionStringDetails.InitialCatalogPropertyName].ToString()));

                        dbTypeString = Constants.ConfigTokenValues.DbTypeSql;
                        dbNameString = Constants.ConfigTokenValues.DbNameSql;
                    }

                    configTerms.Add(new KeyValuePair<String, String>(Constants.ConfigTokenNames.DbType,dbTypeString));
                    configTerms.Add(new KeyValuePair<String, String>(Constants.ConfigTokenNames.DbName,dbNameString));

                    foreach (String fileName in configFiles)
                    {
                        IEnumerable<String> configIn = File.ReadAllLines(fileName);
                        IEnumerable<String> configOut = configUpdater.Update(configIn,configTerms);
                        if (configOut != null)
                        {
                            File.WriteAllLines(fileName, configOut);
                        }
                    }

                }

                else

                {
                    MessageBox.Show(errMsg);
                }

            }

        }

        String StripBackslash(String dirName)
        {
            while (dirName.EndsWith(@"\"))
            {
                String s = String.Format("{0} - {1}", dirName, dirName.Length);
                dirName = dirName.Remove(dirName.Length - 1, 1);
            }
            return dirName;
        }


    }
}
