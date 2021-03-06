﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSMS.Host.FileServerConnectionManager.Config
{
    public interface IConnectionStringValidator
    {
        String Validate(String connectionString);

        DbType GetDbType(String connectionString);
    }
}
