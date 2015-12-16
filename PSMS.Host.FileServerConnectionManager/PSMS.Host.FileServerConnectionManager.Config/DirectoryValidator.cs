using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSMS.Host.FileServerConnectionManager.Config
{
    abstract public class DirectoryValidator
    {
        abstract public string IsValid(String directory);
    }
}
