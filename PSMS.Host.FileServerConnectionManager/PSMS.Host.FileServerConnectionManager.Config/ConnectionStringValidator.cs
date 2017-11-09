using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sms.ConnectionUI.VfpDataProvider;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using PSMS.Utility.Data.DbHandler;

namespace PSMS.Host.FileServerConnectionManager.Config
{
    public class ConnectionStringValidator : IConnectionStringValidator
    {
        public string Validate(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                return Properties.Settings.Default.ConnectionStringCannotBeEmpty;

            IList<String> requiredProps = new List<String>();
            requiredProps.Add(Sms.ConnectionUI.VfpDataProvider.Constants.ConnectionStringDetails.DataSourcePropertyName);

            DbConnectionStringBuilder connectionStringBldr;

            DbType dbType = GetDbType(connectionString);

            String errMsg = null;

            /*
                < add name = "HostDb" connectionString = "Provider=VFPOLEDB.1;Data Source={HostFolder};Collating Sequence=general;" providerName = "System.Data.OleDb" />     
                < add name = "HostSql" connectionString = "Data Source=LANEK-LT;Initial Catalog=LAPTOP_21_000511;User ID=PSMSUsr;Password=$1koj1@VGR2" providerName = "System.Data.SqlClient" />
            */


            if (dbType == DbType.Vfp)
            {
                connectionStringBldr = new OleDbConnectionStringBuilder();
            }
            else
            {
                connectionStringBldr = new SqlConnectionStringBuilder();
                requiredProps.Add(Constants.ConnectionStringDetails.InitialCatalogPropertyName);
            }

            try
            {
                connectionStringBldr.ConnectionString = connectionString;
                foreach (String prop in requiredProps)
                    if ((connectionStringBldr[prop] == null) || (connectionStringBldr[prop].ToString() == String.Empty))
                    {
                        errMsg = String.Format(Properties.Settings.Default.ConnectionStringMissingRequiredProperty, prop);
                        break;
                    }
            }

            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            String dataSource = String.Empty;
            if (String.IsNullOrEmpty(errMsg))
            {
                dataSource = connectionStringBldr[Constants.ConnectionStringDetails.DataSourcePropertyName].ToString();
            }
           

            if (String.IsNullOrEmpty(errMsg))
            {

                const String requiredTableName = "syeumreq";

                if (dbType == DbType.Vfp)
                {

                    List<DirectoryValidator> dvs = new List<DirectoryValidator>();
                    dvs.Add(new RequiredFileDirectoryValidator(Path.ChangeExtension(requiredTableName, "dbf")));
                    dvs.Add(new DriveTypeDirectoryValidator(DriveType.Fixed));

                    foreach (DirectoryValidator dv in dvs)
                    {
                        errMsg = dv.IsValid(dataSource);
                        if (!String.IsNullOrEmpty(errMsg))
                        {
                            break;
                        }
                    }

                }
                else
                {
                    /*
                     * The following code works in a UnitTest - but we can't use it in the Installer because 
                     * the installer runs as NT Authority\SYSTEM and may not have access to the DB.   When this 
                     * is the case the install fails - even with try {} block .... 
                     * 
                     * By default NT Authority\SYSTEM only has the Public Server Role.  And at the DB level 
                     * Public can't do jack...

                    // Make sure SYEUMREQ.DBF exists in the DB

                    const String parmName = "@TableName";
                    String dbName = connectionStringBldr[Constants.ConnectionStringDetails.InitialCatalogPropertyName].ToString();
                    String cmdStr = String.Format("SELECT COUNT(*) FROM {0}.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {1}", dbName, parmName);
                    using (SqlCommand sqlCmd = new SqlCommand(cmdStr))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter(parmName, requiredTableName));
                        DbHandler dbHandler = new DbHandler(connectionString, Constants.ConnectionStringDetails.SqlProviderPropertyValue);

                        if ((int)dbHandler.DbExecuteScalar(sqlCmd) == 0)
                        {
                            errMsg = String.Format(Properties.Settings.Default.InvalidDB, requiredTableName);
                        }
                    }

                    */
                }

            }

            else

            {
                errMsg = Properties.Settings.Default.InvalidConnectionString;
            }

            return errMsg;
        }


        public DbType GetDbType(String connectionString) {
            if ((connectionString.IndexOf(Constants.ConnectionStringDetails.VfpProviderPropertyName) >= 0) && (connectionString.IndexOf(Constants.ConnectionStringDetails.VfpProviderPropertyValue) >= 0))
                return DbType.Vfp;
            else
                return DbType.Sql;

        }
    }
}
