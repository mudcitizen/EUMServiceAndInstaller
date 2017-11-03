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
            public const String DbType = "{DbType}";
            public const String DbName = "{DbName}";
            public const String HostDirectory = "{HostDirectory}";
            public const String SqlServerName = "{SqlServerName}";
            public const String SqlDbName = "{SqlDbName}";
        }

        public static class ConfigTokenValues
        {
            public const String DbTypeVfp = "VFP";
            public const String DbTypeSql = "SQL";
            public const String DbNameVfp = "HostDb";
            public const String DbNameSql = "HostSql";
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
