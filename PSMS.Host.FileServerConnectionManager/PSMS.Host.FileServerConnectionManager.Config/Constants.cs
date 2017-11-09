using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSMS.Host.FileServerConnectionManager.Config
{
    public static class Constants
    {
        public static class ConfigTokenNames
        {
            public static String DbType
            {
                get
                {
                    return "{" + PSMS.Host.WMI.Common.Constants.AppSettingKeys.DbType + "}";
                }
            }

            public static String DbName
            {
                get
                {
                    return "{" + PSMS.Host.WMI.Common.Constants.AppSettingKeys.DbName + "}";
                }
            }
            public static String HostDirectory { get { return "{HostDirectory}"; } }
            public static String SqlServerName { get { return "{SqlServerName}"; } }
            public static String SqlDbName { get { return  "{SqlDbName}"; } }
        }

        public static class ConfigTokenValues
        {
            public static String DbTypeVfp
            {
                get
                {
                    return PSMS.Host.WMI.Common.Constants.AppSettingValues.VfpDbType ;
                }
            }

            public static String DbTypeSql
            {
                get
                {
                    return PSMS.Host.WMI.Common.Constants.AppSettingValues.SqlDbType;
                }
            }

            public static String DbNameVfp
            {
                get
                {
                    return PSMS.Host.WMI.Common.Constants.ConnectionNames.Vfp;
                }
            }

            public static String DbNameSql
            {
                get
                {
                    return PSMS.Host.WMI.Common.Constants.ConnectionNames.Sql;
                }
            }


        }

        public static class ConnectionStringDetails
        {
            public static String VfpProviderPropertyName
            {
                get { return "Provider";
                }
            }
            public static String VfpProviderPropertyValue
            {
                get
                {
                    return "VFPOLEDB.1";
                }
            }
            public static String InitialCatalogPropertyName
            {
                get
                {
                    return "Initial Catalog";
                }
            }
            public static String DataSourcePropertyName
            {
                get
                { return "Data Source"; }
            }

            public static String SqlProviderPropertyValue => "System.Data.SqlClient";
        }
    }
}
