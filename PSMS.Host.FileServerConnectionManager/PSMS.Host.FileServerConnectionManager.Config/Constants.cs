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

            public const String HostDirectory = "{HostDirectory}";
            public const String SqlServerName = "{SqlServerName}";
            public const String SqlDbName = "{SqlDbName}";
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
            public const String VfpProviderPropertyName = "Provider";
            public const String VfpProviderPropertyValue = "VFPOLEDB.1";
            public const String InitialCatalogPropertyName = "Initial Catalog";
            public const String DataSourcePropertyName = "Data Source";
        }
    }
}
